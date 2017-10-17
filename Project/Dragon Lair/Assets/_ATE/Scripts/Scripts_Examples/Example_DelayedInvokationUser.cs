using UnityEngine;
using System.Collections;


namespace Ate
{


	public class Example_DelayedInvokationUser : AteComponent
	{
		
		protected override void AteStart ()
		{
			Debug.Log ("Delayed Invokation Began at time: " + Time.time);
			GameManager.DelayedInvoker.Register (2, RepeatingMethod);
			GameManager.DelayedInvoker.Register (1, DelayedMethod, "Delayed Method One");
			GameManager.DelayedInvoker.Register (3, DelayedMethod, "Delayed Method Two");
		}


		protected override void AteUpdate ()
		{
			if (Time.time >= 7)
			{
				Debug.LogError ("Destroyed after 7 seconds at time: " + Time.time);
				Destroy (this.gameObject);
			}
		}


		private void RepeatingMethod (object callbackInput)
		{
			//	Highlights one of the BIG ISSUES with the DelayedInvoker
			//	Will still call delayed methods after caller is destroyed.
			if (this == null)
			{
				Debug.LogError ("Repeating Method attempted but we're null.\r\nManually returned from method at time: " + Time.time);
				return;
			}
			
			Debug.Log ("Repeating Method at time: " + Time.time);
			GameManager.DelayedInvoker.Register (2, RepeatingMethod);
		}


		private void DelayedMethod (object callbackInput)
		{
			string input = callbackInput as string;
			if (input == null)
			{
				Debug.Log ("Delayed Method called with null input at time: " + Time.time);
			}
			Debug.Log (input + " at time: " + Time.time);
		}

	}//End Class


}//End Namespace
