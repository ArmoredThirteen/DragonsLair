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

			EditorHelper.DrawResizableList<KeyCode> ("Key Codes to Track", ref _target.keysToTrack, DrawEntry_KeyCode);

			if (GUI.changed)
			{
				EditorUtility.SetDirty (target);

				//	If it is a scene object and the application isn't playing
				if (!string.IsNullOrEmpty (_target.gameObject.scene.name) && !Application.isPlaying)
				{
					EditorApplication.MarkSceneDirty ();
				}
			}
		}


		private void DrawEntry_KeyCode (int index)
		{
			_target.keysToTrack[index] = (KeyCode)EditorGUILayout.EnumPopup ("KeyCode", _target.keysToTrack[index]);
		}

	}//End Class


}//End Namespace
