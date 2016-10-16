using UnityEngine;
using System.Collections;


public class GameSystem_InputManager : GameSystem
{
	public enum ClickState
	{
		Up,
		Down,
		Held,
	}


	#region Fields

	private BasicFSM<ClickState> _fsm_clickState;

	#endregion


	#region GameSystem

	public override void Initialize ()
	{
		BuildClickStateFSM ();
	}

	public override void SceneInitialize ()
	{
		
	}


	public override void SystemUpdate ()
	{
		_fsm_clickState.Update ();
	}

	#endregion


	//TODO: Should make an abstract class containing most of this
	//TODO: Then have 3 instances of the class
	//TODO: Each tied to their own mouse button or key
	#region ClickState FSM

	private void BuildClickStateFSM ()
	{
		_fsm_clickState = new BasicFSM<ClickState> (ClickState.Up);

		_fsm_clickState.SetMainCallbacks (ClickState.Up,   FSM_Update_Up,   FSM_Enter_Up,   null);
		_fsm_clickState.SetMainCallbacks (ClickState.Down, FSM_Update_Down, FSM_Enter_Down, null);
		//_fsm_clickState.SetMainCallbacks (ClickState.Held, FSM_Update_Held, FSM_Enter_Held, null);

		_fsm_clickState.AddPossibleSwitch (ClickState.Up,   ClickState.Down, FSM_Switch_UpToDown);
		_fsm_clickState.AddPossibleSwitch (ClickState.Down, ClickState.Up,   FSM_Switch_DownToUp);
		//_fsm_clickState.AddPossibleSwitch (ClickState.Down, ClickState.Held, FSM_Switch_DownToHeld);
		//_fsm_clickState.AddPossibleSwitch (ClickState.Held, ClickState.Up,   FSM_Switch_HeldToUp);
	}


	private void FSM_Enter_Up (ClickState previousState)
	{
		GameManager.Events.Broadcast<EventType_UI> ((int)EventType_UI.Released, new EventData_UI (0, Input.mousePosition));
	}

	private void FSM_Update_Up ()
	{
		//Debug.Log ("Up at " + Input.mousePosition);
	}

	private bool FSM_Switch_UpToDown ()
	{
		return Input.GetMouseButtonDown (0);
	}


	private void FSM_Enter_Down (ClickState previousState)
	{
		GameManager.Events.Broadcast<EventType_UI> ((int)EventType_UI.Clicked, new EventData_UI (0, Input.mousePosition));
	}

	private void FSM_Update_Down ()
	{
		//Debug.Log ("Down at " + Input.mousePosition);
	}

	private bool FSM_Switch_DownToUp ()
	{
		return Input.GetMouseButtonUp (0);
	}

	#endregion

}

