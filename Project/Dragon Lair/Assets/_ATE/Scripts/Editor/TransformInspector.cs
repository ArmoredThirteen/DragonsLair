// Reverse engineered UnityEditor.TransformInspector
//	Taken from
//	http://wiki.unity3d.com/index.php?title=TransformInspector

using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(Transform))]
public class TransformInspector : Editor
{
	public override void OnInspectorGUI()
	{
		
		Transform t = (Transform)target;
		
		// Replicate the standard transform inspector gui
		EditorGUIUtility.LookLikeControls();
		EditorGUI.indentLevel = 0;


		//	Position
		EditorGUILayout.BeginHorizontal ();
		//	reset
		if (GUILayout.Button ("X", GUILayout.Width (20)))
			t.localPosition = Vector3.zero;
		//	label
		GUILayout.Label ("Position", GUILayout.Width (100));
		//	values
		Vector3 position = EditorGUILayout.Vector3Field(new GUIContent (), t.localPosition);
		EditorGUILayout.EndHorizontal ();

		//	Rotation
		EditorGUILayout.BeginHorizontal ();
		//	reset
		if (GUILayout.Button ("X", GUILayout.Width (20)))
			t.localEulerAngles = Vector3.zero;
		//	label
		GUILayout.Label ("Rotation", GUILayout.Width (100));
		//	values
		Vector3 eulerAngles = EditorGUILayout.Vector3Field(new GUIContent (), t.localEulerAngles);
		EditorGUILayout.EndHorizontal ();

		//	Scale
		EditorGUILayout.BeginHorizontal ();
		//	reset
		if (GUILayout.Button ("X", GUILayout.Width (20)))
			t.localScale = new Vector3 (1, 1, 1);
		//	label
		GUILayout.Label ("Scale", GUILayout.Width (100));
		//	values
		Vector3 scale = EditorGUILayout.Vector3Field(new GUIContent (), t.localScale);
		EditorGUILayout.EndHorizontal ();


		EditorGUIUtility.LookLikeInspector();
		
		if (GUI.changed)
		{
			Undo.RegisterUndo(t, "Transform Change");
			
			t.localPosition = FixIfNaN(position);
			t.localEulerAngles = FixIfNaN(eulerAngles);
			t.localScale = FixIfNaN(scale);
		}
	}
	
	private Vector3 FixIfNaN(Vector3 v)
	{
		if (float.IsNaN(v.x))
		{
			v.x = 0;
		}
		if (float.IsNaN(v.y))
		{
			v.y = 0;
		}
		if (float.IsNaN(v.z))
		{
			v.z = 0;
		}
		return v;
	}
	
}