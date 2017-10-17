using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections;
using System.Collections.Generic;


namespace Ate
{


	public class GameSystem_DelayedInvoker : GameSystem
	{
		#region Ordered Callbacks

		private List<DelayedCallback> _noPauseCallbacks = new List<DelayedCallback> ();

		#endregion


		#region GameSystem

		public override void Initialize (){}
		public override void SceneInitialize (){}


		public override void SystemUpdate ()
		{
			InvokeCallbacks (Time.time, ref _noPauseCallbacks);
		}

		public override void SystemLateUpdate (){}

		#endregion


		//TODO: Make it so when GameObject or script is destroyed, they unregister.
		/// <summary>
		/// Registers a delayed callback.
		/// After the delay, will only call if caller is not null.
		/// WARNING: Delayed calls still happen after GameObject is destroyed.
		/// </summary>
		public void Register (float delay, Callback<object> callback, object callbackInput = null)
		{
			float callTime = Time.time + delay;
			_noPauseCallbacks.Add (new DelayedCallback (callTime, callback, callbackInput));
			_noPauseCallbacks.Sort ();
		}

		/// <summary>
		/// Don't know how to do this one yet :(
		/// </summary>
		public void Unregister (Callback callback)
		{
			
		}


		private void InvokeCallbacks (float curTime, ref List<DelayedCallback> callbacks)
		{
			if (callbacks == null)
				return;
			if (callbacks.Count <= 0)
				return;

			//	Find out how many callbacks have timed out
			int invokesThisFrame = 0;
			for (int i = 0; i < callbacks.Count; i++)
			{
				if (callbacks[i].callTime > curTime)
					break;
				invokesThisFrame++;
			}

			//	Attempt to invoke callbacks
			for (int i = 0; i < invokesThisFrame; i++)
			{
				if (callbacks[i] == null)
					continue;
				if (callbacks[i].theCallback == null)
					continue;
				
				callbacks[i].theCallback (callbacks[i].callbackInput);
			}

			//	Remove callbacks from list
			for (int i = 0; i < invokesThisFrame; i++)
			{
				callbacks.RemoveAt (0);
			}
		}
		

		private void Cleanup ()
		{
			
		}



		#region DelayedCallback Class

		private class DelayedCallback : IComparable
		{
			public float            callTime;
			public Callback<object> theCallback;
			public object           callbackInput;

			public DelayedCallback (float callTime, Callback<object> theCallback)
			{
				this.callTime      = callTime;
				this.theCallback   = theCallback;
				this.callbackInput = null;
			}

			public DelayedCallback (float callTime, Callback<object> theCallback, object callbackInput)
			{
				this.callTime      = callTime;
				this.theCallback   = theCallback;
				this.callbackInput = callbackInput;
			}

			public int CompareTo (object obj)
			{
				DelayedCallback toCompare = obj as DelayedCallback;
				if (obj == null)
					return 1;

				if (callTime > toCompare.callTime)
					return 1;
				else if (callTime < toCompare.callTime)
					return -1;

				return 0;
			}
		}

		#endregion

	}//End Class


}//End Namespace
