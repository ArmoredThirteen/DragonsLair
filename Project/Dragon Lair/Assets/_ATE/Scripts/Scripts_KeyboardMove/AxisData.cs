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

		private bool _isKeyClicked = false;

		#endregion


		#region Properties

		public bool IsKeyClicked
		{
			get {return _isKeyClicked;}
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

		public void OnKeyClicked ()
		{
			_isKeyClicked = true;
		}

		public void OnKeyReleased ()
		{
			_isKeyClicked = false;
		}

		#endregion


		#region Private Methods

		#endregion

	}//End Class
	
	
}//End Namespace
