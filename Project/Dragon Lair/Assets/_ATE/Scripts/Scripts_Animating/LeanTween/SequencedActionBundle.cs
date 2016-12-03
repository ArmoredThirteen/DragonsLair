using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[System.Serializable]
public class SequencedActionBundle : AteGameObject
{
	//TODO: Ripe for abuse
	public bool playing = false;

	public List<SequencedAction> Actions
	{get; private set;}


	protected override void AteAwake()
	{
		SetActionsFromChildren ();

		for (int i = 0; i < Actions.Count; i++)
		{
			Actions[i].Initialize ();
		}
	}

	protected override void AteUpdate()
	{
		if (playing)
			UpdateActions ();
	}


	public void SetActionsFromChildren ()
	{
		List<SequencedAction> actions = new List<SequencedAction> ();

		SequencedAction[] children = gameObject.GetComponentsInChildren<SequencedAction> () as SequencedAction[];
		for (int i = 0; i < children.Length; i++)
		{
			actions.Add (children[i]);
		}

		Actions = actions;
	}


	private void UpdateActions ()
	{
		if (Actions.Count <= 0)
			return;

		for (int i = 0; i < Actions.Count; i++)
		{
			//	This one has already completed
			if (Actions[i].CurrentState == SequencedAction.ActionState.Played)
				continue;

			//	Command needs to begin playing
			if (Actions[i].CurrentState == SequencedAction.ActionState.Unplayed)
				Actions[i].RequestPlayCommand ();

			//	Update FSM in Playing or Unplayed state
			Actions[i].UpdateFSM ();

			//	Potentially delay next command
			if (Actions[i].delayNextUntilCompleted)
				break;
		}
	}

}
