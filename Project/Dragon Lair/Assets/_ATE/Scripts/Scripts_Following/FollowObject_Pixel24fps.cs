using UnityEngine;
using System.Collections;
using Ate.GameSystems;

#if UNITY_EDITOR
using UnityEditor;
#endif


namespace Ate
{


	/// <summary>
	/// Follows a target object using a more retro feel.
	/// Intended to snap to pixels on slower framerate.
	/// Uses target object as guide to determine when to snap.
	/// </summary>
	public class FollowObject_Pixel24fps : AteComponent
	{

		#region Public Variables

		public GameObject targetObject = null;

		public int pixelsPerUnit = 8;

		/// <summary>
		/// If true, updates frame when internal update count matches frame length.
		/// If false, updates frame when event data update count matches frame length.
		/// </summary>
		public bool localUpdate = false;
		public int frameLength = 6;

		#endregion


		#region Private Variables

		private int _curFrame = 0;
		private int _totalFramesPlayed = 0;

		#endregion


		#if UNITY_EDITOR

		public override void DrawInspector ()
		{
			base.DrawInspector ();

			targetObject = EditorGUILayout.ObjectField
				("Target", targetObject, typeof(GameObject), true)
				as GameObject;

			pixelsPerUnit = EditorGUILayout.IntField ("Pixels per Unit", pixelsPerUnit);

			localUpdate = EditorGUILayout.Toggle ("Local Update", localUpdate);
			frameLength = EditorGUILayout.IntField ("Frame Length", frameLength);
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
			bool shouldUpdate = false;

			if (localUpdate)
			{
				if ((_totalFramesPlayed % frameLength) == 0)
					shouldUpdate = true;
			}
			else
			{
				if ((eventData.updateIndex % frameLength) == 0)
					shouldUpdate = true;
			}

			if (shouldUpdate)
			{
				UpdateLocation ();
			}

			_totalFramesPlayed += 1;
		}

		private void UpdateLocation ()
		{
			float desiredX = targetObject.GetPosition ().x;
			float desiredY = targetObject.GetPosition ().y;
			float desiredZ = targetObject.GetPosition ().z;

			desiredX = Mathf.Round (desiredX * pixelsPerUnit) / pixelsPerUnit;
			desiredY = Mathf.Round (desiredY * pixelsPerUnit) / pixelsPerUnit;
			desiredZ = Mathf.Round (desiredZ * pixelsPerUnit) / pixelsPerUnit;

			Position = new Vector3 (desiredX, desiredY, desiredZ);
		}

		#endregion

	}//End Class


}//End Namespace
