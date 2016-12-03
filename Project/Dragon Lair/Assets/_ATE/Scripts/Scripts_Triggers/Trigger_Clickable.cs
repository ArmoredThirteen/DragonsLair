using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif


public class Trigger_Clickable : AteGameObject
{
	public EventType_UI eventsToInteract = EventType_UI.None;
	public KeyCode interactKey = KeyCode.Mouse0;

	public List<TriggeredBehaviour> behavioursOnInteract = new List<TriggeredBehaviour> ();


	private int _curMouseOvers = 0;


	#if UNITY_EDITOR

	public override void DrawInspector ()
	{
		base.DrawInspector ();

		eventsToInteract = (EventType_UI)EditorGUILayout.EnumPopup ("Event to Interact", eventsToInteract);
		interactKey = (KeyCode)EditorGUILayout.EnumPopup ("Interact Key", interactKey);

		EditorHelper.DrawResizableList<TriggeredBehaviour> ("Behaviours on Interact", ref behavioursOnInteract, DrawEntry_BehaviourOnInteract);
	}

	private void DrawEntry_BehaviourOnInteract (int index)
	{
		behavioursOnInteract[index] = EditorGUILayout.ObjectField
			("Behaviour", behavioursOnInteract[index], typeof (TriggeredBehaviour), true)
			as TriggeredBehaviour;
	}

	#endif


	protected override void AteUpdate ()
	{

	}


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


	void OnMouseEnter ()
	{
		_curMouseOvers++;
	}

	void OnMouseExit ()
	{
		_curMouseOvers--;
	}


	private void OnInteractEvent (EventData_UI theData)
	{
		if (theData.theKey != interactKey)
			return;
		if (_curMouseOvers <= 0)
			return;

		//TODO: Eventually, send whoever pressed the interaction button instead of null
		TriggerBehaviourList (behavioursOnInteract, null);
	}


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

}
