#define DebugStates


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Ate.Spawning;
using Ate.EditorHelpers;
using Ate.FSM;
using Ate.GameSystems;
using Ate.Pooling;

#if UNITY_EDITOR
using UnityEditor;
#endif


namespace Ate.Killing
{
	
	
	/// <summary>
	/// Description
	/// </summary>
	public class KillableObject : PoolableObject
	{
		public enum DyingState
		{
			None = 0,

			Alive,
			Dying,
			Killed,
			Reviving,
		}

		public enum DyingTrigger
		{
			/// <summary> Starts dying immediately. </summary>
			None = 0,

			/// <summary> Starts dying when another script on this object requests it. </summary>
			//Self = 10,

			/// <summary> Starts dying after a specified GameManager.Events broadcast. </summary>
			//Event_Ate = 100,

			/// <summary> Starts dying after an ATE-specific trigger is fired. </summary>
			Trigger_Ate   = 200,
			/// <summary> Starts dying after a Unity-specific trigger is fired. </summary>
			//Trigger_Unity = 210,

			/// <summary> Starts dying after an ATE-specific collision occurs. </summary>
			//Collision_Ate = 300,
			/// <summary> Starts dying after a Unity-specific collision occurs. </summary>
			//Collision_Unity   = 310,
		}

		public enum OnKilledAction
		{
			Destroy = 0,

			Disable = 100,

			Pool    = 200,
		}


		#region Public Variables

		public bool beginAlive = true;

		public DyingTrigger   dyingTrigger   = DyingTrigger.None;
		public OnKilledAction onKilledAction = OnKilledAction.Destroy;

		/// <summary> How many frames before going from Dying to Killed. </summary>
		public int duration_dying    = 0;
		/// <summary> How many frames before going from Reviving to Alive. </summary>
		public int duration_reviving = 0;

		public DyingState stateDataToDraw = DyingState.None;

		public KillableStateData stateData_alive    = new KillableStateData ();
		public KillableStateData stateData_dying    = new KillableStateData ();
		public KillableStateData stateData_killed   = new KillableStateData ();
		public KillableStateData stateData_reviving = new KillableStateData ();

		#endregion


		#region Private Variables

		private BasicFSM<DyingState> _fsm_dyingState;

		private int _timer_dying    = 0;
		private int _timer_reviving = 0;

		private bool _requestedKill   = false;
		private bool _requestedRevive = false;

		private bool _requestedKillImmediate   = false;
		private bool _requestedReviveImmediate = false;

		private bool _completedKill   = false;
		private bool _completedRevive = false;

		#endregion


		#region Properties

		public bool IsAlive
		{
			get { return _fsm_dyingState.IsCurrentState (DyingState.Alive); }
		}

		public bool IsDying
		{
			get { return _fsm_dyingState.IsCurrentState (DyingState.Dying); }
		}

		public bool IsKilled
		{
			get { return _fsm_dyingState.IsCurrentState (DyingState.Killed); }
		}

		public bool IsReviving
		{
			get { return _fsm_dyingState.IsCurrentState (DyingState.Reviving); }
		}

		#endregion


		#if UNITY_EDITOR

		public override void DrawInspector ()
		{
			base.DrawInspector ();

			if (Application.isPlaying && _fsm_dyingState != null)
			{
				EditorGUILayout.LabelField ("Current State: " + _fsm_dyingState.GetCurrentState ().ToString ());
			}

			beginAlive = EditorGUILayout.Toggle ("Begin Alive", beginAlive);

			dyingTrigger   = (DyingTrigger)EditorGUILayout.EnumPopup   ("Dying Trigger", dyingTrigger);
			onKilledAction = (OnKilledAction)EditorGUILayout.EnumPopup ("Killed Action", onKilledAction);

			/*if (onKilledAction == OnKilledAction.Pool)
			{
				
			}*/

			duration_dying    = EditorGUILayout.IntField ("Dying Duration",    duration_dying);
			duration_reviving = EditorGUILayout.IntField ("Reviving Duration", duration_reviving);

			EditorGUILayout.Space ();

			stateDataToDraw = (DyingState)EditorGUILayout.EnumPopup ("Draw Data", stateDataToDraw);
			OnDrawStateDatas ();
		}

		private void OnDrawStateDatas ()
		{
			if (stateDataToDraw == DyingState.None)
				return;

			switch (stateDataToDraw)
			{
				case DyingState.Alive :
					stateData_alive.OnDrawInspector ();
					break;

				case DyingState.Dying :
					stateData_dying.OnDrawInspector ();
					break;

				case DyingState.Killed :
					stateData_killed.OnDrawInspector ();
					break;

				case DyingState.Reviving :
					stateData_reviving.OnDrawInspector ();
					break;
			}
		}

		#endif


		#region AteComponent

		protected override void RegisterEvents ()
		{
			base.RegisterEvents ();
			OnRegisterEvents ();
		}

		protected override void UnregisterEvents ()
		{
			base.UnregisterEvents ();
			OnUnregisterEvents ();
		}


		protected override void AteAwake ()
		{
			base.AteAwake ();

			BuildDyingStateFSM ();

			if (!beginAlive)
			{
				RequestKillImmediate ();
			}
		}

		protected override void AteStart ()
		{
			base.AteStart ();

			/*if (!beginAlive)
			{
				RequestKillImmediate ();
			}*/

			switch (dyingTrigger)
			{
				case DyingTrigger.None:
					RequestKill ();
					break;

				case DyingTrigger.Trigger_Ate:
					break;
			}
		}


		// Updates every game frame
		protected override void AteUpdate ()
		{
			
		}


		// Updates 24 times per second
		protected override void FpsUpdate24 ()
		{
			
		}

		// Updates once per framelength, which is one or more FpsUpdate## calls
		protected override void UpdateFrameLength ()
		{
			UpdateFSM ();
		}

		#endregion


		#region Public Methods

		#if UNITY_EDITOR

		[MenuItem("CONTEXT/KillableObject/Kill")]
		private static void Menu_Kill (MenuCommand command)
		{
			if (!Application.isPlaying)
				return;
			
			KillableObject component = command.context as KillableObject;
			if (component == null)
				return;

			#if DebugStates
			Debug.Log ("<color=orange>Menu requested KILL</color>");
			#endif

			component.RequestKill ();
		}

		[MenuItem("CONTEXT/KillableObject/Revive")]
		private static void Menu_Revive (MenuCommand command)
		{
			if (!Application.isPlaying)
				return;

			KillableObject component = command.context as KillableObject;
			if (component == null)
				return;

			#if DebugStates
			Debug.Log ("<color=orange>Menu requested REVIVE</color>");
			#endif

			component.RequestRevive ();
		}

		[MenuItem("CONTEXT/KillableObject/Kill Immediate")]
		private static void Menu_KillImmediate (MenuCommand command)
		{
			if (!Application.isPlaying)
				return;

			KillableObject component = command.context as KillableObject;
			if (component == null)
				return;

			#if DebugStates
			Debug.Log ("<color=orange>Menu requested KILL IMMEDIATE</color>");
			#endif

			component.RequestKillImmediate ();
		}

		[MenuItem("CONTEXT/KillableObject/Revive Immediate")]
		private static void Menu_ReviveImmediate (MenuCommand command)
		{
			if (!Application.isPlaying)
				return;

			KillableObject component = command.context as KillableObject;
			if (component == null)
				return;

			#if DebugStates
			Debug.Log ("<color=orange>Menu requested REVIVE IMMEDIATE</color>");
			#endif

			component.RequestReviveImmediate ();
		}

		#endif


		/// <summary>
		/// If the object is Alive is goes into the Dying state.
		/// </summary>
		public void RequestKill ()
		{
			if (!IsAlive)
				return;

			#if DebugStates
			Debug.Log ("<color=green>KILL successfully requested</color>");
			#endif

			_requestedKill = true;
		}

		/// <summary>
		/// Resets the object to be killed once again.
		/// Mainly sets the state to Alive.
		/// </summary>
		public void RequestRevive ()
		{
			if (!IsKilled)
				return;

			#if DebugStates
			Debug.Log ("<color=green>REVIVE successfully requested</color>");
			#endif

			_requestedRevive = true;
		}


		/// <summary>
		/// From any state, attempts to go to Killed this frame.
		/// Skips spawners but not object enabling/disabling.
		/// Sets the requests then calls an FSM update in
		/// order to fully resolve in this frame.
		/// </summary>
		public void RequestKillImmediate ()
		{
			if (IsKilled)
				return;

			#if DebugStates
			Debug.Log ("<color=green>KILL IMMEDIATE successfully requested</color>");
			#endif

			_requestedKillImmediate = true;
			//UpdateFSM ();
		}

		/// <summary>
		/// From any state, attempts to go to Alive this frame.
		/// Skips spawners but not object enabling/disabling.
		/// Sets the requests then calls an FSM update in
		/// order to fully resolve in this frame.
		/// </summary>
		public void RequestReviveImmediate ()
		{
			if (IsAlive)
				return;

			#if DebugStates
			Debug.Log ("<color=green>REVIVE IMMMEDIATE successfully requested</color>");
			#endif

			_requestedReviveImmediate = true;
			//UpdateFSM ();
		}

		#endregion


		#region Private Methods

		/// <summary>
		/// Spawns FX and enables/disables objects.
		/// </summary>
		private void EnterStateEffects (KillableStateData stateData, bool isImmediate)
		{
			if (!isImmediate)
			{
				for (int i = 0; i < stateData.FXspawners.Count; i++)
				{
					stateData.FXspawners[i].Spawn ();
				}
			}

			for (int i = 0; i < stateData.objectsToEnable.Count; i++)
			{
				stateData.objectsToEnable[i].gameObject.SetActive (true);
			}

			for (int i = 0; i < stateData.objectsToDisable.Count; i++)
			{
				stateData.objectsToDisable[i].gameObject.SetActive (false);
			}
		}

		#endregion


		#region Event Registering

		private void OnRegisterEvents ()
		{
			switch (dyingTrigger)
			{
				case DyingTrigger.None:
					break;

				case DyingTrigger.Trigger_Ate:
					GameManager.Events.Register<EventType_Gameplay, EventData_Gameplay>
						((int)EventType_Gameplay.KillObject, OnKillObject);
					break;
			}
		}

		private void OnUnregisterEvents ()
		{
			switch (dyingTrigger)
			{
				case DyingTrigger.None:
					break;

				case DyingTrigger.Trigger_Ate:
					GameManager.Events.Unregister<EventType_Gameplay, EventData_Gameplay>
						((int)EventType_Gameplay.KillObject, OnKillObject);
					break;
			}
		}


		private void OnKillObject (EventData_Gameplay theData)
		{
			if (this.GOInstanceID != theData.AteGOInstanceID)
				return;

			if (theData.IsImmediate)
				RequestKillImmediate ();
			else
				RequestKill ();
		}

		#endregion


		#region Dying FSM

		/// <summary>
		/// Uses MultipleUpdate() to update _fsm_dyingState multiple times.
		/// Will keep updating then switching states until it can't switch.
		/// </summary>
		private void UpdateFSM (bool updateThenSwitch = true)
		{
			_fsm_dyingState.MultipleUpdate (DyingState.None, updateThenSwitch);
		}


		private void BuildDyingStateFSM ()
		{
			_fsm_dyingState = new BasicFSM<DyingState> (DyingState.Alive);

			_fsm_dyingState.SetMainCallbacks (DyingState.Alive,    FSM_Update_Alive,    FSM_Enter_Alive,    null);
			_fsm_dyingState.SetMainCallbacks (DyingState.Dying,    FSM_Update_Dying,    FSM_Enter_Dying,    null);
			_fsm_dyingState.SetMainCallbacks (DyingState.Killed,   FSM_Update_Killed,   FSM_Enter_Killed,   null);
			_fsm_dyingState.SetMainCallbacks (DyingState.Reviving, FSM_Update_Reviving, FSM_Enter_Reviving, null);

			_fsm_dyingState.AddPossibleSwitch (DyingState.Alive,    DyingState.Dying,    FSM_Switch_AliveToDying);
			_fsm_dyingState.AddPossibleSwitch (DyingState.Dying,    DyingState.Killed,   FSM_Switch_DyingToKilled);
			_fsm_dyingState.AddPossibleSwitch (DyingState.Killed,   DyingState.Reviving, FSM_Switch_KilledToReviving);
			_fsm_dyingState.AddPossibleSwitch (DyingState.Reviving, DyingState.Alive,    FSM_Switch_RevivingToAlive);
		}

		#endregion


		#region DyingState Alive

		private void FSM_Enter_Alive (DyingState previousState)
		{
			#if DebugStates
			Debug.Log ("<color=blue>Entered ALIVE</color>");
			#endif
			EnterStateEffects (stateData_alive, _requestedReviveImmediate);

			//_requestedRevive          = false;
			//_requestedReviveImmediate = false;
		}

		private void FSM_Update_Alive ()
		{

		}

		private bool FSM_Switch_AliveToDying ()
		{
			return _requestedKill || _requestedKillImmediate;
		}

		#endregion

		#region DyingState Dying

		private void FSM_Enter_Dying (DyingState previousState)
		{
			#if DebugStates
			Debug.Log ("<color=blue>Entered DYING</color>");
			#endif
			EnterStateEffects (stateData_dying, _requestedKillImmediate);

			_requestedRevive          = false;
			_requestedReviveImmediate = false;

			_timer_dying = 0;
		}

		private void FSM_Update_Dying ()
		{
			_timer_dying++;
		}

		private bool FSM_Switch_DyingToKilled ()
		{
			return (_timer_dying > duration_dying) || _requestedKillImmediate;
		}

		#endregion

		#region DyingState Killed

		private void FSM_Enter_Killed (DyingState previousState)
		{
			#if DebugStates
			Debug.Log ("<color=blue>Entered KILLED</color>");
			#endif
			EnterStateEffects (stateData_killed, _requestedKillImmediate);

			//_requestedKill          = false;
			//_requestedKillImmediate = false;

			switch (onKilledAction)
			{
				case OnKilledAction.Destroy :
					Destroy (gameObject);
					break;

				case OnKilledAction.Disable :
					gameObject.SetActive (false);
					break;

				case OnKilledAction.Pool :
					GameManager.Pooling.PoolObject (this);
					break;
			}
		}

		private void FSM_Update_Killed ()
		{
			
		}

		private bool FSM_Switch_KilledToReviving ()
		{
			return _requestedRevive || _requestedReviveImmediate;
		}

		#endregion

		#region DyingState Reviving

		private void FSM_Enter_Reviving (DyingState previousState)
		{
			#if DebugStates
			Debug.Log ("<color=blue>Entered REVIVING</color>");
			#endif
			EnterStateEffects (stateData_reviving, _requestedReviveImmediate);

			_requestedKill          = false;
			_requestedKillImmediate = false;

			_timer_reviving = 0;

			switch (onKilledAction)
			{
				case OnKilledAction.Destroy :
					break;

				case OnKilledAction.Disable :
					gameObject.SetActive (true);
					break;

				case OnKilledAction.Pool :
					break;
			}
		}

		private void FSM_Update_Reviving ()
		{
			_timer_reviving++;
		}

		private bool FSM_Switch_RevivingToAlive ()
		{
			return (_timer_reviving > duration_reviving) || _requestedReviveImmediate;
		}

		#endregion


	}//End Class
	
	
}//End Namespace
