using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif


[System.Serializable]
public abstract class TriggeredBehaviour : AteGameObject
{
	public enum TriggeredState
	{
		Ready = 0,
		Playing,
		Paused,
		Complete,
	}


	#region Public Variables

	/// <summary>
	/// For inspectors to know if they should show base settings.
	/// Should be editor-only but not sure how to save toggling and have it editor-only.
	/// </summary>
	public bool showBaseSettings  = true;
	/// <summary>
	/// For inspectors to know if they should show child settings.
	/// Should be editor-only but not sure how to save toggling and have it editor-only.
	/// </summary>
	public bool showChildSettings = true;

	public bool isActive = true;
	public bool cancelRequestsWhileInactive = true;

	public int playOnRequest = 1;
	public int afterResetPlayOnRequest = 1;
	public int maxPlayResets = -1;

	//	Sends a reset request when it enters Complete
	public bool automaticPlayReset = true;

	#endregion


	#region Private Variables

	private bool _startingIsActive;

	private BasicFSM<TriggeredState> _fsm;

	private bool _requestedPlaying  = false;
	private bool _requestedComplete = false;
	private bool _requestedPlayReset    = false;

	private int _playsRequested = 0;
	private int _resetCount = 0;

	#endregion


	#if UNITY_EDITOR

	protected abstract void DrawChildInspector ();

	public override void DrawInspector ()
	{
		base.DrawInspector ();

		float labelFieldWidth = 135;
		float dataEntryWidth = 40;

		EditorGUILayout.BeginVertical ();

		//	Toggle which settings to show
		DrawShowSettingsToggles (labelFieldWidth);

		if (showBaseSettings)
		{
			DrawBaseSettings (labelFieldWidth, dataEntryWidth);
		}

		if (showChildSettings)
		{
			DrawChildInspector ();	
		}

		EditorHelper.SetDirtyIfChanged (this);

		EditorGUILayout.EndVertical ();
	}

	private void DrawShowSettingsToggles (float labelFieldWidth)
	{
		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField ("Show Settings", GUILayout.Width (labelFieldWidth));

		EditorGUILayout.LabelField ("Base", GUILayout.Width (40+(EditorGUI.indentLevel*10)));
		showBaseSettings  = EditorGUILayout.Toggle (showBaseSettings, GUILayout.Width (30+(EditorGUI.indentLevel*6)));

		EditorGUILayout.LabelField ("Child", GUILayout.Width (40+(EditorGUI.indentLevel*10)));
		showChildSettings = EditorGUILayout.Toggle (showChildSettings);

		EditorGUILayout.EndHorizontal ();
	}

	private void DrawBaseSettings (float labelFieldWidth, float dataEntryWidth)
	{
		dataEntryWidth += EditorGUI.indentLevel * 10;

		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField ("Is Active", GUILayout.Width (labelFieldWidth));
		isActive = EditorGUILayout.Toggle (isActive);
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField ("No Inactive Requests", GUILayout.Width (labelFieldWidth));
		cancelRequestsWhileInactive = EditorGUILayout.Toggle (cancelRequestsWhileInactive);
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField ("Play on Request", GUILayout.Width (labelFieldWidth));
		playOnRequest = EditorGUILayout.IntField (playOnRequest, GUILayout.Width (dataEntryWidth));
		//EditorGUILayout.EndHorizontal ();

		//EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField ("post Reset", GUILayout.Width (75));
		afterResetPlayOnRequest = EditorGUILayout.IntField (afterResetPlayOnRequest, GUILayout.Width (dataEntryWidth));
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField ("Max Play Resets", GUILayout.Width (labelFieldWidth));
		maxPlayResets = EditorGUILayout.IntField (maxPlayResets, GUILayout.Width (dataEntryWidth));
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField ("Automatic Play Reset", GUILayout.Width (labelFieldWidth));
		automaticPlayReset = EditorGUILayout.Toggle (automaticPlayReset);
		EditorGUILayout.EndHorizontal ();
	}

	#endif


	#region Properties

	public bool IsReady    {get {return _fsm.GetCurrentState ().Equals(TriggeredState.Ready);}}
	public bool IsPlaying  {get {return _fsm.GetCurrentState ().Equals(TriggeredState.Playing);}}
	public bool IsPaused   {get {return _fsm.GetCurrentState ().Equals(TriggeredState.Paused);}}
	public bool IsComplete {get {return _fsm.GetCurrentState ().Equals(TriggeredState.Complete);}}

	protected int ResetCount {get {return _resetCount;}}

	public int CurPlaysRequested {get {return _playsRequested;}}

	private int CurPlayOnRequest {
		get {
			if (_resetCount == 0)
				return playOnRequest;
			return afterResetPlayOnRequest;
		}}

	#endregion


	#region AteGameObject

	protected override void AteAwake()
	{
		_startingIsActive = isActive;
		BuildFSM ();
		OnAwake ();
	}


	protected override void AteUpdate ()
	{
		if (!isActive)
			return;
		
		UpdateFSM ();
	}

	#endregion


	#region Public Methods

	/// <summary>
	/// Resets the data to a more 'factory default' version.
	/// For things like only playing a sequencer once, then
	/// resetting it from a different behaviour somewhere
	/// else so it can be played again.
	/// Also resets isActive and rebuilds the FSM.
	/// </summary>
	public void DataReset ()
	{
		isActive = _startingIsActive;
		_resetCount = 0;
		_playsRequested = 0;
		BuildFSM ();

		OnDataReset ();
	}


	/// <summary>
	/// Requests putting the FSM into Playing state.
	/// Only works if FSM is in Ready state.
	/// </summary>
	public void RequestPlaying (AteGameObject triggerer)
	{
		if (!isActive && cancelRequestsWhileInactive)
			return;
		
		//if (IsReady)
		{
			_playsRequested++;
			_requestedPlaying = true;
			OnRequestedPlaying (triggerer);
		}
	}

	/// <summary>
	/// Requests putting the FSM into Complete state.
	/// Only works if FSM is in Playing state.
	/// Generally used by children to indicate they're complete.
	/// </summary>
	public void RequestComplete ()
	{
		if (!isActive && cancelRequestsWhileInactive)
			return;

		//if (IsPlaying)
		{
			_requestedComplete = true;
			OnRequestedComplete ();
		}
	}

	/// <summary>
	/// Requests putting the FSM into Ready state.
	/// Only works if FSM is in Complete state.
	/// </summary>
	public void RequestReset ()
	{
		if (!isActive && cancelRequestsWhileInactive)
			return;

		//if (IsComplete)
		{
			_requestedPlayReset = true;
			OnRequestedPlayReset ();
		}
	}

	#endregion


	#region Abstract Methods

	protected abstract void OnAwake ();

	protected abstract void OnDataReset ();

	protected abstract void OnRequestedPlaying (AteGameObject triggerer);
	protected abstract void OnRequestedComplete ();
	protected abstract void OnRequestedPlayReset ();

	protected abstract void OnEnteredReady (/*bool wasReset, */TriggeredState prevState);
	protected abstract void OnEnteredPlaying (TriggeredState prevState);
	//protected abstract void OnEnteredPaused (TriggeredState prevState);
	protected abstract void OnEnteredComplete (TriggeredState prevState);

	protected abstract void OnUpdateReady ();
	protected abstract void OnUpdatePlaying ();
	//protected abstract void OnUpdatePaused ();
	protected abstract void OnUpdateComplete ();

	protected abstract bool CanSwitchToPlaying ();
	protected abstract bool CanSwitchToComplete ();
	protected abstract bool CanPlayReset ();

	#endregion


	#region FSM

	private void BuildFSM ()
	{
		_fsm = new BasicFSM<TriggeredState> (TriggeredState.Ready);

		_fsm.SetMainCallbacks (TriggeredState.Ready,    FSM_UpdateReady,    FSM_EnteringReady,    null);
		_fsm.SetMainCallbacks (TriggeredState.Playing,  FSM_UpdatePlaying,  FSM_EnteringPlaying,  null);
		_fsm.SetMainCallbacks (TriggeredState.Complete, FSM_UpdateComplete, FSM_EnteringComplete, null);

		_fsm.AddPossibleSwitch (TriggeredState.Ready,    TriggeredState.Playing,  FSM_ShouldSwitchToPlaying);
		_fsm.AddPossibleSwitch (TriggeredState.Playing,  TriggeredState.Complete, FSM_ShouldSwitchToComplete);
		_fsm.AddPossibleSwitch (TriggeredState.Complete, TriggeredState.Ready,    FSM_ShouldSwitchToReady);
	}

	private void UpdateFSM ()
	{
		if (_fsm == null)
			return;

		_fsm.MultipleUpdate (TriggeredState.Complete);
	}

	#endregion

	#region FSM Callbacks

	public void FSM_EnteringReady (TriggeredState prevState)
	{
		//bool wasReset = prevState == TriggeredState.Complete;
		bool wasReset = true;
		if (wasReset)
		{
			//isActive = _startingIsActive;
			_playsRequested = 0;
			_resetCount++;
			_requestedPlayReset = false;
		}
		
		OnEnteredReady (/*wasReset, */prevState);
	}

	public void FSM_EnteringPlaying (TriggeredState prevState)
	{
		//if (prevState == TriggeredState.Ready)
			_requestedPlaying = false;

		OnEnteredPlaying (prevState);
	}

	public void FSM_EnteringComplete (TriggeredState prevState)
	{
		//if (prevState == TriggeredState.Playing)
			_requestedComplete = false;

		if (automaticPlayReset)
			RequestReset ();

		OnEnteredComplete (prevState);
	}


	public void FSM_UpdateReady ()
	{
		OnUpdateReady ();
	}

	public void FSM_UpdatePlaying ()
	{
		OnUpdatePlaying ();
	}

	public void FSM_UpdateComplete ()
	{
		OnUpdateComplete ();
	}


	public bool FSM_ShouldSwitchToPlaying ()
	{
		if (_playsRequested < CurPlayOnRequest)
			return false;
		
		return _requestedPlaying && CanSwitchToPlaying ();
	}

	public bool FSM_ShouldSwitchToComplete ()
	{
		return _requestedComplete && CanSwitchToComplete ();
	}

	public bool FSM_ShouldSwitchToReady ()
	{
		if (maxPlayResets >= 0)
		{
			if (_resetCount >= maxPlayResets)
				return false;
		}

		return _requestedPlayReset && CanPlayReset ();
	}

	#endregion

}
