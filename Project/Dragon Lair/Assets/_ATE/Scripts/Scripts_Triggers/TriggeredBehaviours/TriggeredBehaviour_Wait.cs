using UnityEngine;
using System.Collections;
using Ate.GameSystems;

#if UNITY_EDITOR
using UnityEditor;
#endif


namespace Ate
{


	/// <summary>
	/// For use with a Sequencer that is set to play OnAtATime.
	/// Causes a delay in the OneAtATime play type.
	/// </summary>
	public class TriggeredBehaviour_Wait : TriggeredBehaviour
	{
		public enum WaitType
		{
			Seconds = 0,

			Frames = 100,
		}


		//	Variables for designers.
		//	Shown in editor with DrawInspector() at bottom.
		#region Public Variables

		public WaitType waitType = WaitType.Seconds;

		public float minWait = 1;
		public float maxWait = 2;

		/// <summary>
		/// If true, updates frame when internal update count matches frame length.
		/// If false, updates frame when event data update count matches frame length.
		/// </summary>
		public bool localUpdate         = false;
		public bool useLocalFrameLength = false;

		public int frameLength = 3;

		#endregion


		#region Private Variables

		private float _rolledDuration;
		private float _timer_duration;

		private int _totalFramesPlayed = 0;
		/// <summary> If using 24fps updates, increments +1 every frameLengths' worth of frames. </summary>
		private int _totalFrameLengthsPlayed = 0;

		#endregion


		#if UNITY_EDITOR

		/// <summary>
		/// Called by parent class for drawing specific variables at top.
		/// Parent class should automatically check for when it is dirty.
		/// </summary>
		protected override void DrawChildInspector ()
		{
			waitType = (WaitType)EditorGUILayout.EnumPopup ("Wait Type", waitType);

			switch (waitType)
			{
				case WaitType.Seconds:
					Draw_WaitType_Seconds ();
					break;

				case WaitType.Frames:
					Draw_WaitType_Frames ();
					break;
			}
		}

		private void Draw_WaitType_Seconds ()
		{
			minWait = EditorGUILayout.FloatField ("Min Wait", minWait);
			maxWait = EditorGUILayout.FloatField ("Max Wait", maxWait);
		}

		private void Draw_WaitType_Frames ()
		{
			minWait = (float)EditorGUILayout.IntField ("Min Wait", (int)minWait);
			maxWait = (float)EditorGUILayout.IntField ("Max Wait", (int)maxWait);

			localUpdate = EditorGUILayout.Toggle ("Local Update", localUpdate);

			useLocalFrameLength = EditorGUILayout.Toggle ("Local Frame Length", useLocalFrameLength);

			if (useLocalFrameLength)
				frameLength = EditorGUILayout.IntField ("Frame Length", frameLength);
		}

		#endif


		#region Awake/Start

		/// <summary>
		/// Called by parent class at the end of its AteAwake().
		/// </summary>
		protected override void OnAwake ()
		{
			
		}

		/// <summary>
		/// Called by AteObject at end of its Awake().
		/// </summary>
		protected override void AteStart ()
		{
			
		}


		protected override void RegisterEvents ()
		{
			base.RegisterEvents();

			GameManager.Events.Register<EventType_Updates, EventData_Updates>
			((int)EventType_Updates.fpsUpdate24, OnFpsUpdate24);
		}

		protected override void UnregisterEvents ()
		{
			base.RegisterEvents();

			GameManager.Events.Unregister<EventType_Updates, EventData_Updates>
			((int)EventType_Updates.fpsUpdate24, OnFpsUpdate24);
		}

		#endregion


		#region Private Methods

		private void OnFpsUpdate24 (EventData_Updates eventData)
		{
			_totalFramesPlayed += 1;

			int currFrame = localUpdate ? _totalFramesPlayed : eventData.updateIndex;
			int currFrameLength = useLocalFrameLength ? frameLength : eventData.universalFrameLength;

			if ((currFrame % currFrameLength) == 0)
				_totalFrameLengthsPlayed += 1;
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
			
		}


		/// <summary>
		/// Called when parent class had a request to play.
		/// If inactive and cancelRequestsWhileInactive is true, won't be called.
		/// </summary>
		protected override void OnRequestedPlaying (AteObject triggerer)
		{
			
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
			_timer_duration    = 0;
			_totalFramesPlayed = 0;
			_totalFrameLengthsPlayed = 0;

			if (waitType == WaitType.Seconds)
				_rolledDuration = Random.Range (minWait, maxWait);
			else if (waitType == WaitType.Frames)
				_rolledDuration = (float)Random.Range ((int)minWait, (int)maxWait);

			//	Called at end of this method for an instant-fire behaviour
			//RequestComplete ();
		}

		/// <summary>
		/// Called when behaviour enters the Complete state.
		/// Happens after a RequestComplete() call and CanSwitchToComplete is true.
		/// </summary>
		protected override void OnEnteredComplete (TriggeredState prevState)
		{
			
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
			if (waitType == WaitType.Seconds)
			{
				_timer_duration += Time.deltaTime;
				if (_timer_duration >= _rolledDuration)
					RequestComplete();
			}
			else if (waitType == WaitType.Frames)
			{
				if (_totalFrameLengthsPlayed >= _rolledDuration)
					RequestComplete ();
			}

			//	Called when an end-condition happens (such as a timer)
			//RequestComplete ();
		}

		/// <summary>
		/// Called every frame behaviour is in the Complete state.
		/// This will happen after Playing until it is Reset or canceled.
		/// </summary>
		protected override void OnUpdateComplete ()
		{
			
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

		#endregion

	}//End Class


}//End Namespace
