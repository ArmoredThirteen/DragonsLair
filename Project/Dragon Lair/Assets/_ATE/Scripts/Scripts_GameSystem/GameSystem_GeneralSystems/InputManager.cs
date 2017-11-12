//#define DebugClickstates
//#define DebugEnter


using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Ate.FSM;


namespace Ate.GameSystems
{


	public class InputManager : GameSystem
	{
		public enum MouseState
		{
			Stopped,
			Moving,
		}

		public enum ClickState
		{
			Up,
			Down,
			Held,
		}


		#region Public Fields

		public float moveSensitivity = 0.0f;
		public float moveCheckTime   = 1.0f;
		public float satStillTime    = 5.0f;

		public List<KeyCode> keysToTrack = new List<KeyCode> (new KeyCode[] {KeyCode.Mouse0});

		#endregion


		#region Private Fields

		private BasicFSM<MouseState> _fsm_mouseState;

		private Vector3 _lastMousePos        = new Vector3 ();
		private float   _timer_minMovingTime = 0;
		private bool    _mouseMovedThisFrame = false;

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
			InitializeMouseStateFSM ();
		}


		public override void SystemUpdate ()
		{
			Vector3 curMousePos  = Input.mousePosition;
			float   sqrDistance  = _lastMousePos.SqrDistanceTo (curMousePos);
			_mouseMovedThisFrame = (sqrDistance > (moveSensitivity * moveSensitivity));
			_lastMousePos = curMousePos;

			_fsm_mouseState.Update ();

			for (int i = 0; i < _inputs.Count; i++)
			{
				_inputs[i].Update ();
			}
		}

		public override void SystemLateUpdate (){}

		#endregion


		#region Mouse Move/Stop FSM

		private void InitializeMouseStateFSM ()
		{
			_fsm_mouseState = new BasicFSM<MouseState> (MouseState.Stopped);

			_fsm_mouseState.SetMainCallbacks (MouseState.Stopped,  FSM_Update_Stopped,  FSM_Enter_Stopped,  null);
			_fsm_mouseState.SetMainCallbacks (MouseState.Moving,   FSM_Update_Moving,   FSM_Enter_Moving,   null);

			_fsm_mouseState.AddPossibleSwitch (MouseState.Stopped,  MouseState.Moving,   FSM_Switch_StoppedToMoving);
			_fsm_mouseState.AddPossibleSwitch (MouseState.Moving,   MouseState.Stopped,  FSM_Switch_MovingToStopped);
		}


		private void FSM_Enter_Stopped (MouseState previousState)
		{
			EventData_UI theData = new EventData_UI ();
			GameManager.Events.Broadcast<EventType_UI> ((int)EventType_UI.MouseStopped, theData);

			#if DebugEnter && UNITY_EDITOR
			Debug.Log ("Entered: MouseState.Stopped");
			#endif
		}

		private void FSM_Update_Stopped ()
		{
			
		}

		private bool FSM_Switch_StoppedToMoving ()
		{
			return _mouseMovedThisFrame;
		}


		private void FSM_Enter_Moving (MouseState previousState)
		{
			_timer_minMovingTime = moveCheckTime;

			EventData_UI theData = new EventData_UI ();
			GameManager.Events.Broadcast<EventType_UI> ((int)EventType_UI.MouseMoved, theData);

			#if DebugEnter && UNITY_EDITOR
			Debug.Log ("Entered: MouseState.Moving");
			#endif
		}

		private void FSM_Update_Moving ()
		{
			_timer_minMovingTime = _timer_minMovingTime - Time.deltaTime;

			if (_mouseMovedThisFrame)
				_timer_minMovingTime = moveCheckTime;
		}

		private bool FSM_Switch_MovingToStopped ()
		{
			return _timer_minMovingTime <= 0;
		}

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
				#if DebugClickstates && UNITY_EDITOR
				Debug.Log (theKey.ToString () + "Clicked Up.\r\nMouse at position: " + Input.mousePosition);
				#endif

				EventData_UI theData = new EventData_UI (theKey, Input.mousePosition);
				GameManager.Events.Broadcast<EventType_UI> ((int)EventType_UI.Released, theData);
			}

			private void FSM_Update_Up ()
			{
				
			}

			private bool FSM_Switch_UpToDown ()
			{
				return Input.GetKeyDown (theKey);
			}


			private void FSM_Enter_Down (ClickState previousState)
			{
				#if DebugClickstates && UNITY_EDITOR
				Debug.Log (theKey.ToString () + "Clicked Down.\r\nMouse at position: " + Input.mousePosition);
				#endif

				EventData_UI theData = new EventData_UI (theKey, Input.mousePosition);
				GameManager.Events.Broadcast<EventType_UI> ((int)EventType_UI.Clicked, theData);
			}

			private void FSM_Update_Down ()
			{
				
			}

			private bool FSM_Switch_DownToUp ()
			{
				return Input.GetKeyUp (theKey);
			}

			#endregion
		}

	}//End Class


}//End Namespace
