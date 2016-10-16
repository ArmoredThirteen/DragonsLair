using UnityEngine;
using System.Collections;
using UnityEditor;


[CustomEditor (typeof(AteGameObject), true)]
public class AteGameObjectEditor : Editor
{
	private AteGameObject _target;


	public override void OnInspectorGUI()
	{
		_target = (AteGameObject)target;

		_target.DrawInspector ();

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
}
