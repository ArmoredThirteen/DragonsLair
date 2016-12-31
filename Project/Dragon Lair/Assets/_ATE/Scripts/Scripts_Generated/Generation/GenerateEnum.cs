
#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;


public static class GenerateEnum
{
	#region Load Existing

	public static void LoadExisting (GenerateEnum_Data theData)
	{
		theData.enumValues.Clear ();

		System.Type theType = EditorHelper.GetEnumType (theData.enumType);
		if (theType == null)
			return;

		List<string> values = GenerateEnum.FindValuesByType (theData.enumType);

		for (int i = 0; i < values.Count; i++)
		{
			int enumID = System.Convert.ToInt32(System.Enum.Parse (theType, values[i]));
			theData.enumValues.Add (new GenerateEnum_Data.SingleEnum_Data (values[i], enumID));
		}
	}

	private static List<string> FindValuesByType (string enumType)
	{
		Type theType = EditorHelper.GetEnumType (enumType);
		if (theType == null)
		{
			return new List<string> ();
		}

		return new List<string> (Enum.GetNames (theType));
	}

	#endregion


	#region Generate

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

		AssetDatabase.Refresh ();
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

	#endregion

}

#endif
