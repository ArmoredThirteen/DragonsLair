namespace ScriptGeneration
{
	using UnityEngine;
	using System.Collections;
	using System.Collections.Generic;
	using System.IO;

	#if UNITY_EDITOR
	using UnityEditor;
	#endif


	/// <summary>
	/// Holds the data necessary to generate an enum script.
	/// Use DrawInspector() to display settings to designers.
	/// </summary>
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


		/// <summary>
		/// Hackily delays the automatic generation by one draw call.
		/// Used to avoid null when adding a value while using AutomaticGeneration.
		/// </summary>
		private bool _generateNextDraw = false;


		#if UNITY_EDITOR

		public enum DrawSettings
		{
			AutomaticGeneration,
			DrawPath,
			DrawType,
		}

		public void DrawInspector (params DrawSettings[] settings)
		{
			DrawInspector (null, false, null, null, null, null, settings);
		}

		public void DrawInspector (Callback<int> drawCallback, bool drawCallbackReplacesDefault = false, params DrawSettings[] settings)
		{
			DrawInspector (drawCallback, drawCallbackReplacesDefault, null, null, null, null, settings);
		}

		/// <summary>
		/// Draws the data for generating a single enum's entries.
		/// If drawCallback is null, the default draw method is used.
		/// If drawCallback is not null, the default draw method may or may not be called.
		/// If drawCallbackReplacesDefault is true, the default is not called,
		/// otherwise the default is called and then the given callback is.
		/// </summary>
		public void DrawInspector (
			Callback<int> drawCallback,   bool drawCallbackReplacesDefault,
			Callback<int> addCallback,    Callback<int> delCallback,
			Callback<int> moveUpCallback, Callback<int> moveDownCallback,
			params DrawSettings[] settings)
		{
			//	Turn settings into bools
			List<DrawSettings> listSettings = new List<DrawSettings> (settings);
			bool automaticGeneration = listSettings.Contains (DrawSettings.AutomaticGeneration);
			bool drawPath = listSettings.Contains (DrawSettings.DrawPath);
			bool drawType = listSettings.Contains (DrawSettings.DrawType);

			showData = EditorGUILayout.Toggle ("Show "+enumType+" Data", showData);
			if (!showData)
				return;

			if (drawPath)
				enumPath = EditorGUILayout.TextField ("Path", enumPath);
			if (drawType)
				enumType = EditorGUILayout.TextField ("Type", enumType);

			EditorGUILayout.BeginHorizontal ();

			if (!automaticGeneration)
			{
				if (Application.isPlaying)
					EditorGUILayout.LabelField ("Generation disabled while playing.");
				else
					DrawButton_Generate ();
			}

			DrawButton_LoadExisting ();
			EditorGUILayout.Space ();

			EditorGUILayout.EndHorizontal ();

			Callback<int> defaultDrawCallback = DrawEnumName;
			if (!automaticGeneration)
				defaultDrawCallback += DrawEnumID;

			Callback<int> actualDrawCallback = defaultDrawCallback;
			if (drawCallback != null)
			{
				if (drawCallbackReplacesDefault)
					actualDrawCallback = drawCallback;
				else
					actualDrawCallback += drawCallback;
			}

			if (_generateNextDraw)
			{
				GenerateEnum.Generate (this, true);
				_generateNextDraw = false;
			}

			bool modded = EditorHelper.DrawResizableList<SingleEnum_Data> ("Enum Values", ref enumValues, actualDrawCallback, addCallback, delCallback, moveUpCallback, moveDownCallback);
			if (modded && automaticGeneration)
			{
				_generateNextDraw = true;
			}
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

		private void DrawEnumName (int index)
		{
			enumValues[index].enumName = EditorGUILayout.TextField ("Name",  enumValues[index].enumName);
			//enumValues[index].enumID   = EditorGUILayout.IntField  ("ID",    enumValues[index].enumID);
		}

		private void DrawEnumID (int index)
		{
			//enumValues[index].enumName = EditorGUILayout.TextField ("Name",  enumValues[index].enumName);
			enumValues[index].enumID   = EditorGUILayout.IntField  ("ID",    enumValues[index].enumID);
		}

		#endif

	}
	
} //End namespace