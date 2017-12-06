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

		public char   defaultCharacter = 'Ŋ';
		public Sprite defaultSprite    = null;

		public List<char>   characters = new List<char> ();
		public List<Sprite> sprites    = new List<Sprite> ();


		#if UNITY_EDITOR

		public void DrawInspector ()
		{
			if (GUILayout.Button ("Build Default Alphabet"))
			{
				BuildDefaultAlphabet ();
			}

			EditorGUILayout.Space ();

			defaultCharacter = EditorGUILayout.TextField    ("Default Character", defaultCharacter.ToString ()).ToCharArray(0,1)[0];
			defaultSprite    = EditorGUILayout.ObjectField ("Sprite", defaultSprite, typeof(Sprite), false) as Sprite;

			EditorGUILayout.Space ();
			EditorGUILayout.Space ();

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

			for (int i = 32; i < 128; i++)
			{
				characters.Add ((char)i);
				sprites.Add (null);
			}
		}


		public List<int> GetStringIndexes (string theString)
		{
			List<int>  result   = new List<int>  ();
			List<char> charList = new List<char> (theString.ToCharArray ());

			for (int i = 0; i < charList.Count; i++)
			{
				int characterIndex = GetIndexByChar (charList[i]);

				if (characterIndex == -1)
					continue;

				result.Add (characterIndex);
			}

			return result;
		}

		#endif


		/// <summary>
		/// Finds the index of a given character.
		/// If no character is found, returns -1.
		/// </summary>
		public int GetIndexByChar (char theChar)
		{
			for (int i = 0; i < characters.Count; i++)
			{
				if (characters[i].Equals (theChar))
					return i;
			}

			return -1;
		}

		/// <summary>
		/// Given an index, returns a sprite.
		/// If index is -1, returns default sprite.
		/// </summary>
		public Sprite GetSpriteByIndex (int index)
		{
			if (index == -1)
				return defaultSprite;

			return sprites[index];
		}

		/// <summary>
		/// Given an index, returns a character.
		/// If index is -1, returns default character.
		/// </summary>
		public char GetCharByIndex (int index)
		{
			if (index == -1)
				return defaultCharacter;

			return characters[index];
		}

	}//End Class
	
	
}//End Namespace
