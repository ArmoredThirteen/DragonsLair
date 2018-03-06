using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace Ate.GameSystems
{


	public class UpdateBroadcaster : GameSystem
	{
		public int controlledFPS_universalFrameLength = 3;

		private static List<EventData_Updates> _data_systemUpdates     = new List<EventData_Updates> ();
		private static List<EventData_Updates> _data_systemLateUpdates = new List<EventData_Updates> ();

		private static List<EventData_Updates> _data_fps_updates       = new List<EventData_Updates> ();
		private static List<int>               _data_fps_updateCount   = new List<int> ();

		private static List<float>             _data_fps_timeIntervals = new List<float> ();
		private static List<float>             _data_fps_timers        = new List<float> ();


		#region GameSystem

		public override void Initialize ()
		{
			_data_systemUpdates.Clear ();
			_data_systemLateUpdates.Clear ();

			_data_fps_updates.Clear ();

			_data_fps_timeIntervals.Clear ();
			_data_fps_timers.Clear ();


			for (int i = 0; i < 5; i++)
			{
				_data_systemUpdates.Add (new EventData_Updates (i));
			}

			for (int i = 0; i < 5; i++)
			{
				_data_systemLateUpdates.Add (new EventData_Updates (i));
			}

			for (int i = 0; i < 24; i++)
			{
				//	Actual value is modified each frame as it is sent out
				_data_fps_updates.Add (new EventData_Updates (0));
				_data_fps_updateCount.Add (0);

				float timeInterval = 1.0f/(i+1);
				_data_fps_timeIntervals.Add (timeInterval);
				_data_fps_timers.Add (timeInterval);
			}
		}


		public override void SceneLoaded (){}


		public override void SystemUpdate ()
		{
			BroadcastSystemUpdates ();
			ProcessFPS ();
		}

		public override void SystemLateUpdate ()
		{
			BroadcastSystemLateUpdates ();
		}

		#endregion


		#region Public Methods

		public void ModUniversalFramelength (int amount)
		{
			int newLength = controlledFPS_universalFrameLength + amount;

			//newLength = Mathf.Min (newLength, 24);
			newLength = Mathf.Max (newLength, 1);

			controlledFPS_universalFrameLength = newLength;
		}

		#endregion


		#region Private Methods

		private void BroadcastSystemUpdates ()
		{
			BroadcastUpdate (EventType_Updates.UpdateOne, _data_systemUpdates[0]);
			BroadcastUpdate (EventType_Updates.UpdateTwo, _data_systemUpdates[1]);
		}

		private void BroadcastSystemLateUpdates ()
		{
			BroadcastUpdate (EventType_Updates.LateUpdateOne, _data_systemLateUpdates[0]);
		}


		private void BroadcastUpdate (EventType_Updates eventType, EventData_Updates eventData)
		{
			GameManager.Events.Broadcast<EventType_Updates> ((int)eventType, eventData);
		}


		private void ProcessFPS ()
		{
			float dTime = Time.deltaTime;

			//TODO: More than just the fps24 update
			for (int i = 0; i < _data_fps_updates.Count; i++)
			{
				_data_fps_timers[i] = _data_fps_timers[i] - dTime;

				//	Time for this fps' frame hasn't run out
				if (_data_fps_timers[i] > 0)
					continue;

				BroadcastFPS (i);
			}
		}

		private void BroadcastFPS (int index)
		{
			//	Set up event data
			EventData_Updates updateData    = _data_fps_updates[index];
			updateData.updateIndex          = _data_fps_updateCount[index];
			updateData.universalFrameLength = controlledFPS_universalFrameLength;

			//	Progress this fps' frame
			_data_fps_updateCount[index] = _data_fps_updateCount[index] + 1;
			_data_fps_timers[index]      = _data_fps_timeIntervals[index] + _data_fps_timers[index];

			//TODO: More than just the fps24 update
			if (index != 23)
				return;

			BroadcastUpdate (EventType_Updates.fpsUpdate24, updateData);
		}

		#endregion


	}//End Class


}//End Namespace
