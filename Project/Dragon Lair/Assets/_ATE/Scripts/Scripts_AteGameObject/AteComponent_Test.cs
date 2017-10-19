using UnityEngine;
using System.Collections;
using Ate.GameSystems;

#if UNITY_EDITOR
using UnityEditor;
#endif


namespace Ate
{


	/// <summary>
	/// Description
	/// </summary>
	public class AteComponent_Test : AteComponent
	{
		
		#region Public Variables

		#endregion


		#region Private Variables

		private float _lastTime_fpsUpdate = 0;
		private float _lastTime_fpsSecond = 0;

		#endregion


		#if UNITY_EDITOR

		public override void DrawInspector ()
		{
			base.DrawInspector ();
		}

		#endif


		#region AteComponent

		protected override void AteUpdate ()
		{
			
		}


		protected override void RegisterEvents ()
		{
			GameManager.Events.Register<EventType_Updates, EventData_Updates>
				((int)EventType_Updates.fpsUpdate24, OnFpsUpdate24);
		}

		protected override void UnregisterEvents ()
		{
			GameManager.Events.Unregister<EventType_Updates, EventData_Updates>
				((int)EventType_Updates.fpsUpdate24, OnFpsUpdate24);
		}

		#endregion


		#region Public Methods

		#endregion


		#region Private Methods

		private void OnFpsUpdate24 (EventData_Updates eventData)
		{
			float timeSinceLastUpdate = Time.time - _lastTime_fpsUpdate;
			_lastTime_fpsUpdate = Time.time;
			Debug.Log ("dTime: " + timeSinceLastUpdate);

			if ((eventData.updateIndex % 24) == 0)
			{
				float timeSinceLastSecond = Time.time - _lastTime_fpsSecond;
				_lastTime_fpsSecond = Time.time;
				Debug.LogError ("secTime: " + timeSinceLastSecond);
			}
		}

		#endregion

	}//End Class


}//End Namespace
