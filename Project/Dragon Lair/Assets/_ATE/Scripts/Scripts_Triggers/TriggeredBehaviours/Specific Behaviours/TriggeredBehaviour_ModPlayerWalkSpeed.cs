using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif


namespace Ate
{


	public class TriggeredBehaviour_ModPlayerWalkSpeed : TriggeredBehaviour
	{
		public enum ModType
		{
			Reset = 0,
			Set   = 10,
		}

		//	Variables for designers.
		//	Shown in editor with DrawInspector() at bottom.
		#region Public Variables

		public ModType modType = ModType.Reset;

		public float setWalkSpeed = 0;

		#endregion


		#if UNITY_EDITOR

		/// <summary>
		/// Called by parent class for drawing specific variables at top.
		/// Parent class should automatically check for when it is dirty.
		/// </summary>
		protected override void DrawChildInspector ()
		{
			modType = (ModType)EditorGUILayout.EnumPopup ("Mod Type", modType);

			if (modType == ModType.Set)
				setWalkSpeed = EditorGUILayout.FloatField ("Set Walk Speed", setWalkSpeed);
		}

		#endif


		#region Awake/Start

		/// <summary>
		/// Called by parent class at the end of its AteAwake().
		/// </summary>
		protected override void OnAwake ()
		{
			
		}

		/// <summary>
		/// Called by AteObject at end of its Awake().
		/// </summary>
		protected override void AteStart ()
		{
			
		}

		#endregion


		#region OnRequested

		/// <summary>
		/// For resetting to a more 'factory default' version.
		/// For things like only playing a sequencer once, then
		/// resetting it from a different behaviour somewhere
		/// else so it can be played again.
		/// </summary>
		protected override void OnDataReset()
		{
			
		}


		/// <summary>
		/// Called when parent class had a request to play.
		/// If inactive and cancelRequestsWhileInactive is true, won't be called.
		/// </summary>
		protected override void OnRequestedPlaying (AteObject triggerer)
		{
			
		}

		/// <summary>
		/// Called when parent class had a request to complete.
		/// If inactive and cancelRequestsWhileInactive is true, won't be called.
		/// </summary>
		protected override void OnRequestedComplete ()
		{
			
		}

		/// <summary>
		/// Called when parent class had a request to reset.
		/// If inactive and cancelRequestsWhileInactive is true, won't be called.
		/// </summary>
		protected override void OnRequestedPlayReset ()
		{
			
		}

		#endregion


		#region OnEntered

		/// <summary>
		/// Called when behaviour enters the Ready state.
		/// Currently it starts in Ready, but the enter callback
		/// only happens when it switches to Ready.
		/// So for now it can be thought more as 'OnReset'.
		/// </summary>
		protected override void OnEnteredReady (TriggeredState prevState)
		{
			
		}

		/// <summary>
		/// Called when behaviour enters the Playing state.
		/// For instant-fire behaviours, this is where 99% of the logic will go.
		/// </summary>
		protected override void OnEnteredPlaying (TriggeredState prevState)
		{
			EventData_Gameplay theData = new EventData_Gameplay ();
			theData.WalkSpeed = setWalkSpeed;

			switch (modType)
			{
				case ModType.Reset :
					OnEnteredPlaying_Reset (theData);
					break;
				case ModType.Set :
					OnEnteredPlaying_Set (theData);
					break;
			}

			//	Called at end of this method for an instant-fire behaviour
			RequestComplete ();
		}

		/// <summary>
		/// Called when behaviour enters the Complete state.
		/// Happens after a RequestComplete() call and CanSwitchToComplete is true.
		/// </summary>
		protected override void OnEnteredComplete (TriggeredState prevState)
		{
			
		}


		private void OnEnteredPlaying_Reset (EventData_Gameplay theData)
		{
			GameManager.Events.Broadcast<EventType_Gameplay> ((int)EventType_Gameplay.ResetPlayerWalkSpeed, theData);
		}

		private void OnEnteredPlaying_Set (EventData_Gameplay theData)
		{
			GameManager.Events.Broadcast<EventType_Gameplay> ((int)EventType_Gameplay.SetPlayerWalkSpeed, theData);
		}

		#endregion


		#region OnUpdate

		/// <summary>
		/// Called every frame behaviour is in the Ready state.
		/// </summary>
		protected override void OnUpdateReady ()
		{
			
		}

		/// <summary>
		/// Called every frame behaviour is in the Playing state.
		/// For over-time behaviours, this is where most of the logic will go.
		/// </summary>
		protected override void OnUpdatePlaying ()
		{
			//	Called when an end-condition happens (such as a timer)
			//RequestComplete ();
		}

		/// <summary>
		/// Called every frame behaviour is in the Complete state.
		/// This will happen after Playing until it is Reset or canceled.
		/// </summary>
		protected override void OnUpdateComplete ()
		{
			
		}

		#endregion


		#region CanSwitch

		/// <summary>
		/// After parent class determines if a switch was requested,
		/// it uses this as an extra check if it can switch yet.
		/// </summary>
		protected override bool CanSwitchToPlaying ()
		{
			return true;
		}

		/// <summary>
		/// After parent class determines if a switch was requested,
		/// it uses this as an extra check if it can switch yet.
		/// </summary>
		protected override bool CanSwitchToComplete ()
		{
			return true;
		}

		/// <summary>
		/// After parent class determines if a switch was requested,
		/// it uses this as an extra check if it can switch yet.
		/// </summary>
		protected override bool CanPlayReset ()
		{
			return true;
		}

		#endregion


		#region Helper Methods

		#endregion

	}//End Class


}//End Namespace
