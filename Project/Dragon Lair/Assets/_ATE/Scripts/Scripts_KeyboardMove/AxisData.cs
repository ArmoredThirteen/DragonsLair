using UnityEngine;
using System.Collections;
using Ate.GameSystems;

#if UNITY_EDITOR
using UnityEditor;
#endif


namespace Ate.KeyboardMove
{
	
	
	/// <summary>
	/// Description
	/// </summary>
	[System.Serializable]
	public class AxisData
	{
		public enum AxisDir
		{
			X,
			Y,
			Z,
		}


		#region Public Variables

		public bool isUsed = true;

		public KeyCode activateKey = KeyCode.None;

		public AxisDir axisDirection = AxisDir.X;
		public bool    negativeAxis  = false;

		#endregion


		#region Private Variables

		private bool _isKeyPressed = false;

		#endregion


		#region Properties

		public bool IsKeyPressed
		{
			get {return _isKeyPressed;}
		}

		#endregion


		#if UNITY_EDITOR

		public void DrawInspector ()
		{
			isUsed = EditorGUILayout.Toggle ("Is Used", isUsed);
			if (!isUsed)
				return;
			
			activateKey = (KeyCode)EditorGUILayout.EnumPopup ("Activate Key", activateKey);

			axisDirection = (AxisDir)EditorGUILayout.EnumPopup ("Axis Direction", axisDirection);
			negativeAxis  = EditorGUILayout.Toggle             ("Negative Axis",  negativeAxis);
		}

		#endif


		#region Public Methods

		public void RegisterEvents ()
		{
			GameManager.Events.Register<EventType_UI, EventData_UI>
				((int)EventType_UI.Clicked, OnKeyClicked);

			GameManager.Events.Register<EventType_UI, EventData_UI>
				((int)EventType_UI.Released, OnKeyReleased);
		}

		public void UnregisterEvents ()
		{
			GameManager.Events.Unregister<EventType_UI, EventData_UI>
				((int)EventType_UI.Clicked, OnKeyClicked);

			GameManager.Events.Unregister<EventType_UI, EventData_UI>
				((int)EventType_UI.Released, OnKeyReleased);
		}

		#endregion


		#region Private Methods

		private void OnKeyClicked (EventData_UI eventData)
		{
			if (eventData.TheKey != activateKey)
				return;

			_isKeyPressed = true;
		}

		private void OnKeyReleased (EventData_UI eventData)
		{
			if (eventData.TheKey != activateKey)
				return;

			_isKeyPressed = false;
		}

		#endregion

	}//End Class
	
	
}//End Namespace
