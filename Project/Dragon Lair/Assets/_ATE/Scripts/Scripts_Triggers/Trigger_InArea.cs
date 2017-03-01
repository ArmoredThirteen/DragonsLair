﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif


public class Trigger_InArea : AteGameObject
{
	public List<GOType> triggeredByTypes = new List<GOType> ();

	/// <summary>
	/// How many objects of type GOType must
	/// be in trigger to be interacted with.
	/// </summary>
	public int requiredToInteract = 1;
	public EventType_UI eventsToInteract = EventType_UI.None;
	public KeyCode interactKey = KeyCode.Mouse0;

	public List<TriggeredBehaviour> behavioursOnEnter    = new List<TriggeredBehaviour> ();
	public List<TriggeredBehaviour> behavioursOnInteract = new List<TriggeredBehaviour> ();
	public List<TriggeredBehaviour> behavioursOnExit     = new List<TriggeredBehaviour> ();

	private List<AteGameObject> _currentlyInTrigger = new List<AteGameObject> ();


	#if UNITY_EDITOR

	public override void DrawInspector ()
	{
		base.DrawInspector ();

		EditorHelper.DrawResizableList<GOType> ("Triggered by Types", ref triggeredByTypes, DrawEntry_TriggeredByType);

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


	protected override void RegisterEvents ()
	{
		if (eventsToInteract == EventType_UI.None)
			return;
		
		GameManager.Events.Register<EventType_UI, EventData_UI> ((int)eventsToInteract, OnInteractEvent);
	}

	protected override void UnregisterEvents()
	{
		if (eventsToInteract == EventType_UI.None)
			return;
		
		GameManager.Events.Unregister<EventType_UI, EventData_UI> ((int)eventsToInteract, OnInteractEvent);
	}


	protected override void AteUpdate ()
	{
		
	}


	#region Trigger Entered

	private void AttemptEnterTriggers (AteGameObject triggerer)
	{
		if (!GameObjectIsAllowedType (triggerer))
			return;

		if (!_currentlyInTrigger.Contains (triggerer))
			_currentlyInTrigger.Add (triggerer);

		TriggerBehaviourList (behavioursOnEnter, triggerer);
	}

	public void ManualEnterArea (AteGameObject triggerer)
	{
		AttemptEnterTriggers (triggerer);
	}

	void OnTriggerEnter (Collider theCollider)
	{
		AteGameObject triggerer = theCollider.gameObject.AteGameObject ();

		if (triggerer == null)
			return;

		AttemptEnterTriggers (triggerer);
	}

	private void OnAteCollisionBegan (EventData_Collision eventData)
	{
		
	}

	#endregion


	#region Trigger Interacted

	private void OnInteractEvent (EventData_UI theData)
	{
		if (theData.theKey != interactKey)
			return;
		if (_currentlyInTrigger.Count < requiredToInteract)
			return;

		//TODO: Eventually, send whoever pressed the interaction button instead of null
		TriggerBehaviourList (behavioursOnInteract, null);
	}

	#endregion


	#region Trigger Exited

	private void AttemptExitTriggers (AteGameObject triggerer)
	{
		if (!GameObjectIsAllowedType (triggerer))
			return;

		if (_currentlyInTrigger.Contains (triggerer))
			_currentlyInTrigger.Remove (triggerer);

		TriggerBehaviourList (behavioursOnExit, triggerer);
	}

	public void ManualExitArea (AteGameObject triggerer)
	{
		AttemptExitTriggers (triggerer);
	}

	void OnTriggerExit (Collider theCollider)
	{
		AteGameObject triggerer = theCollider.gameObject.AteGameObject ();
		if (triggerer == null)
			return;

		AttemptExitTriggers (triggerer);
	}

	private void AteOnCollisionEnded (EventData_Collision eventData)
	{
		
	}

	#endregion


	private void TriggerBehaviourList (List<TriggeredBehaviour> behaviours, AteGameObject triggerer)
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


	private bool GameObjectIsAllowedType (AteGameObject theGameObject)
	{
		if (triggeredByTypes.Contains (theGameObject.type))
			return true;
		
		return false;
	}

}
