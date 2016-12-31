using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif


[System.Serializable]
public class GenerateEnum_Data
{
	public bool showData = true;


	[System.Serializable]
	public class SingleEnum_Data
	{
		public string enumName = "Default";
		public int    enumID   = 0;

		public SingleEnum_Data ()
		{
			enumName = "Default";
			enumID   = 0;
		}

		public SingleEnum_Data (string enumName, int enumID)
		{
			this.enumName = enumName;
			this.enumID   = enumID;
		}
	}


	public string enumPath = "Assets/_Ate/Scripts/Scripts_Generated/Generated_Enums/";
	public string enumType = "GeneratedEnum";

	public List<SingleEnum_Data> enumValues = new List<SingleEnum_Data> ();


	#if UNITY_EDITOR

	public void DrawInspector ()
	{
		DrawInspector (null, null, null, null, null);
	}

	public void DrawInspector (Callback<int> drawCallback)
	{
		DrawInspector (drawCallback, null, null, null, null);
	}

	public void DrawInspector (Callback<int> drawCallback, Callback<int> addCallback, Callback<int> delCallback, Callback<int> moveUpCallback, Callback<int> moveDownCallback)
	{
		showData = EditorGUILayout.Toggle ("Show "+enumType, showData);
		if (!showData)
			return;

		enumPath = EditorGUILayout.TextField ("Path", enumPath);
		enumType = EditorGUILayout.TextField ("Type", enumType);

		EditorGUILayout.BeginHorizontal ();

		DrawButton_Generate ();
		DrawButton_LoadExisting ();
		EditorGUILayout.Space ();

		EditorGUILayout.EndHorizontal ();

		if (drawCallback == null)
			drawCallback = DrawSingleEnum;
		
		EditorHelper.DrawResizableList<SingleEnum_Data> ("Enum Values", ref enumValues, drawCallback, addCallback, delCallback, moveUpCallback, moveDownCallback);
	}

	private void DrawButton_Generate ()
	{
		if (!GUILayout.Button ("Generate"))
			return;
		
		GenerateEnum.Generate (this);
		EditorHelper.UnfocusControl ();
	}

	private void DrawButton_LoadExisting ()
	{
		if (!GUILayout.Button ("Load Existing"))
			return;

		GenerateEnum.LoadExisting (this);
		EditorHelper.UnfocusControl ();
	}

	private void DrawSingleEnum (int index)
	{
		enumValues[index].enumName = EditorGUILayout.TextField ("Name",  enumValues[index].enumName);
		enumValues[index].enumID   = EditorGUILayout.IntField  ("ID",    enumValues[index].enumID);
	}

	#endif

}
