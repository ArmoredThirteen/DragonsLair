﻿
namespace ScriptGeneration
{
	
	#if UNITY_EDITOR

	using UnityEngine;
	using UnityEditor;
	using System;
	using System.IO;
	using System.Linq;
	using System.Collections;
	using System.Collections.Generic;


	/// <summary>
	/// Editor-only methods for design-side creation of script enums.
	/// Methods are slow and shouldn't be run every draw frame.
	/// </summary>
	public static class GenerateEnum
	{
		#region Load Existing

		/// <summary>
		/// Using theData.enumType, finds the existing compiled enum values.
		/// Then loads theData.enumValues with the found existing values.
		/// If no enum type is found, exits without changing theData.enumValues.
		/// </summary>
		public static void LoadExisting (GenerateEnum_Data theData)
		{
			theData.enumValues.Clear ();

			//	Get Type by string
			System.Type theType = EditorHelper.GetEnumType (theData.enumType);
			if (theType == null)
				return;

			List<string> values = GenerateEnum.FindValuesByType (theData.enumType);

			for (int i = 0; i < values.Count; i++)
			{
				//	Turn enum value string into int value
				int enumID = System.Convert.ToInt32(System.Enum.Parse (theType, values[i]));
				//	Add to given data structure, theData.enumValues
				theData.enumValues.Add (new GenerateEnum_Data.SingleEnum_Data (values[i], enumID));
			}
		}

		private static List<string> FindValuesByType (string enumType)
		{
			//	Get Type by string
			Type theType = EditorHelper.GetEnumType (enumType);
			if (theType == null)
			{
				return new List<string> ();
			}

			return new List<string> (Enum.GetNames (theType));
		}

		#endregion


		#region Generate

		/// <summary>
		/// Given theData.enumValues, writes or rewrites an enum script.
		/// Script is located at theData.enumPath.
		/// After generating the script it recompiles (by calling AssetDatabse.Refresh()).
		/// </summary>
		public static void Generate (GenerateEnum_Data theData)
		{
			string fullPath = theData.enumPath + theData.enumType + ".cs";

			using (StreamWriter writer = new StreamWriter (fullPath))
			{
				WriteHeader (writer, theData);

				for (int i = 0; i < theData.enumValues.Count; i++)
				{
					WriteEnumValue (writer, theData.enumValues[i]);
				}

				WriteFooter (writer, theData);
			}

			Recompile ();
		}

		private static void WriteHeader (StreamWriter writer, GenerateEnum_Data theData)
		{
			writer.WriteLine ("//SCRIPT GENERATED, do not modify manually!");

			writer.WriteLine ("public enum " + theData.enumType);
			writer.WriteLine ("{");
		}

		private static void WriteFooter (StreamWriter writer, GenerateEnum_Data theData)
		{
			writer.WriteLine ("}");
		}

		private static void WriteEnumValue (StreamWriter writer, GenerateEnum_Data.SingleEnum_Data enumData)
		{
			string enumLine = "\t" + enumData.enumName + " = " + enumData.enumID + ",";
			writer.WriteLine (enumLine);
		}

		private static void Recompile ()
		{
			AssetDatabase.Refresh ();
		}

		#endregion

	}

	#endif

}//	End namespace