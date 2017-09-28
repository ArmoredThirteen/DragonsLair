using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CollisionSystem;

#if UNITY_EDITOR
using UnityEditor;
#endif


public class Trigger_InArea : AteComponent
{
	public enum AreaType
	{
		UnityTriggers = 100,
		//AteCollider = 200,
		CollisionArea = 300,
	}

	public List<GOType> triggeredByTypes = new List<GOType> ();

	public AreaType areaType = AreaType.CollisionArea;
	//public AteCollider ateCollider;
	public CollisionArea collisionArea;


	/// <summary> How many objects of type GOType must be in trigger to be interacted with. </summary>
	public int requiredToInteract = 1;
	/// <summary> Which UI events count as interactions. </summary>
	public EventType_UI eventsToInteract = EventType_UI.None;
	/// <summary> When interaction events fire, checks if the event is from this key. </summary>
	public KeyCode interactKey = KeyCode.Mouse0;

	public List<TriggeredBehaviour> behavioursOnEnter    = new List<TriggeredBehaviour> ();
	public List<TriggeredBehaviour> behavioursOnInteract = new List<TriggeredBehaviour> ();
	public List<TriggeredBehaviour> behavioursOnExit     = new List<TriggeredBehaviour> ();

	private List<AteObject> _currentlyInTrigger = new List<AteObject> ();


	private bool AreEnterBehavioursEmpty
	{
		get {return behavioursOnEnter.Count == 0;}
	}

	private bool AreInteractBehavioursEmpty
	{
		get {return behavioursOnInteract.Count == 0;}
	}

	private bool AreExitBehavioursEmpty
	{
		get {return behavioursOnExit.Count == 0;}
	}

	#if UNITY_EDITOR

	public override void DrawInspector ()
	{
		base.DrawInspector ();

		EditorHelper.DrawResizableList<GOType> ("Triggered by Specific Types", ref triggeredByTypes, DrawEntry_TriggeredByType);
		if (triggeredByTypes.Count <= 0)
			EditorGUILayout.LabelField ("Leave empty to be triggered by all types.");
		
		EditorGUILayout.Space ();
		EditorGUILayout.Space ();

		areaType = (AreaType)EditorGUILayout.EnumPopup ("Triggered Using Area Type", areaType);
		if (areaType.Equals (AreaType.CollisionArea))
		{
			EditorGUI.indentLevel++;
			collisionArea = EditorGUILayout.ObjectField ("Collision Area", collisionArea, typeof(CollisionArea), true) as CollisionArea;
			EditorGUI.indentLevel--;
		}

		requiredToInteract = EditorGUILayout.IntField ("Required to Interact", requiredToInteract);
		eventsToInteract = (EventType_UI)EditorGUILayout.EnumPopup ("Event to Interact", eventsToInteract);
		interactKey = (KeyCode)EditorGUILayout.EnumPopup ("Interact Key", interactKey);

		EditorHelper.DrawResizableList<TriggeredBehaviour> ("Behaviours on Entry",    ref behavioursOnEnter,    DrawEntry_BehaviourOnEnter);
		EditorHelper.DrawResizableList<TriggeredBehaviour> ("Behaviours on Interact", ref behavioursOnInteract, DrawEntry_BehaviourOnInteract);
		EditorHelper.DrawResizableList<TriggeredBehaviour> ("Behaviours on Exit",     ref behavioursOnExit,     DrawEntry_BehaviourOnExit);
	}

	private void DrawEntry_TriggeredByType (int index)
	{
		triggeredByTypes[index] = (GOType)EditorGUILayout.EnumPopup ("Type", triggeredByTypes[index]);
	}

	private void DrawEntry_BehaviourOnEnter (int index)
	{
		behavioursOnEnter[index] = EditorGUILayout.ObjectField
			("Behaviour", behavioursOnEnter[index], typeof (TriggeredBehaviour), true)
			as TriggeredBehaviour;
	}

	private void DrawEntry_BehaviourOnInteract (int index)
	{
		behavioursOnInteract[index] = EditorGUILayout.ObjectField
			("Behaviour", behavioursOnInteract[index], typeof (TriggeredBehaviour), true)
			as TriggeredBehaviour;
	}

	private void DrawEntry_BehaviourOnExit (int index)
	{
		behavioursOnExit[index] = EditorGUILayout.ObjectField
			("Behaviour", behavioursOnExit[index], typeof (TriggeredBehaviour), true)
			as TriggeredBehaviour;
	}

	#endif


	#region AteGameObject

	protected override void RegisterEvents ()
	{
		if (eventsToInteract != EventType_UI.None)
			GameManager.Events.Register<EventType_UI, EventData_UI> ((int)eventsToInteract, OnInteractEvent);

		GameManager.Events.Register<EventType_Collision, EventData_Collision>
			((int)EventType_Collision.AreaCollisionBegan, OnAteCollisionBegan);
		GameManager.Events.Register<EventType_Collision, EventData_Collision>
			((int)EventType_Collision.AreaCollisionEnded, OnAteCollisionEnded);
	}

	protected override void UnregisterEvents()
	{
		if (eventsToInteract != EventType_UI.None)
			GameManager.Events.Unregister<EventType_UI, EventData_UI> ((int)eventsToInteract, OnInteractEvent);

		GameManager.Events.Unregister<EventType_Collision, EventData_Collision>
			((int)EventType_Collision.AreaCollisionBegan, OnAteCollisionBegan);
		GameManager.Events.Unregister<EventType_Collision, EventData_Collision>
			((int)EventType_Collision.AreaCollisionEnded, OnAteCollisionEnded);
	}


	protected override void AteUpdate ()
	{
		
	}

	#endregion


	#region Trigger Entered

	private void AttemptEnterTriggers (AteObject triggerer)
	{
		if (!GameObjectIsAllowedType (triggerer))
			return;

		if (!_currentlyInTrigger.Contains (triggerer))
			_currentlyInTrigger.Add (triggerer);

		TriggerBehaviourList (behavioursOnEnter, triggerer);
	}


	public void ManualEnterArea (AteObject triggerer)
	{
		if (AreEnterBehavioursEmpty)
			return;
		
		AttemptEnterTriggers (triggerer);
	}

	void OnTriggerEnter (Collider theCollider)
	{
		if (AreEnterBehavioursEmpty)
			return;

		if (areaType != AreaType.UnityTriggers)
			return;

		AteObject triggerer = theCollider.gameObject.AteGameObject ();
		if (triggerer == null)
			return;

		AttemptEnterTriggers (triggerer);
	}

	private void OnAteCollisionBegan (EventData_Collision eventData)
	{
		if (AreEnterBehavioursEmpty)
			return;

		if (areaType != AreaType.CollisionArea)
			return;

		if (collisionArea == null)
			return;
		if (eventData.FullCollisionArea == null)
			return;
		if (collisionArea.InstanceID != eventData.FullCollisionArea.InstanceID)
			return;

		AteObject triggerer = eventData.HittingCollider.gameObject.AteGameObject ();
		if (triggerer == null)
			return;

		AttemptEnterTriggers (triggerer);
	}

	#endregion


	#region Trigger Interacted

	private void OnInteractEvent (EventData_UI theData)
	{
		if (AreInteractBehavioursEmpty)
			return;

		if (behavioursOnInteract.Count == 0)
			return;

		if (theData.theKey != interactKey)
			return;
		if (_currentlyInTrigger.Count < requiredToInteract)
			return;

		//TODO: Eventually, send whoever pressed the interaction button instead of null
		TriggerBehaviourList (behavioursOnInteract, null);
	}

	#endregion


	#region Trigger Exited

	private void AttemptExitTriggers (AteObject triggerer)
	{
		if (AreExitBehavioursEmpty)
			return;

		if (behavioursOnExit.Count == 0)
			return;

		if (!GameObjectIsAllowedType (triggerer))
			return;

		if (_currentlyInTrigger.Contains (triggerer))
			_currentlyInTrigger.Remove (triggerer);

		TriggerBehaviourList (behavioursOnExit, triggerer);
	}


	public void ManualExitArea (AteObject triggerer)
	{
		if (AreExitBehavioursEmpty)
			return;

		AttemptExitTriggers (triggerer);
	}

	void OnTriggerExit (Collider theCollider)
	{
		if (AreExitBehavioursEmpty)
			return;

		if (areaType != AreaType.UnityTriggers)
			return;

		AteObject triggerer = theCollider.gameObject.AteGameObject ();
		if (triggerer == null)
			return;

		AttemptExitTriggers (triggerer);
	}

	private void OnAteCollisionEnded (EventData_Collision eventData)
	{
		if (AreExitBehavioursEmpty)
			return;

		if (areaType != AreaType.CollisionArea)
			return;

		if (collisionArea == null)
			return;
		if (eventData.FullCollisionArea == null)
			return;
		if (collisionArea.InstanceID != eventData.FullCollisionArea.InstanceID)
			return;

		AteObject triggerer = eventData.HittingCollider.gameObject.AteGameObject ();
		if (triggerer == null)
			return;

		AttemptExitTriggers (triggerer);
	}

	#endregion


	private void TriggerBehaviourList (List<TriggeredBehaviour> behaviours, AteObject triggerer)
	{
		if (behaviours == null)
			return;
		if (behaviours.Count <= 0)
			return;

		for (int i = 0; i < behaviours.Count; i++)
		{
			if (behaviours[i] == null)
				continue;
			
			behaviours[i].RequestPlaying (triggerer);
		}
	}


	private bool GameObjectIsAllowedType (AteObject theGameObject)
	{
		//TODO: This is a bit hacky and specific, will be confusing for designers.
		if (triggeredByTypes == null)
			return true;
		if (triggeredByTypes.Count <= 0)
			return true;
		
		if (triggeredByTypes.Contains (theGameObject.type))
			return true;
		
		return false;
	}

}
