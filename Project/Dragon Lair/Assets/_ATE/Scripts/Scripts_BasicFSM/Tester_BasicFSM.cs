using UnityEngine;
using System.Collections;


namespace Ate
{


	public class Tester_BasicFSM : MonoBehaviour
	{

		public bool switchCondition = false;

		public enum TestStates
		{
			StateOne,
			StateTwo,
			StateThree,
		}

		public BasicFSM<TestStates> _fsm;


		void Awake ()
		{
			_fsm = new BasicFSM<TestStates> (TestStates.StateOne);
			
			_fsm.SetMainCallbacks (TestStates.StateOne, FSM_UpdateStateOne, FSM_EnterStateOne, FSM_ExitStateOne);
			_fsm.SetMainCallbacks (TestStates.StateTwo, FSM_UpdateStateTwo, FSM_EnterStateTwo, FSM_ExitStateTwo);
			_fsm.SetMainCallbacks (TestStates.StateThree, FSM_UpdateStateThree, FSM_EnterStateThree, FSM_ExitStateThree);
			
			_fsm.AddPossibleSwitch (TestStates.StateOne, TestStates.StateTwo, FSM_ShouldSwitchStates, FSM_SwitchingFromStateOneToStateTwo);
			_fsm.AddPossibleSwitch (TestStates.StateTwo, TestStates.StateThree, FSM_ShouldSwitchStates, FSM_SwitchingFromStateTwoToStateThree);
			_fsm.AddPossibleSwitch (TestStates.StateThree, TestStates.StateTwo, FSM_ShouldSwitchStates, FSM_SwitchingFromStateThreeToStateTwo);
		}

		void Update ()
		{
			_fsm.Update ();
		}


		private bool FSM_ShouldSwitchStates ()
		{
			return switchCondition;
		}


		#region StateOne
		
		private void FSM_EnterStateOne (TestStates previousState)
		{
			Debug.Log ("Enter State One");
		}
		
		private void FSM_UpdateStateOne ()
		{
			Debug.Log ("Update State One");
		}
		
		private void FSM_ExitStateOne (TestStates nextState)
		{
			Debug.Log ("Exit State One");
			switchCondition = false;
		}

		private void FSM_SwitchingFromStateOneToStateTwo (TestStates oldState, TestStates newState)
		{
			Debug.Log ("Switching from One to Two");
		}
		
		#endregion


		#region StateTwo
		
		private void FSM_EnterStateTwo (TestStates previousState)
		{
			Debug.Log ("Enter State Two");
		}
		
		private void FSM_UpdateStateTwo ()
		{
			//Debug.Log ("Update State Two");
		}
		
		private void FSM_ExitStateTwo (TestStates nextState)
		{
			Debug.Log ("Exit State Two");
			switchCondition = false;
		}
		
		private void FSM_SwitchingFromStateTwoToStateThree (TestStates oldState, TestStates newState)
		{
			Debug.Log ("Switching from Two to Three");
		}
		
		#endregion


		#region StateThree
		
		private void FSM_EnterStateThree (TestStates previousState)
		{
			Debug.Log ("Enter State Three");
		}
		
		private void FSM_UpdateStateThree ()
		{
			//Debug.Log ("Update State Three");
		}
		
		private void FSM_ExitStateThree (TestStates nextState)
		{
			Debug.Log ("Exit State Three");
			switchCondition = false;
		}
		
		private void FSM_SwitchingFromStateThreeToStateTwo (TestStates oldState, TestStates newState)
		{
			Debug.Log ("Switching from Three to Two");
		}
		
		#endregion

	}//End Class


}//End Namespace
