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

		#region Public Variables

		public char   defaultCharacter = 'Ŋ';
		public Sprite defaultSprite    = null;

		/// <summary>
		/// This is tied to special 'pick any character' functionality.
		/// Basically when told to update random characters, anything
		/// that was this character can be rerolled to anything in the library.
		/// </summary>
		public char   randomCharacter  = '¤';

		public List<char>   characters = new List<char> ();
		public List<Sprite> sprites    = new List<Sprite> ();

		#endregion


		#region Private Variables

		private int _asciiOffsetVal = 32;

		private int _index_Default = -1;
		private int _index_Random  = -2;

		#endregion


		#region Properties

		public int DefaultIndex
		{
			get {return _index_Default;}
		}

		public int RandomIndex
		{
			get {return _index_Random;}
		}

		#endregion


		#if UNITY_EDITOR

		public void DrawInspector ()
		{
			if (GUILayout.Button ("Build Default Alphabet"))
			{
				BuildDefaultAlphabet ();
			}

			EditorGUILayout.Space ();

			defaultCharacter = EditorGUILayout.TextField   ("Default Character", defaultCharacter.ToString ()).ToCharArray(0,1)[0];
			defaultSprite    = EditorGUILayout.ObjectField ("Sprite", defaultSprite, typeof(Sprite), false) as Sprite;

			randomCharacter  = EditorGUILayout.TextField   ("Random Character", randomCharacter.ToString ()).ToCharArray(0,1)[0];

			EditorGUILayout.Space ();
			EditorGUILayout.Space ();

			bool drawList = true;
			EditorHelper.DrawResizableList<char> ("Characters", ref drawList, ref characters,
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

			for (int i = _asciiOffsetVal; i < 128; i++)
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

				if (characterIndex == _index_Default)
					continue;

				result.Add (characterIndex);
			}

			return result;
		}

		#endif


		/// <summary>
		/// Finds the index of a given character.
		/// If no character is found, returns DefaultIndex.
		/// If it matches randomCharacter, returns RandomIndex.
		/// </summary>
		public int GetIndexByChar (char theChar)
		{
			if (theChar.Equals (randomCharacter))
				return _index_Random;
			
			for (int i = 0; i < characters.Count; i++)
			{
				if (characters[i].Equals (theChar))
					return i + _asciiOffsetVal;
			}

			return _index_Default;
		}

		/// <summary>
		/// Given an index, returns a sprite.
		/// If index is DefaultIndex, returns default sprite.
		/// If index is RandomIndex, returns a random sprite.
		/// </summary>
		public Sprite GetSpriteByIndex (int index)
		{
			int asciiOffsetIndex = index - _asciiOffsetVal;

			if (index == _index_Default)
				return defaultSprite;

			//	Random character
			if (index == _index_Random)
			{
				return sprites[Random.Range (0, sprites.Count)];
			}

			return sprites[asciiOffsetIndex];
		}

		/// <summary>
		/// Given an index, returns a character.
		/// If index is DefaultIndex, returns default character.
		/// If index is RandomIndex, returns the randomCharacter indicator.
		/// </summary>
		public char GetCharByIndex_NoRng (int index)
		{
			int asciiOffsetIndex = index - _asciiOffsetVal;

			if (index ==_index_Default)
				return defaultCharacter;

			//	Random character indicator
			if (index == _index_Random)
			{
				return randomCharacter;
			}

			return characters[asciiOffsetIndex];
		}


		/// <summary>
		/// Given an index, returns a character.
		/// If index is DefaultIndex, returns default character.
		/// If index is RandomIndex, returns a random character.
		/// </summary>
		public char GetCharByIndex_Rng (int index)
		{
			int asciiOffsetIndex = index - _asciiOffsetVal;

			if (index == _index_Default)
				return defaultCharacter;

			//	Random character
			if (index == _index_Random)
			{
				return characters[Random.Range (0, characters.Count)];
			}

			return characters[asciiOffsetIndex];
		}

	}//End Class
	
	
}//End Namespace
