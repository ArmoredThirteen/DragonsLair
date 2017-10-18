using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Ate.FSM;


namespace Ate.GameSystems
{


	/// <summary>
	/// Manages all the GameSystems. Namely, calling their SystemUpdate methods.
	/// </summary>
	public class GameManager : MonoBehaviour
	{
		public enum LoadState
		{
			Unloaded = 0,
			Loading,
			Loaded,
			Unloading
		}


		/// <summary>
		/// The single instance of our in-scene GameManager.
		/// </summary>
		public static GameManager Instance
		{
			get; private set;
		}


		#region Fields

		public List<GameSystem> gameSystems = new List<GameSystem> ();

		private BasicFSM<LoadState> _fsm_loadState;

		private bool requestedLoad = false;
		private bool requestedUnload = false;

		private bool completedLoad = false;
		private bool completedUnload = false;

		#endregion


		//	These are manually set while the Manager loads systems.
		#region Game Systems

		/// <summary>
		/// Manager for registering and broadcasting events.
		/// </summary>
		public static EventManager Events {get; private set;}

		/// <summary>
		/// Manager for registering delayed callbacks.
		/// </summary>
		public static DelayedInvoker DelayedInvoker {get; private set;}

		/// <summary>
		/// Tracks GameObjects to make them more easily accessed.
		/// </summary>
		public static GameObjectTracker ObjectTracker {get; private set;}

		/// <summary>
		/// For adding and clearing text from Hud boxes.
		/// </summary>
		public static HudText HudText {get; private set;}

		/// <summary>
		/// For setting and modifying various universal stats.
		/// </summary>
		public static StatTracker Stats {get; private set;}

		#endregion


		#if UNITY_EDITOR
		//TODO: 
		//TODO: Needs official way to reload systems!
		//TODO: 
		[ContextMenu("Reload Systems from Children")]
		private void ReloadSystemsFromChildren ()
		{
			gameSystems.Clear ();

			//TODO: Produces garbage
			foreach (Transform child in transform)
			{
				GameSystem theSystem = child.GetComponent<GameSystem> () as GameSystem;
				if (theSystem == null)
					continue;

				gameSystems.Add (theSystem);
			}
		}
		#endif


		//	These should be some of the only Unity-default methods in the project.
		//	GameManager controls GameSystem.SystemUpdate().
		//	The EventUpdatesBroadcaster is a GameSystem that sends out the Update
		//	events that almost all other scripts should run off of.
		#region Unity Methods

		/// <summary>
		/// Makes sure there is only one Instance.
		/// </summary>
		void Awake ()
		{
			//	Keep the original instance
			if (Instance != null)
			{
				Destroy (gameObject);
				return;
			}

			//	Initialize the original instance
			Instance = this;
			//DontDestroyOnLoad (this);

			BuildLoadStateFSM ();

			//TODO NEEDS TO BE CALLED BY SEPERATE SYSTEM
			//TODO
			LoadSystems ();
			_fsm_loadState.Update (false);
			//TODO NEEDS TO BE CALLED BY SEPERATE SYSTEM
			//TODO
		}

		/// <summary>
		/// Calls all the GameSystem.SystemUpdate() methods.
		/// Eventually might house logic for pausing too.
		/// </summary>
		void Update ()
		{
			_fsm_loadState.Update (false);
		}

		/// <summary>
		/// Like Update(), calls all the GameSystem.SystemLateUpdate()
		/// methods. Does not go through any fsms, though, and instead
		/// calls on each system directly.
		/// </summary>
		void LateUpdate ()
		{
			if (!Instance.completedLoad)
				return;
			
			for (int i = 0; i < Instance.gameSystems.Count; i++)
			{
				Instance.gameSystems[i].SystemLateUpdate ();
			}
		}

		#endregion


		#region System Access

		public static GameSystemType GetGameSystem<GameSystemType> () where GameSystemType: GameSystem
		{
			if (Instance == null)
				return null;

			for (int i = 0; i < Instance.gameSystems.Count; i++)
			{
				if (Instance.gameSystems[i] is GameSystemType)
					return Instance.gameSystems[i] as GameSystemType;
			}

			return null;
		}

		#endregion


		#region System (Un)Loading

		/// <summary>
		/// Requests loading the GameSystems.
		/// Only works if LoadState is currently Unloaded.
		/// </summary>
		public void LoadSystems ()
		{
			if (!_fsm_loadState.IsCurrentState (LoadState.Unloaded))
				return;

			requestedLoad = true;
		}

		/// <summary>
		/// Requests unloading the GameSystems.
		/// Only works if LoadState is currently Loaded.
		/// </summary>
		public void UnloadSystems ()
		{
			if (!_fsm_loadState.IsCurrentState (LoadState.Loaded))
				return;

			requestedUnload = true;
		}

		#endregion


		#region Helpers

		private void BuildLoadStateFSM ()
		{
			_fsm_loadState = new BasicFSM<LoadState> (LoadState.Unloaded);

			_fsm_loadState.SetMainCallbacks (LoadState.Unloaded,  FSM_Update_Unloaded,  FSM_Enter_Unloaded,  null);
			_fsm_loadState.SetMainCallbacks (LoadState.Loading,   FSM_Update_Loading,   FSM_Enter_Loading,   null);
			_fsm_loadState.SetMainCallbacks (LoadState.Loaded,    FSM_Update_Loaded,    FSM_Enter_Loaded,    null);
			_fsm_loadState.SetMainCallbacks (LoadState.Unloading, FSM_Update_Unloading, FSM_Enter_Unloading, null);

			_fsm_loadState.AddPossibleSwitch (LoadState.Unloaded,  LoadState.Loading,   FSM_Switch_UnloadedToLoading);
			_fsm_loadState.AddPossibleSwitch (LoadState.Loading,   LoadState.Loaded,    FSM_Switch_LoadingToLoaded);
			_fsm_loadState.AddPossibleSwitch (LoadState.Loaded,    LoadState.Unloading, FSM_Switch_LoadedToUnloading);
			_fsm_loadState.AddPossibleSwitch (LoadState.Unloading, LoadState.Unloaded,  FSM_Switch_UnloadingToUnloaded);
		}

		private void InitializeSystems ()
		{
			for (int i = 0; i < gameSystems.Count; i++)
			{
				gameSystems[i].Initialize ();
			}
		}

		private void SceneInitializeSystems ()
		{
			for (int i = 0; i < gameSystems.Count; i++)
			{
				gameSystems[i].SceneInitialize ();
			}
		}

		private void UpdateSystems ()
		{
			for (int i = 0; i < gameSystems.Count; i++)
			{
				gameSystems[i].SystemUpdate ();
			}
		}

		#endregion


		#region LoadState Unloaded

		private void FSM_Enter_Unloaded (LoadState previousState)
		{
			
		}

		private void FSM_Update_Unloaded ()
		{
			
		}

		private bool FSM_Switch_UnloadedToLoading ()
		{
			return requestedLoad;
		}

		#endregion

		#region LoadState Loading

		private void FSM_Enter_Loading (LoadState previousState)
		{
			completedUnload = false;

			requestedLoad = false;
			completedLoad = false;
		}

		private void FSM_Update_Loading ()
		{
			//	Put before GameSystem initialization so they're safer
			Events         = GetGameSystem<EventManager> ();
			DelayedInvoker = GetGameSystem<DelayedInvoker> ();
			ObjectTracker  = GetGameSystem<GameObjectTracker> ();
			HudText        = GetGameSystem<HudText> ();
			Stats          = GetGameSystem<StatTracker> ();

			//TODO: Seperate InitializeSystems and SceneInitializeSystems
			InitializeSystems ();
			SceneInitializeSystems ();

			completedLoad = true;
		}

		private bool FSM_Switch_LoadingToLoaded ()
		{
			return completedLoad;
		}

		#endregion

		#region LoadState Loaded

		private void FSM_Enter_Loaded (LoadState previousState)
		{

		}

		private void FSM_Update_Loaded ()
		{
			//TODO: Logic for pausing or otherwise controlling which systems run
			UpdateSystems ();
		}

		private bool FSM_Switch_LoadedToUnloading ()
		{
			return requestedUnload;
		}

		#endregion

		#region LoadState Unloading

		private void FSM_Enter_Unloading (LoadState previousState)
		{
			completedLoad = false;

			requestedUnload = false;
			completedUnload = false;
		}

		private void FSM_Update_Unloading ()
		{
			//TODO: Unload systems

			completedUnload = true;
		}

		private bool FSM_Switch_UnloadingToUnloaded ()
		{
			return completedUnload;
		}

		#endregion

	}//End Class


}//End Namespace
