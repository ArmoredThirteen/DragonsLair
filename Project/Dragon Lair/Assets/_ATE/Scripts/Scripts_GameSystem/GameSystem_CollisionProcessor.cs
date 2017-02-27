using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CollisionSystem;


public class GameSystem_CollisionProcessor : GameSystem
{

	public CollisionProcessor processor;


	#region GameSystem

	public override void Initialize ()
	{
		GameManager.Events.Register<EventType_Collision, EventData_Collision> ((int)EventType_Collision.RegisterCollider,   OnRegisterCollider);
		GameManager.Events.Register<EventType_Collision, EventData_Collision> ((int)EventType_Collision.UnregisterCollider, OnUnregisterCollider);
	}

	public override void SceneInitialize ()
	{
		//GameManager.Events.Register<EventType_Collision, EventData_Collision> ((int)EventType_Collision.RegisterCollider,   OnRegisterCollider);
		//GameManager.Events.Register<EventType_Collision, EventData_Collision> ((int)EventType_Collision.UnregisterCollider, OnUnregisterCollider);
	}

	public override void SystemUpdate ()
	{
		List<CollisionDetails> details = processor.FindCollisionDetails ();

		for (int i = 0; i < details.Count; i++)
		{
			DebugLog.Simple ("Col One: ", details[i].colOne.name, "Col Two: ", details[i].colTwo.name);

			EventData_Collision theData = new EventData_Collision ();
			theData.ColliderOne = details[i].colOne;
			theData.ColliderTwo = details[i].colTwo;

			GameManager.Events.Broadcast<EventType_Collision> ((int)EventType_Collision.Collision, theData);
		}
	}

	public override void SystemLateUpdate (){}


	void OnDestroy ()
	{
		GameManager.Events.Unregister<EventType_Collision, EventData_Collision> ((int)EventType_Collision.RegisterCollider,   OnRegisterCollider);
		GameManager.Events.Unregister<EventType_Collision, EventData_Collision> ((int)EventType_Collision.UnregisterCollider, OnUnregisterCollider);
	}

	#endregion


	#region Events

	private void OnRegisterCollider (EventData_Collision theData)
	{
		if (processor == null)
		{
			#if UNITY_EDITOR
			Debug.LogError ("GameSystem_CollisionProcessor attempted collider registration but the CollisionProcessor is null.");
			#endif
			return;
		}
		Debug.Log (theData.ColliderOne.name);
		processor.RegisterCollider (theData.ColliderOne);
	}

	private void OnUnregisterCollider (EventData_Collision theData)
	{
		if (processor == null)
		{
			#if UNITY_EDITOR
			Debug.LogError ("GameSystem_CollisionProcessor attempted collider registration but the CollisionProcessor is null.");
			#endif
			return;
		}

		processor.UnregisterCollider (theData.ColliderOne);
	}

	#endregion

}

