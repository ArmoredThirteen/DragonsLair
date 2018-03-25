using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Ate.Spawning;
using Ate.EditorHelpers;
using Ate.FSM;

#if UNITY_EDITOR
using UnityEditor;
#endif


namespace Ate.Killing
{
	
	
	/// <summary>
	/// Description
	/// </summary>
	public class KillableObject : AteComponent_fpsControlled
	{
		public enum DyingState
		{
			Alive,
			Dying,
			Killed,
			Reviving,
		}

		public enum DyingTrigger
		{
			/// <summary> Starts dying immediately. </summary>
			None = 0,

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

			//Disable = 100,

			Pool    = 200,
		}


		#region Public Variables

		public DyingTrigger   dyingTrigger   = DyingTrigger.None;
		public OnKilledAction onKilledAction = OnKilledAction.Destroy;

		/// <summary> How many frames before going from dying to killed. </summary>
		public int dyingDuration = 0;

		public List<GameObject> objectsDisableImmediate = new List<GameObject> ();

		public List<Spawner> dyingFXSpawners  = new List<Spawner> ();
		public List<Spawner> killedFXSpawners = new List<Spawner> ();


		public bool drawList_objectsDisableImmediate = false;
		public bool drawList_dyingFXSpawners  = false;
		public bool drawList_killedFXSpawners = false;

		#endregion


		#region Private Variables

		private BasicFSM<DyingState> _fsm_dyingState;

		private bool _requestedKill   = false;
		private bool _requestedRevive = false;

		private bool _completedKill   = false;
		private bool _completedRevive = false;

		#endregion


		#region Properties

		public bool IsAlive
		{
			get { return _fsm_dyingState.IsCurrentState (DyingState.Alive); }
		}

		public bool IsKilled
		{
			get { return _fsm_dyingState.IsCurrentState (DyingState.Killed); }
		}

		#endregion


		#if UNITY_EDITOR

		public override void DrawInspector ()
		{
			base.DrawInspector ();

			dyingTrigger   = (DyingTrigger)EditorGUILayout.EnumPopup   ("Dying Trigger", dyingTrigger);
			onKilledAction = (OnKilledAction)EditorGUILayout.EnumPopup ("Killed Action", onKilledAction);

			dyingDuration = EditorGUILayout.IntField ("Dying Duration", dyingDuration);

			EditorGUILayout.Space ();

			EditorHelper.DrawResizableList<GameObject>
				("Objects to Disable Immediately", ref drawList_objectsDisableImmediate, ref objectsDisableImmediate, DrawEntry_ObjectsDisableImmediate);

			EditorGUILayout.Space ();

			EditorHelper.DrawResizableList<Spawner>
				("Dying FX Spawners", ref drawList_dyingFXSpawners, ref dyingFXSpawners, DrawEntry_DyingFXSpawners);

			EditorGUILayout.Space ();

			EditorHelper.DrawResizableList<Spawner>
				("Killed FX Spawners", ref drawList_killedFXSpawners, ref killedFXSpawners, DrawEntry_KilledFXSpawners);
		}

		private void DrawEntry_ObjectsDisableImmediate (int index)
		{
			objectsDisableImmediate[index] = EditorGUILayout.ObjectField
				("Object #"+index, objectsDisableImmediate[index], typeof (GameObject), true)
				as GameObject;
		}

		private void DrawEntry_DyingFXSpawners (int index)
		{
			dyingFXSpawners[index] = EditorGUILayout.ObjectField
				("Spawner #"+index, dyingFXSpawners[index], typeof (GameObject), true)
				as Spawner;
		}

		private void DrawEntry_KilledFXSpawners (int index)
		{
			killedFXSpawners[index] = EditorGUILayout.ObjectField
				("Spawner #"+index, killedFXSpawners[index], typeof (GameObject), true)
				as Spawner;
		}

		#endif


		#region AteComponent

		protected override void AteAwake ()
		{
			base.AteAwake ();

			BuildDyingStateFSM ();
		}

		protected override void AteStart ()
		{
			base.AteStart ();

			switch (dyingTrigger)
			{
				case DyingTrigger.None:
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

		/// <summary>
		/// Resets the object to be killed once again.
		/// Mainly sets the state to Alive.
		/// </summary>
		public void RequestRevive ()
		{
			if (!IsKilled)
				return;

			_requestedRevive = true;
		}

		/// <summary>
		/// If the object is Alive is goes into the Dying state.
		/// </summary>
		public void RequestKill ()
		{
			if (!IsAlive)
				return;
			
			_requestedKill = true;
		}

		#endregion


		#region Dying FSM

		/// <summary>
		/// Updates _fsm_dyingState once then attempts to switch states
		/// </summary>
		private void UpdateFSM (bool updateThenSwitch = true)
		{
			_fsm_dyingState.Update (updateThenSwitch);
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

		}

		private void FSM_Update_Alive ()
		{

		}

		private bool FSM_Switch_AliveToDying ()
		{
			return _requestedKill;
		}

		#endregion

		#region DyingState Dying

		private void FSM_Enter_Dying (DyingState previousState)
		{
			
		}

		private void FSM_Update_Dying ()
		{
			//TODO: Logic that controls if _completedKill actually happens this frame
			_completedKill = true;
		}

		private bool FSM_Switch_DyingToKilled ()
		{
			return _completedKill;
		}

		#endregion

		#region DyingState Killed

		private void FSM_Enter_Killed (DyingState previousState)
		{

		}

		private void FSM_Update_Killed ()
		{
			
		}

		private bool FSM_Switch_KilledToReviving ()
		{
			return _requestedRevive;
		}

		#endregion

		#region DyingState Reviving

		private void FSM_Enter_Reviving (DyingState previousState)
		{
			
		}

		private void FSM_Update_Reviving ()
		{
			//TODO: Logic that controls if _completedRevive actually happens this frame
			_completedRevive = true;
		}

		private bool FSM_Switch_RevivingToAlive ()
		{
			return _completedRevive;
		}

		#endregion


	}//End Class
	
	
}//End Namespace
