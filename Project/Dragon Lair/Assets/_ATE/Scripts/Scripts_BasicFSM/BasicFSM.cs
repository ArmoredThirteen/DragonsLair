using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


//TODO: I believe this script suffers from boxing as explained here:
//TODO: http://www.somasim.com/blog/2015/08/c-performance-tips-for-unity-part-2-structs-and-enums/


public class BasicFSM<T> where T : struct, IConvertible, IComparable
{
	#region Fields
	
	private T _curStateID;
	private Dictionary<T, State> _ourStates = new Dictionary<T, State> ();
	
	#endregion


	#region Constructors

	public BasicFSM (T startingStateID)
	{
		_curStateID = startingStateID;
	}

	#endregion


	public bool IsCurrentState (T compareState)
	{
		return _curStateID.Equals (compareState);
	}

	public T GetCurrentState ()
	{
		return _curStateID;
	}


	#region Add Callbacks

	public void SetMainCallbacks (T stateID, Callback updateCallback, Callback<T> enterCallback, Callback<T> exitCallback)
	{
		SetUpdateCallback (stateID, updateCallback);
		SetEnterCallback  (stateID, enterCallback);
		SetExitCallback   (stateID, exitCallback);
	}


	/// <summary>
	/// Sets _ourStates[stateID]'s update callback.
	/// If there is no preexisting stateID entry it will be added.
	/// </summary>
	public void SetUpdateCallback (T stateID, Callback updateCallback)
	{
		if (updateCallback == null)
			return;

		if (!_ourStates.ContainsKey (stateID))
			_ourStates.Add (stateID, new State ());

		_ourStates[stateID].Update = updateCallback;
	}

	/// <summary>
	/// Sets _ourStates[stateID]'s enter callback.
	/// If there is no preexisting stateID entry it will be added.
	/// </summary>
	public void SetEnterCallback (T stateID, Callback<T> enterCallback)
	{
		if (enterCallback == null)
			return;

		if (!_ourStates.ContainsKey (stateID))
			_ourStates.Add (stateID, new State ());
		
		_ourStates[stateID].Enter = enterCallback;
	}

	/// <summary>
	/// Sets _ourStates[stateID]'s exit callback.
	/// If there is no preexisting stateID entry it will be added.
	/// </summary>
	public void SetExitCallback (T stateID, Callback<T> exitCallback)
	{
		if (exitCallback == null)
			return;

		if (!_ourStates.ContainsKey (stateID))
			_ourStates.Add (stateID, new State ());
		
		_ourStates[stateID].Exit = exitCallback;
	}


	/// <summary>
	/// Adds a new possible switch from fromStateID to toStateID.
	/// If there is no preexisting from/toStateID they will be added.
	/// If given switchCondition is null, this does nothing.
	/// The switchCallback can be null without problems.
	/// </summary>
	public void AddPossibleSwitch (T fromStateID, T toStateID, ReturnCallback<bool> switchCondition, Callback<T,T> switchCallback = null)
	{
		//	No condition to utilize the switchCallback means we shouldn't even have an entry
		if (switchCondition == null)
			return;

		//	Add missing fromState
		if (!_ourStates.ContainsKey (fromStateID))
			_ourStates.Add (fromStateID, new State ());
		//	Add missing toState
		if (!_ourStates.ContainsKey (toStateID))
			_ourStates.Add (toStateID, new State ());

		//	Add missing fromState's possible toState switch
		if (!_ourStates[fromStateID].possibleSwitches.ContainsKey (toStateID))
			_ourStates[fromStateID].possibleSwitches.Add (toStateID, new StateSwitch ());

		_ourStates[fromStateID].possibleSwitches[toStateID].SwitchCondition = switchCondition;

		if (switchCallback != null)
			_ourStates[fromStateID].possibleSwitches[toStateID].SwitchCallback = switchCallback;
	}

	#endregion


	#region State Machine Logic

	/// <summary>
	/// Calls the current state's callback method, and then attempts to switch into a new state.
	/// </summary>
	public void Update (bool updateThenSwitch = true)
	{
		AttemptUpdateAndSwitch (updateThenSwitch);
	}

	/// <summary>
	/// Calls the current state's callback method, and then attempts to switch into a new state.
	/// Continues doing this until it fails to switch states or reaches endAtState.
	/// The state it ends at does not have its Update() called.
	/// </summary>
	public void MultipleUpdate (T endAtState, bool updateThenSwitch = true)
	{
		//	Editor-only feedback to hopefully catch any infinite loops in FSM logic.
		#if UNITY_EDITOR
		int attempts = 0;
		#endif

		T lastStateID = _curStateID;
		do
		{
			lastStateID = _curStateID;
			AttemptUpdateAndSwitch ();

			#if UNITY_EDITOR
			attempts++;
			if ((attempts % 100) == 0)
				Debug.LogError ("A single BasicFSM is attempting hundreds of switches per frame.\r\n" +
					"There could be an infinite loop in the FSM's logic!");
			#endif
		}
		while (!_curStateID.Equals (lastStateID) && !_curStateID.Equals (endAtState));
	}


	/// <summary>
	/// Attempts to update and switch the state machine. If updateFirst
	/// is true, it updates the current state then switches. If it
	/// is false, it switches then updates the new state.
	/// </summary>
	private void AttemptUpdateAndSwitch (bool updateFirst = true)
	{
		if (!_ourStates.ContainsKey (_curStateID))
		{
			Debug.LogError ("[BasicFSM.Update()] No such state '" + _curStateID.ToString() + "' could be found!");
			return;
		}

		T nextStateID = GetNextStateID (_curStateID);
		bool shouldSwitch = !_curStateID.Equals (nextStateID);

		if (!updateFirst && shouldSwitch)
			SwitchToState (nextStateID);

		if (_ourStates[_curStateID].Update != null)
			_ourStates[_curStateID].Update ();

		if (updateFirst && shouldSwitch)
			SwitchToState (nextStateID);
	}


	/// <summary>
	/// Returns what the next state should be.
	/// Assumes _ourStates has a fromStateID.
	/// </summary>
	private T GetNextStateID (T fromStateID)
	{
		//	Search through _ourStates' possible switches for the first succesful ShouldSwitchToState().
		Dictionary<T, StateSwitch>.Enumerator enumerator = _ourStates[fromStateID].possibleSwitches.GetEnumerator ();

		while (enumerator.MoveNext())
		{
			if (ShouldSwitchToState (fromStateID, enumerator.Current.Key))
				return enumerator.Current.Key;
		}

		return fromStateID;
	}

	/// <summary>
	/// Returns true if fromState's possibleSwitches condition callback to toState returns true.
	/// Assumes both _ourStates has a fromStateID and its possible switches has a toStateID.
	/// </summary>
	private bool ShouldSwitchToState (T fromStateID, T toStateID)
	{
		return _ourStates[fromStateID].possibleSwitches[toStateID].SwitchCondition ();
	}

	/// <summary>
	/// Switches _curStateID and calls the appropriate entry/exit callbacks if they're not null.
	/// Assumes _ourStates has valid _curStateID and toStateID,
	/// as well as _curStateID having a valid possibleSwitch to toState.
	/// The switch callback is called between the Exit and Enter callbacks.
	/// </summary>
	private void SwitchToState (T toStateID)
	{
		if (_ourStates[_curStateID].Exit != null)
			_ourStates[_curStateID].Exit (_curStateID);

		if (_ourStates[_curStateID].possibleSwitches[toStateID].SwitchCallback != null)
			_ourStates[_curStateID].possibleSwitches[toStateID].SwitchCallback (_curStateID, toStateID);

		if (_ourStates[toStateID].Enter != null)
			_ourStates[toStateID].Enter (toStateID);

		_curStateID = toStateID;
	}

	#endregion


	#region Helpers



	#endregion


	#region Nested Class - State
	
	/// <summary>
	/// Data only class for individual states.
	/// Should consist almost entirely of callbacks,
	/// which are utilized by BasicFSM's logic.
	/// </summary>
	protected class State
	{
		/// <summary> Update callback for this state. </summary>
		public Callback Update;
		
		/// <summary> Enter callback for this state, where the parameter is the previous state. </summary>
		public Callback<T> Enter;
		
		/// <summary> Exit callback for this state, where the parameter is the next state. </summary>
		public Callback<T> Exit;
		
		/// <summary> Possible states to switch to, and their success and checking callbacks. </summary>
		public Dictionary<T, StateSwitch> possibleSwitches = new Dictionary<T, StateSwitch> ();
	}
	
	#endregion
	
	
	#region Nested Class - StateSwitch
	
	/// <summary>
	/// Data only class for switching states.
	/// Should consiste almost entirely of callbacks,
	/// which are utilized by BasicFSM's logic.
	/// </summary>
	protected class StateSwitch
	{
		/// <summary>
		/// Callback for checking if the state should switch to the next state.
		/// This should never be null, otherwise there would be no possible
		/// way for this StateSwitch to come into effect.
		/// </summary>
		public ReturnCallback<bool> SwitchCondition;
		
		/// <summary>
		/// Callback for if the state successfully switched. Called between exit and enter callbacks.
		/// The paremeterss are the fromState then the toState.
		/// </summary>
		public Callback<T,T> SwitchCallback;
	}
	
	#endregion

}

