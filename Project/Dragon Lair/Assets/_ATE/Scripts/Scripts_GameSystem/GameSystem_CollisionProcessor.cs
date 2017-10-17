//#define DebugBegan
//#define DebugContinued
//#define DebugEnded

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Ate.Collision;


namespace Ate
{


	public class GameSystem_CollisionProcessor : GameSystem
	{

		public CollisionProcessor processor;

		private List<CollisionDetails> _lastDetails = new List<CollisionDetails> ();


		#region GameSystem

		public override void Initialize ()
		{
			GameManager.Events.Register<EventType_Collision, EventData_Collision> ((int)EventType_Collision.RegisterCollider,   OnRegisterCollider);
			GameManager.Events.Register<EventType_Collision, EventData_Collision> ((int)EventType_Collision.UnregisterCollider, OnUnregisterCollider);
		}

		public override void SceneInitialize ()
		{
			_lastDetails.Clear ();
		}

		public override void SystemUpdate ()
		{
			List<CollisionDetails> details = processor.FindCollisionDetails ();

			for (int i = 0; i < details.Count; i++)
			{
				EventData_Collision theData = new EventData_Collision ();
				theData.ColliderOne = details[i].colOne;
				theData.ColliderTwo = details[i].colTwo;

				if (_lastDetails.Contains (details[i]))
				{
					#if DebugContinued
					DebugLog.Simple ("Collision Continued\r\nCol One: ", details[i].colOne.name, "Col Two: ", details[i].colTwo.name);
					#endif
					GameManager.Events.Broadcast<EventType_Collision> ((int)EventType_Collision.CollisionContinued, theData);

					//	Remove from _lastDetails so next area can go over the remaining ones and declare them ended
					_lastDetails.Remove (details[i]);
				}
				else
				{
					#if DebugBegan
					DebugLog.Simple ("<color=green>Collision Began</color>\r\nCol One: ", details[i].colOne.name, "Col Two: ", details[i].colTwo.name);
					#endif
					GameManager.Events.Broadcast<EventType_Collision> ((int)EventType_Collision.CollisionBegan, theData);
				}
			}

			//	In previous area any matching collisions were removed from _lastDetails.
			//	So the remaining members were in collisions last frame but not this one, and have ended.
			for (int i = 0; i < _lastDetails.Count; i++)
			{
				EventData_Collision theData = new EventData_Collision ();
				theData.ColliderOne = _lastDetails[i].colOne;
				theData.ColliderTwo = _lastDetails[i].colTwo;

				#if DebugEnded
				DebugLog.Simple ("<color=red>Collision Ended</color>\r\nCol One: ", _lastDetails[i].colOne.name, "Col Two: ", _lastDetails[i].colTwo.name);
				#endif
				GameManager.Events.Broadcast<EventType_Collision> ((int)EventType_Collision.CollisionEnded, theData);
			}

			_lastDetails = details;
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

	}//End Class


}//End Namespace
