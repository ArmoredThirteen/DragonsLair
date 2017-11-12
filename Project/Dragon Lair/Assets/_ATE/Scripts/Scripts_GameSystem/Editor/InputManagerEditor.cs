using UnityEngine;
using System.Collections;
using UnityEditor;
using Ate.GameSystems;
using Ate.EditorHelpers;


namespace Ate.GameSystems
{


	[CustomEditor (typeof(InputManager))]
	public class InputManagerEditor : Editor
	{
		private InputManager _target;


		public override void OnInspectorGUI()
		{
			_target = (InputManager)target;

			_target.moveSensitivity = EditorGUILayout.FloatField ("Move Sensitivity", _target.moveSensitivity);
			_target.moveCheckTime   = EditorGUILayout.FloatField ("Move Check Time",  _target.moveCheckTime);
			_target.satStillTime    = EditorGUILayout.FloatField ("Sat Still Time",   _target.satStillTime);

			EditorHelper.DrawResizableList<KeyCode> ("Key Codes to Track", ref _target.keysToTrack, DrawEntry_KeyCode);

			EditorHelper.SetDirtyIfChanged (_target);
		}


		private void DrawEntry_KeyCode (int index)
		{
			_target.keysToTrack[index] = (KeyCode)EditorGUILayout.EnumPopup ("KeyCode", _target.keysToTrack[index]);
		}

	}//End Class


}//End Namespace
