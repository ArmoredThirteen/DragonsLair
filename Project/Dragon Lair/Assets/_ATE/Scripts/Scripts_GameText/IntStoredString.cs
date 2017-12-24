using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
using Ate.EditorHelpers;
#endif


namespace Ate.GameText
{
	
	
	/// <summary>
	/// A way to store strings as an integer array to be used more efficiently during gameplay.
	/// The integer array is used as a list of indexes to a SpriteAlphabet. Designers set a string,
	/// the string is translated to an integer array, and then during gameplay these integer
	/// arrays are used instead of strings and characters.
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

		#endif

		/// <summary>
		/// For each character in the string, its index in the given alphabet is found.
		/// Then assigns the indexes to textIndexes.
		/// The string manipulation and comparing going on internally is inefficient.
		/// This should only be used in-game when there is no other choice.
		/// Usually during design-time this translates designer strings to int arrays
		/// to be used more efficiently during live gameplay.
		/// </summary>
		public void SetIndexesFromString (SpriteAlphabet alphabet, string newString)
		{
			char[] charArray = newString.ToCharArray ();
			// Initialize to be either the max size or shortened to the string's size
			//	Chooses whichever is smaller to guarantee no overflow
			textIndexes = new int[Mathf.Min (maxTextSize, charArray.Length)];

			for (int i = 0; i < textIndexes.Length; i++)
			{
				textIndexes[i] = alphabet.GetIndexByChar (charArray[i]);
			}
		}

		//#endif


		#region Public Methods

		#endregion


		#region Private Methods

		#endregion

	}//End Class
	
	
}//End Namespace
