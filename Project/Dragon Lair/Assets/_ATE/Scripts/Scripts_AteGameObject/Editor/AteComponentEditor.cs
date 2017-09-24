using UnityEngine;
using System.Collections;
using UnityEditor;


[CustomEditor (typeof(AteComponent), true)]
public class AteComponentEditor : Editor
{
	private AteComponent _target;


	public override void OnInspectorGUI ()
	{
		_target = (AteComponent)target;

		_target.DrawInspector ();

		EditorHelper.SetDirtyIfChanged (_target);
	}

}
