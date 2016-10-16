#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class TriggeredBehaviour_Sequencer : TriggeredBehaviour
{
	public enum SequenceType
	{
		/// <summary>
		/// Upon entering the Playing state, requests a play
		/// from all behaviours then completes itself.
		/// Order of behaviours is ARBITRARY.
		/// </summary>
		Simultaneous,

		/// <summary>
		/// Similar to Simultaneous, but requests a play
		/// from all behaviours at the rate of one per frame.
		/// Then it completes itself.
		/// This preserves the order of behaviours at the
		/// cost of not doing them all in the same frame.
		/// </summary>
		RapidInOrder,

		/// <summary>
		/// During the Playing state update, requests a play
		/// from the next unplayed behaviour in the list, if
		/// the previous behaviour is completed.
		/// </summary>
		OneAtATime,
		/// <summary>
		/// Like OneAtATime but will attempt to automatically
		/// put some Ready behaviours into Playing.
		/// For time-based OneAtATime instead of reset-based.
		/// If the first incomplete behaviour is playing, does
		/// nothing on update. If it is Ready, plays it then
		/// does nothing to next ones.
		/// </summary>
		OneAtATime_AutomaticPlayReady,
	}

	//	Variables for designers.
	//	Shown in editor with DrawInspector() at bottom.
	#region Public Variables

	/// <summary>
	/// Used by the editor to create serializable object references.
	/// Unity cannot serialize a list of abstract classes for _behaviours.
	/// </summary>
	public List<Transform> behaviourTransforms = new List<Transform> ();
	public SequenceType sequenceType = SequenceType.Simultaneous;

	#endregion


	#region Private Variables

	/// <summary>
	/// A list of behaviours built on Awake from the behaviourTransforms.
	/// </summary>
	private List<TriggeredBehaviour> _behaviours;

	//	Used by RapidInOrder update to request one call per frame
	//	Unlike Simultaneous which plays on the same frame but not in order.
	private int _nextActivate = 0;

	#endregion


	#region Awake/Start

	/// <summary>
	/// Called by parent class at the end of its AteAwake().
	/// </summary>
	protected override void OnAwake ()
	{
		//	Populate _behaviours so we don't have to use GetComponent all the time
		_behaviours = new List<TriggeredBehaviour> ();
		for (int i = 0; i < behaviourTransforms.Count; i++)
		{
			if (behaviourTransforms[i] == null)
				continue;

			TriggeredBehaviour behaviour = behaviourTransforms[i].GetComponent<TriggeredBehaviour> () as TriggeredBehaviour;
			if (behaviour != null)
				_behaviours.Add (behaviour);
		}
	}

	/// <summary>
	/// Called by AteGameObject at end of its Awake().
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
		for (int i = 0; i < _behaviours.Count; i++)
		{
			_behaviours[i].DataReset ();
		}
	}


	/// <summary>
	/// Called when parent class had a request to play.
	/// If inactive and cancelRequestsWhileInactive is true, won't be called.
	/// </summary>
	protected override void OnRequestedPlaying ()
	{
		switch (sequenceType)
		{
			case SequenceType.Simultaneous :
				OnRequestedPlaying_Simultaneous ();
				break;
			case SequenceType.RapidInOrder :
				OnRequestedPlaying_RapidInOrder ();
				break;
			case SequenceType.OneAtATime :
			case SequenceType.OneAtATime_AutomaticPlayReady :
				OnRequestedPlaying_OneAtATime ();
				break;
		}
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
		for (int i = 0; i < _behaviours.Count; i++)
		{
			if (_behaviours[i].IsComplete)
				_behaviours[i].RequestReset ();
		}
	}


	/// <summary>
	/// Calls RequestPlaying() on all Ready _behaviours.
	/// </summary>
	private void OnRequestedPlaying_Simultaneous ()
	{
		for (int i = 0; i < _behaviours.Count; i++)
		{
			if (_behaviours[i].IsReady)
				_behaviours[i].RequestPlaying ();
		}
	}

	/// <summary>
	/// Currently does nothing. RequestPlaying() is handled in OnUpdatePlaying_RapidInOrder().
	/// </summary>
	private void OnRequestedPlaying_RapidInOrder ()
	{
		
	}

	/// <summary>
	/// Calls RequestPlaying() on the first Ready _behaviour.
	/// </summary>
	private void OnRequestedPlaying_OneAtATime ()
	{
		for (int i = 0; i < _behaviours.Count; i++)
		{
			if (_behaviours[i].IsReady)
			{
				_behaviours[i].RequestPlaying ();
				break;
			}
		}
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
		
	}

	/// <summary>
	/// Called when behaviour enters the Complete state.
	/// Happens after a RequestComplete() call and CanSwitchToComplete is true.
	/// </summary>
	protected override void OnEnteredComplete (TriggeredState prevState)
	{
		_nextActivate = 0;
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
		TriggeredBehaviour nextIncomplete = GetNextIncompleteBehaviour ();
		if (nextIncomplete == null)
		{
			RequestComplete ();
			return;
		}

		switch (sequenceType)
		{
			case SequenceType.Simultaneous :
				OnUpdatePlaying_Simultaneous (nextIncomplete);
				break;
			case SequenceType.RapidInOrder :
				OnUpdatePlaying_RapidInOrder (nextIncomplete);
				break;
			case SequenceType.OneAtATime :
			case SequenceType.OneAtATime_AutomaticPlayReady :
				OnUpdatePlaying_OneAtATime (nextIncomplete);
				break;
		}
	}

	/// <summary>
	/// Called every frame behaviour is in the Complete state.
	/// This will happen after Playing until it is Reset or canceled.
	/// </summary>
	protected override void OnUpdateComplete ()
	{
		
	}


	private void OnUpdatePlaying_Simultaneous (TriggeredBehaviour nextIncomplete)
	{
		if (nextIncomplete.IsReady && nextIncomplete.CurPlaysRequested >= 0)
			RequestComplete ();
	}

	private void OnUpdatePlaying_RapidInOrder (TriggeredBehaviour nextIncomplete)
	{
		if (_nextActivate >= _behaviours.Count)
		{
			if (nextIncomplete.IsReady && nextIncomplete.CurPlaysRequested >= 0)
			{
				RequestComplete ();
			}
			return;
		}

		TriggeredBehaviour behaviour = _behaviours[_nextActivate];

		if (behaviour.IsReady)
			behaviour.RequestPlaying ();

		_nextActivate++;
	}

	private void OnUpdatePlaying_OneAtATime (TriggeredBehaviour nextIncomplete)
	{
		//TODO: This method and the area that calls it needs some revision.

		//if (sequenceType == SequenceType.OneAtATime_AutomaticPlayReady && nextIncomplete.IsReady)
		//	nextIncomplete.RequestPlaying ();
		if (sequenceType == SequenceType.OneAtATime)
			return;

		if (nextIncomplete.IsPlaying)
			return;
		if (nextIncomplete.IsReady)
			nextIncomplete.RequestPlaying ();

		/*for (int i = 0; i < _behaviours.Count; i++)
		{
			if (_behaviours[i].IsPlaying)
				break;
			if (_behaviours[i].IsReady)
			{
				_behaviours[i].RequestPlaying ();
				break;
			}
		}*/
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

	/// <summary>
	/// Returns the first _behaviour that is not in a Complete state.
	/// Returns null if all _behaviours are Complete.
	/// </summary>
	private TriggeredBehaviour GetNextIncompleteBehaviour ()
	{
		for (int i = 0; i < _behaviours.Count; i++)
		{
			if (!_behaviours[i].IsComplete)
				return _behaviours[i];
		}

		return null;
	}

	#endregion


	#if UNITY_EDITOR

	/// <summary>
	/// Called by parent class for drawing specific variables at top.
	/// Parent class should automatically check for when it is dirty.
	/// </summary>
	protected override void DrawChildInspector ()
	{
		sequenceType = (SequenceType)EditorGUILayout.EnumPopup ("Sequence Type", sequenceType);
		EditorHelper.DrawResizableList<Transform> ("Triggered Behaviours", ref behaviourTransforms, DrawTriggeredBehaviourEntry, null, null, null, null, false);
	}

	private void DrawTriggeredBehaviourEntry (int index)
	{
		TriggeredBehaviour behaviour = DrawBehaviourField (index);
		if (behaviour == null)
			return;
		
		behaviour.DrawInspector ();
	}

	private TriggeredBehaviour DrawBehaviourField (int index)
	{
		Transform          curTransform = behaviourTransforms[index];
		TriggeredBehaviour curBehaviour = null;
		if (curTransform != null)
			curBehaviour = curTransform.GetComponent<TriggeredBehaviour> () as TriggeredBehaviour;

		TriggeredBehaviour newBehaviour = EditorGUILayout.ObjectField
			("Behaviour", curBehaviour, typeof (TriggeredBehaviour), true)
			as TriggeredBehaviour;

		if (newBehaviour == null)
			behaviourTransforms[index] = null;
		else
			behaviourTransforms[index] = newBehaviour.transform;

		return newBehaviour;
	}

	#endif

}
