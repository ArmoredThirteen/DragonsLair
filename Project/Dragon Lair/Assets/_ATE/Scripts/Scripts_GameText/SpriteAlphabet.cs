using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
using Ate.EditorHelpers;
#endif


namespace Ate.GameText
{
	
	
	/// <summary>
	/// For holding info about each letter in an alphabet.
	/// Stores each letter's relation to a sprite.
	/// </summary>
	[System.Serializable]
	[CreateAssetMenu (fileName = "SpriteAlphabet_Default", menuName = "ATE/Fonts/SpriteAlphabet", order = 1)]
	public class SpriteAlphabet : ScriptableObject
	{

		public List<char>   characters = new List<char> ();
		public List<Sprite> sprites    = new List<Sprite> ();


		#if UNITY_EDITOR

		public void DrawInspector ()
		{
			if (GUILayout.Button ("Build Default Alphabet"))
			{
				BuildDefaultAlphabet ();
			}

			EditorHelper.DrawResizableList<char> ("Characters", ref characters,
				OnDrawEntry, OnAddEntry, OnDelEntry, OnMoveUpEntry, OnMoveDownEntry);

			EditorHelper.SetObjectDirtyIfChanged (this);
		}

		private void OnDrawEntry (int index)
		{
			characters[index] = EditorGUILayout.TextField   ("Character", characters[index].ToString ()).ToCharArray(0,1)[0];
			sprites[index]    = EditorGUILayout.ObjectField ("Sprite",    sprites[index], typeof(Sprite), false) as Sprite;
		}

		private void OnAddEntry (int index)
		{
			sprites.Insert (index, default(Sprite));
		}

		private void OnDelEntry (int index)
		{
			sprites.RemoveAt (index);
		}

		private void OnMoveUpEntry (int index)
		{
			sprites.Swap (index, index-1);
		}

		private void OnMoveDownEntry (int index)
		{
			sprites.Swap (index, index+1);
		}


		private void BuildDefaultAlphabet ()
		{
			characters.Clear ();
			sprites.Clear ();

			characters.Add (' ');

			characters.Add ('A');
			characters.Add ('B');
			characters.Add ('C');
			characters.Add ('D');
			characters.Add ('E');
			characters.Add ('F');
			characters.Add ('G');
			characters.Add ('H');
			characters.Add ('I');
			characters.Add ('J');
			characters.Add ('K');
			characters.Add ('L');
			characters.Add ('M');
			characters.Add ('N');
			characters.Add ('O');
			characters.Add ('P');
			characters.Add ('Q');
			characters.Add ('R');
			characters.Add ('S');
			characters.Add ('T');
			characters.Add ('U');
			characters.Add ('V');
			characters.Add ('W');
			characters.Add ('X');
			characters.Add ('Y');
			characters.Add ('Z');

			characters.Add ('.');
			characters.Add (',');
			characters.Add ('!');
			characters.Add ('?');

			for (int i = 0; i < characters.Count; i++)
			{
				sprites.Add (null);
			}
		}


		public List<int> GetStringIndexes (string theString, bool caseSensitive = false)
		{
			List<int>  result   = new List<int>  ();
			List<char> charList = new List<char> (theString.ToCharArray ());

			for (int i = 0; i < charList.Count; i++)
			{
				int characterIndex = GetCharacterIndex (charList[i], caseSensitive);

				if (characterIndex == -1)
					continue;

				result.Add (characterIndex);
			}

			return result;
		}

		/// <summary>
		/// Finds the index of a given character.
		/// If no character is found, returns -1.
		/// </summary>
		private int GetCharacterIndex (char theChar, bool caseSensitive = false)
		{
			for (int i = 0; i < characters.Count; i++)
			{
				if (caseSensitive)
				{
					if (characters[i].Equals (theChar))
						return i;
				}
				else
				{
					if (char.ToUpper (characters[i]).Equals (char.ToUpper (theChar)))
						return i;
				}
			}

			return -1;
		}

		#endif

	}//End Class
	
	
}//End Namespace
