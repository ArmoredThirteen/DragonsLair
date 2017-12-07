using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
using Ate.EditorHelpers;
#endif


namespace Ate.GameText
{
	
	
	/// <summary>
	/// Description
	/// </summary>
	[System.Serializable]
	public class IntStoredString
	{
		
		#region Public Variables

		public int maxTextSize = 24;
		public int[] textIndexes = new int[]{};

		#endregion


		#region Private Variables

		#endregion


		#if UNITY_EDITOR

		/// <summary>
		/// Draws a TextField to textIndexes inspector.
		/// Returns true if the text field has changed.
		/// </summary>
		public bool DrawInspector (string title, SpriteAlphabet alphabet)
		{
			string oldString = GetStringFromIndexes (alphabet);
			string newString = EditorGUILayout.TextField (title, oldString);

			//	Nothing changed
			if (oldString.Equals (newString))
				return false;

			SetIndexesFromString (alphabet, newString);

			return true;
		}

		private string GetStringFromIndexes (SpriteAlphabet alphabet)
		{
			string theString = "";

			if (textIndexes.Length == 0)
				return theString;

			for (int i = 0; i < textIndexes.Length && i < maxTextSize; i++)
			{
				theString = theString + alphabet.GetCharByIndex_NoRng (textIndexes[i]);
			}

			return theString;
		}

		private void SetIndexesFromString (SpriteAlphabet alphabet, string newString)
		{
			char[] charArray = newString.ToCharArray ();
			textIndexes = new int[Mathf.Min (maxTextSize, charArray.Length)];

			for (int i = 0; i < textIndexes.Length; i++)
			{
				textIndexes[i] = alphabet.GetIndexByChar (charArray[i]);
			}
		}

		#endif


		#region Public Methods

		#endregion


		#region Private Methods

		#endregion

	}//End Class
	
	
}//End Namespace
