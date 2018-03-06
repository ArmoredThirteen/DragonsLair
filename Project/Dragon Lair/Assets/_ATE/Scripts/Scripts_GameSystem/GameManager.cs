//#define DebugLoadState


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

		private bool _requestedLoad   = false;
		private bool _requestedUnload = false;

		private bool _completedLoad   = false;
		private bool _completedUnload = false;

		#endregion


		#region Properties

		public bool IsLoaded
		{
			get { return _completedLoad; }
		}

		public bool IsUnloaded
		{
			get { return _completedUnload; }
		}

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

		/// <summary>
		/// Handles pooling and unpooling of frequently used objects.
		/// </summary>
		public static PoolManager Pooling {get; private set;}

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
			HandleSingleton ();
			if (Instance != this)
				return;

			//	Put before InitializeSystems() so systems can safely refer to the properties
			SetGameSystemProperties ();

			BuildLoadStateFSM ();
			InitializeSystems ();

			#if DebugLoadState
			Debug.LogError ("Awake(): Initial Request Loading");
			#endif
			RequestLoadSystems ();
			//	Unloaded to Loading
			UpdateFSM ();
		}

		private void HandleSingleton ()
		{
			if (Instance == null)
			{
				Instance = this;
				DontDestroyOnLoad (gameObject);
			}
			else if (Instance != this)
			{
				DestroyImmediate (gameObject);
			}
		}

		private void SetGameSystemProperties ()
		{
			Events         = GetGameSystem<EventManager> ();
			DelayedInvoker = GetGameSystem<DelayedInvoker> ();
			ObjectTracker  = GetGameSystem<GameObjectTracker> ();
			HudText        = GetGameSystem<HudText> ();
			Stats          = GetGameSystem<StatTracker> ();
			Pooling        = GetGameSystem<PoolManager> ();
		}


		/// <summary>
		/// Unity's built-in update.
		/// Will maybe eventually house logic for scene changing or pausing
		/// </summary>
		void Update ()
		{
			UpdateFSM ();
		}


		/// <summary>
		/// Unity's build-in event for scene changes
		/// </summary>
		void OnLevelWasLoaded ()
		{
			//TODO: Should instead have another script/method handle scene loading and unloading
			//		It would properly order how things get loaded and unloaded during scene changes
			// ChangeScene()
			//    GameManager.UnloadSystems ();
			//    when systems unloaded ->
			//        Unity.SceneChange ();
			// OnLevelLoaded()
			//    GameManager.LoadSystems ();
			//    when systems loaded ->
			//        SendEvent (GameSystemsLoaded);

			#if DebugLoadState
			Debug.LogError ("OnLevelWasLoaded(): Request Unloading");
			#endif
			//	Force a system unload
			RequestUnloadSystems ();
			//	Loaded to Unloading, update Unloading, Unloading to Unloaded
			UpdateFSM ();
			UpdateFSM ();

			//	In an actual scene-change management solution, the scene change would go here

			#if DebugLoadState
			Debug.LogError ("OnLevelWasLoaded(): Request Loading");
			#endif
			//	Now load the systems again
			RequestLoadSystems ();
			//	Unloaded to Loading, update Loading
			UpdateFSM ();
			//	Should be able to stay at Loading and they'll automatically go to Loaded this frame or next

			//	Now that the systems are properly loaded, do initialization stuff.
			//	As the loading/unloading/scene changing gets better segmented
			//	this should be seperated from the GameManager.
			SceneInitializeSystems ();
		}


		/// <summary>
		/// Updates _fsm_loadState once.
		/// </summary>
		void UpdateFSM (bool updateThenSwitch = false)
		{
			_fsm_loadState.Update (updateThenSwitch);
		}

		/// <summary>
		/// Like Update(), calls all the GameSystem.SystemLateUpdate()
		/// methods. Does not go through any fsms, though, and instead
		/// calls on each system directly.
		/// </summary>
		void LateUpdate ()
		{
			if (!Instance._completedLoad)
				return;
			
			for (int i = 0; i < Instance.gameSystems.Count; i++)
			{
				Instance.gameSystems[i].SystemLateUpdate ();
			}
		}

		#endregion


		#region System Access

		/// <summary>
		/// Searches the gameSystems list for the first (and hopefully only)
		/// matching GameSystem. Should be safe to use at any time but the
		/// received systems shouldn't be trusted until after Awake().
		/// </summary>
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
		public void RequestLoadSystems ()
		{
			if (!_fsm_loadState.IsCurrentState (LoadState.Unloaded))
				return;

			_requestedLoad = true;
		}

		/// <summary>
		/// Requests unloading the GameSystems.
		/// Only works if LoadState is currently Loaded.
		/// </summary>
		public void RequestUnloadSystems ()
		{
			if (!_fsm_loadState.IsCurrentState (LoadState.Loaded))
				return;

			_requestedUnload = true;
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
				gameSystems[i].SceneLoaded ();
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
			return _requestedLoad;
		}

		#endregion

		#region LoadState Loading

		private void FSM_Enter_Loading (LoadState previousState)
		{
			_completedUnload = false;

			_requestedLoad = false;
			_completedLoad = false;
		}

		private void FSM_Update_Loading ()
		{
			_completedLoad = true;
		}

		private bool FSM_Switch_LoadingToLoaded ()
		{
			return IsLoaded;
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
			return _requestedUnload;
		}

		#endregion

		#region LoadState Unloading

		private void FSM_Enter_Unloading (LoadState previousState)
		{
			_completedLoad = false;

			_requestedUnload = false;
			_completedUnload = false;
		}

		private void FSM_Update_Unloading ()
		{
			//TODO: Unload systems

			_completedUnload = true;
		}

		private bool FSM_Switch_UnloadingToUnloaded ()
		{
			return IsUnloaded;
		}

		#endregion

	}//End Class


}//End Namespace
