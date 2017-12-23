using UnityEngine;
using System.Collections;
using Ate.GameSystems;

#if UNITY_EDITOR
using UnityEditor;
#endif


namespace Ate
{
	
	
	/// <summary>
	/// Similar to a normal AteComponent but with extra
	/// functionality to control its update timing.
	/// All FPS are 24 or lower (depending on frame length).
	/// </summary>
	public abstract class AteComponent_fpsControlled : AteComponent
	{

		public enum FrameLengthSetting
		{
			/// <summary> Updates each frame up to the maximum framerate. In this case that is 24 frames per second. </summary>
			None      = 0,

			/// <summary> Frame length taken from the static frameLength_universal setting. </summary>
			Universal = 100,

			/// <summary> Frame length taken from the internal frameLength_custom setting. </summary>
			Custom    = 200,
		}

		
		#region Public Variables

		/// <summary>
		/// If true, updates happen based on the internal update count.
		/// If false, updates happen based on the given external event's update count.
		/// </summary>
		public bool localUpdate = false;

		public FrameLengthSetting frameLengthFrom = FrameLengthSetting.Universal;
		public int frameLength_custom = 4;

		#endregion


		#region Private Variables

		#if UNITY_EDITOR
		private bool _showUpdateSettings = false;
		#endif

		private UpdateBroadcaster _updateSystem = null;

		private int _framesPlayed = 0;

		#endregion


		#if UNITY_EDITOR

		public override void DrawInspector ()
		{
			base.DrawInspector ();

			EditorGUILayout.Space ();

			_showUpdateSettings = EditorGUILayout.Toggle ("Show Update Settings", _showUpdateSettings);

			if (!_showUpdateSettings)
				return;

			localUpdate = EditorGUILayout.Toggle ("Local Update", localUpdate);

			frameLengthFrom = (FrameLengthSetting)EditorGUILayout.EnumPopup ("Frame Length From", frameLengthFrom);

			if (frameLengthFrom == FrameLengthSetting.Universal)
			{
				//TODO: Find the object in the scene because this throws nulls in editor
				//UpdateBroadcaster theUpdateBroadcaster = GameManager.GetGameSystem<UpdateBroadcaster> ();
				//EditorGUILayout.LabelField ("Current Universal Frame Length: " + theUpdateBroadcaster.controlledFPS_universalFrameLength);
				EditorGUILayout.LabelField ("Go to the UpdateBroadcaster GameSystem to change the Universal Frame Length.");
			}
			else if (frameLengthFrom == FrameLengthSetting.Custom)
			{
				frameLength_custom    = EditorGUILayout.IntField ("Custom Frame Length",    frameLength_custom);
			}
		}

		#endif


		#region Properties

		#endregion


		#region AteComponent

		protected override void AteStart ()
		{
			base.AteStart ();

			_updateSystem = GameManager.GetGameSystem<UpdateBroadcaster> ();
			if (_updateSystem == null)
			{
				#if UNITY_EDITOR
				Debug.LogError ("FPS controlled component could not find an UpdateBroadcaster system\r\n" +
					"Component has been automatically removed!");
				#endif
				_myObject.DestroyComponent (this);
				return;
			}
		}


		protected override void RegisterEvents ()
		{
			base.RegisterEvents();

			GameManager.Events.Register<EventType_Updates, EventData_Updates>
			((int)EventType_Updates.fpsUpdate24, OnFpsUpdate24);
		}

		protected override void UnregisterEvents ()
		{
			base.UnregisterEvents();

			GameManager.Events.Unregister<EventType_Updates, EventData_Updates>
			((int)EventType_Updates.fpsUpdate24, OnFpsUpdate24);
		}


		protected abstract void UpdateBaseFps ();
		protected abstract void UpdateFrameLength ();

		#endregion


		#region Public Methods

		/// <summary>
		/// Slow method for getting a frame length based on current settings.
		/// </summary>
		public int GetFrameLength ()
		{
			//TODO: Hard-coded goodness
			int value = 24;

			switch (frameLengthFrom)
			{
				case FrameLengthSetting.None:
					//TODO: Hard-coded goodness
					value = 24;
					break;

				case FrameLengthSetting.Universal:
					value = _updateSystem.controlledFPS_universalFrameLength;
					break;

				case FrameLengthSetting.Custom:
					value = frameLength_custom;
					break;
			}

			return value;
		}

		#endregion


		#region Private Methods

		private void OnFpsUpdate24 (EventData_Updates eventData)
		{
			UpdateBaseFps ();

			//	No framelength setting, just update at max fps
			if (frameLengthFrom == FrameLengthSetting.None)
			{
				UpdateFrameLength ();
				//TODO: Putting the increment in 2 spots is hacky
				_framesPlayed += 1;
				return;
			}

			int frameLength = GetFrameLength ();
			bool shouldUpdate = false;

			if (localUpdate)
			{
				if ((_framesPlayed % frameLength) == 0)
					shouldUpdate = true;
			}
			else
			{
				if ((eventData.updateIndex % frameLength) == 0)
					shouldUpdate = true;
			}

			if (shouldUpdate)
			{
				UpdateFrameLength ();
			}

			//TODO: Putting the increment in 2 spots is hacky
			_framesPlayed += 1;
		}

		#endregion

	}//End Class
	
	
}//End Namespace
