using UnityEngine;
using System.Collections;


public abstract class SequencedAction : MonoBehaviour
{
	public enum ActionState
	{
		Unplayed,
		Playing,
		Paused,
		Played,
	}


	public float actionDuration = 1;
	public bool  delayNextUntilCompleted = true;

	private BasicFSM<ActionState> _fsm;

	private bool _playRequested = false;
	private bool _playCompleted = false;


	public ActionState CurrentState
	{
		get {return _fsm.GetCurrentState ();}
	}


	protected abstract void OnBeganPlay ();

	//protected abstract void OnPauseBegan ();
	//protected abstract void OnPauseEnded ();

	protected abstract void OnEndedPlay ();


	public void Initialize ()
	{
		_fsm = new BasicFSM<ActionState> (ActionState.Unplayed);

		_fsm.SetMainCallbacks (ActionState.Unplayed, FSM_UpdateUnplayed, null, null);
		_fsm.SetMainCallbacks (ActionState.Playing,  FSM_UpdatePlaying,  FSM_EnterPlaying, null);
		_fsm.SetMainCallbacks (ActionState.Played,   FSM_UpdatePlayed,   null, null);

		_fsm.AddPossibleSwitch (ActionState.Unplayed, ActionState.Playing, FSM_ShouldSwitchToPlaying);
		_fsm.AddPossibleSwitch (ActionState.Playing,  ActionState.Played,  FSM_ShouldSwitchToPlayed);
	}


	public void UpdateFSM ()
	{
		if (_fsm == null)
			return;

		_fsm.Update ();
	}


	public void RequestPlayCommand ()
	{
		_playRequested = true;
	}

	/// <summary>
	/// For child Actions to call when they're completed.
	/// </summary>
	protected void OnActionCompleted ()
	{
		_playCompleted = true;
	}


	#region FSM Callbacks

	private void FSM_UpdateUnplayed ()
	{

	}


	private bool FSM_ShouldSwitchToPlaying ()
	{
		return _playRequested;
	}


	private void FSM_EnterPlaying (ActionState previousState)
	{
		OnBeganPlay ();
	}

	private void FSM_UpdatePlaying ()
	{

	}


	private bool FSM_ShouldSwitchToPlayed ()
	{
		return _playCompleted;
	}


	private void FSM_EnteredPlayed (ActionState previousState)
	{
		OnEndedPlay ();
	}

	private void FSM_UpdatePlayed ()
	{

	}

	#endregion

}
