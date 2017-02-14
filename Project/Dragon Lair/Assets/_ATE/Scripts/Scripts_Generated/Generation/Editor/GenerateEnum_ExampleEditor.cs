using UnityEngine;
using UnityEditor;
using System.Collections;
using ScriptGeneration;


[CustomEditor (typeof(GenerateEnum_Example))]
public class GenerateEnum_ExampleEditor : Editor
{
	private GenerateEnum_Example _targ;


	public override void OnInspectorGUI ()
	{
		_targ = (GenerateEnum_Example)target;
		if (_targ == null)
			return;

		_targ.enumData.DrawInspector (
			GenerateEnum_Data.DrawSettings.DrawPath,
			GenerateEnum_Data.DrawSettings.DrawType);

		EditorHelper.SetDirtyIfChanged (_targ);
	}

}
