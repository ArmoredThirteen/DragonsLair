using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Ate.FSM;


namespace Ate.GameSystems
{


	public class InputManager : GameSystem
	{
		public enum ClickState
		{
			Up,
			Down,
			Held,
		}


		#region Fields

		public List<KeyCode> keysToTrack = new List<KeyCode> (new KeyCode[] {KeyCode.Mouse0});

		private List<HoldableInput> _inputs = new List<HoldableInput> ();

		#endregion


		#region GameSystem

		public override void Initialize ()
		{
			_inputs.Clear ();

			for (int i = 0; i < keysToTrack.Count; i++)
			{
				_inputs.Add (new HoldableInput (keysToTrack[i]));
			}

			for (int i = 0; i < _inputs.Count; i++)
			{
				_inputs[i].Initialize ();
			}
		}

		public override void SceneInitialize ()
		{
			
		}


		public override void SystemUpdate ()
		{
			for (int i = 0; i < _inputs.Count; i++)
			{
				_inputs[i].Update ();
			}
		}

		public override void SystemLateUpdate (){}

		#endregion


		/// <summary>
		/// For keeping track of inputs the user can press/hold.
		/// Such as mouse and keyboard buttons.
		/// </summary>
		private class HoldableInput
		{
			public KeyCode theKey = KeyCode.Mouse0;

			private BasicFSM<ClickState> _fsm_clickState;


			public HoldableInput (KeyCode theKey)
			{
				this.theKey = theKey;
			}


			public void Initialize ()
			{
				BuildClickStateFSM ();
			}

			public void Update ()
			{
				_fsm_clickState.Update ();
			}


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
				EventData_UI theData = new EventData_UI (theKey, Input.mousePosition);
				GameManager.Events.Broadcast<EventType_UI> ((int)EventType_UI.Released, theData);
			}

			private void FSM_Update_Up ()
			{
				//Debug.Log ("Up at " + Input.mousePosition);
			}

			private bool FSM_Switch_UpToDown ()
			{
				return Input.GetKeyDown (theKey);
			}


			private void FSM_Enter_Down (ClickState previousState)
			{
				EventData_UI theData = new EventData_UI (theKey, Input.mousePosition);
				GameManager.Events.Broadcast<EventType_UI> ((int)EventType_UI.Clicked, theData);
			}

			private void FSM_Update_Down ()
			{
				//Debug.Log ("Down at " + Input.mousePosition);
			}

			private bool FSM_Switch_DownToUp ()
			{
				return Input.GetKeyUp (theKey);
			}

			#endregion
		}

	}//End Class


}//End Namespace
