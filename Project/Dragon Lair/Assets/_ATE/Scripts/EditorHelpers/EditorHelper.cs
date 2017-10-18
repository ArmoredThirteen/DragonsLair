
#if UNITY_EDITOR


using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;


namespace Ate.EditorHelpers
{


	public static class EditorHelper
	{
		public static Color DefaultGUIColor
		{
			get {return new Color (1, 1, 1, 1);}
		}
		public static Color AddButtonColor
		{
			get {return new Color (0.65f, 1.0f, 0.65f);}
		}

		public static Color DelButtonColor
		{
			get {return new Color (1.0f, 0.65f, 0.65f);}
		}


		private static string UpArrow
		{
			get {return '\u25B2'.ToString ();}
		}

		private static string DownArrow
		{
			get {return '\u25BC'.ToString ();}
		}


		/// <summary>
		/// Unfocuses the control by calling GUI.FocusControl ("");
		/// </summary>
		public static void UnfocusControl ()
		{
			GUI.FocusControl ("");
		}

		/// <summary>
		/// If GUI.changed is true, sets the target as dirty.
		/// If target is a scene object, also marks the scene as dirty.
		/// </summary>
		public static void SetDirtyIfChanged (MonoBehaviour target)
		{
			//	Can happen if target destroys itself because of inspector scripts
			if (target == null)
				return;
			if (!GUI.changed)
				return;
			
			EditorUtility.SetDirty (target);

			//	If it is a scene object and the application isn't playing
			if (!string.IsNullOrEmpty (target.gameObject.scene.name) && !Application.isPlaying)
			{
				EditorApplication.MarkSceneDirty ();
			}
		}


		//	Shamelessly stolen from user 'dbc':
		//	http://www.stackoverflow.com/questions/25404237/how-to-get-enum-type-by-specifying-its-name-in-string
		/// <summary>
		/// Inefficient, don't use constantly!
		/// Using enumName, finds an existing Type.
		/// Returns null if none are found.
		/// </summary>
		public static System.Type GetEnumType (string enumName)
		{
			//	? Finds all the enums in the assembly ?
			foreach (System.Reflection.Assembly assembly in System.AppDomain.CurrentDomain.GetAssemblies ())
			{
				System.Type type = assembly.GetType (enumName);

				if (type == null)
					continue;
				if (type.IsEnum)
					return type;
			}

			return null;
		}


		#region Lists

		/*public static void ResizeList<T> (int newSize, ref List<T> theList, T defaultEntry = default(T))
		{
			int curSize = theList.Count;

			//	Requesting smaller size, delete entries
			if (newSize < curSize)
			{
				for (int i = curSize-1; i >= newSize; i--)
				{
					theList.RemoveAt (i);
				}
			}
			//	Requesting larger size, add entries
			else if (newSize > curSize)
			{
				for (int i = 0; i < newSize-curSize; i++)
				{
					theList.Add (defaultEntry);
				}
			}
		}*/


		/*public static bool DrawPairedResizableList<T,Y> (
			string title,
			ref List<T> parentList,
			ref List<Y> pairedList,
			Callback<int> DrawEntryCallback)
		{
			return DrawPairedResizableList<T,Y> (title, ref parentList, ref pairedList, DrawEntryCallback, null, null, null, null);
		}

		//TODO: Make it not a direct copy/paste and something more easily extendible
		//TODO: Make it not a direct copy/paste and something more easily extendible
		//TODO: Make it not a direct copy/paste and something more easily extendible
		public static bool DrawPairedResizableList<T,Y> (
			string title,
			ref List<T> parentList,
			ref List<Y> pairedList,
			Callback<int> DrawEntryCallback,
			Callback<int> AddCallback,
			Callback<int> DelCallback,
			Callback<int> MoveUpCallback,
			Callback<int> MoveDownCallback,
			bool horizontalButtons = true
		)
		{
			EditorGUILayout.Space ();
			//	If list changed from 0 to 1, don't draw list stuff until next time
			bool modButtonPressed = DrawListHeader (title, ref theList, AddCallback);
			if (modButtonPressed)
				return true;
			EditorGUILayout.Space ();

			EditorGUI.indentLevel++;
			EditorGUILayout.BeginVertical ();
			for (int i = 0; i < theList.Count; i++)
			{
				//	Begin 'indent' spacing for buttons
				EditorGUILayout.BeginHorizontal ();
				GUILayout.Space (20*EditorGUI.indentLevel);

				if (!horizontalButtons)
				{
					EditorGUI.indentLevel--;
					EditorGUILayout.BeginHorizontal ();
				}

				//	Draw vertical or horizontal buttons
				if (!horizontalButtons)
					EditorGUILayout.BeginVertical ();
				modButtonPressed = DrawListEntryButtons<T>
					(ref theList, i, AddCallback, DelCallback, MoveUpCallback, MoveDownCallback, horizontalButtons);
				if (!horizontalButtons)
					EditorGUILayout.EndVertical ();

				//	End 'indent' spacing
				EditorGUILayout.EndHorizontal ();

				//	If no buttons were pressed, draw the entry data
				if (!modButtonPressed)
					DrawListEntry<T> (ref theList, i , DrawEntryCallback);

				if (!horizontalButtons)
				{
					EditorGUI.indentLevel++;
					EditorGUILayout.EndHorizontal ();
				}

				EditorGUILayout.Space ();

				if (modButtonPressed)
					break;
			}
			EditorGUILayout.EndVertical ();
			EditorGUI.indentLevel--;

			GUI.backgroundColor = DefaultGUIColor;

			//	To prevent previously selected, modified entry data appearing in the wrong entry
			if (modButtonPressed)
				UnfocusControl ();

			return modButtonPressed;
		}*/


		/// <summary>
		/// Draws a list of objects with the option to resize the list.
		/// Callers must specify how entries are drawn using the drawEntryCallback.
		/// The callback passes the entry index.
		/// </summary>
		public static bool DrawResizableList<T> (
			string title,
			ref List<T> theList,
			Callback<int> DrawEntryCallback,
			bool horizontalButtons = true)
		{
			return DrawResizableList<T> (title, ref theList, DrawEntryCallback, null, null, null, null, horizontalButtons);
		}

		/// <summary>
		/// Draws a list of objects with the option to resize the list.
		/// Callers must specify how entries are drawn using the drawEntryCallback.
		/// The callbacks pass the entry index.
		/// 
		/// Returns true if any of the add/del/swap buttons are pressed.
		/// 
		/// If horizontalButtons is true, the buttons will be on the top horizontally.
		/// If it is false, the buttons will be to the left vertically, with data to the right.
		/// 
		/// AddCallback performs default adding functionality then calls AddCallback,
		/// with an index of where the new entry was added to.
		/// Del/SwapCallbacks call their callbacks, with an index of which entry will be modified,
		/// then performs default functionality.
		/// Default add functionality:  theList.Insert (index+1, default(T));
		/// Default del functionality:  theList.RemoveAt (index);
		/// Default swap functionality: theList.Swap (index, index +/- 1);
		/// </summary>
		public static bool DrawResizableList<T> (
			string title,
			ref List<T> theList,
			Callback<int> DrawEntryCallback,
			Callback<int> AddCallback,
			Callback<int> DelCallback,
			Callback<int> MoveUpCallback,
			Callback<int> MoveDownCallback,
			bool horizontalButtons = true)
		{
			EditorGUILayout.Space ();

			//	If list changed from 0 to 1, don't draw list stuff until next time
			bool modButtonPressed = DrawListHeader (title, ref theList, AddCallback);
			if (modButtonPressed)
				return true;
			EditorGUILayout.Space ();

			EditorGUI.indentLevel++;
			EditorGUILayout.BeginVertical ();
			for (int i = 0; i < theList.Count; i++)
			{
				//	Begin 'indent' spacing for buttons
				EditorGUILayout.BeginHorizontal ();
				GUILayout.Space (20*EditorGUI.indentLevel);

				if (!horizontalButtons)
				{
					EditorGUI.indentLevel--;
					EditorGUILayout.BeginHorizontal ();
				}
				
				//	Draw vertical or horizontal buttons
				if (!horizontalButtons)
					EditorGUILayout.BeginVertical ();
				modButtonPressed = DrawListEntryButtons<T>
					(ref theList, i, AddCallback, DelCallback, MoveUpCallback, MoveDownCallback, horizontalButtons);
				if (!horizontalButtons)
					EditorGUILayout.EndVertical ();

				//	End 'indent' spacing
				EditorGUILayout.EndHorizontal ();

				//	If no buttons were pressed, draw the entry data
				if (!modButtonPressed)
					DrawListEntry<T> (ref theList, i , DrawEntryCallback);

				if (!horizontalButtons)
				{
					EditorGUI.indentLevel++;
					EditorGUILayout.EndHorizontal ();
				}

				EditorGUILayout.Space ();

				if (modButtonPressed)
					break;
			}
			EditorGUILayout.EndVertical ();
			EditorGUI.indentLevel--;

			GUI.backgroundColor = DefaultGUIColor;

			//	To prevent previously selected, modified entry data appearing in the wrong entry
			if (modButtonPressed)
				UnfocusControl ();
			
			return modButtonPressed;
		}


		/// <summary>
		/// Draws the list header. Returns true if list was 0 and an entry was added.
		/// </summary>
		private static bool DrawListHeader<T> (string title, ref List<T> theList, Callback<int> AddCallback)
		{
			bool pressed = false;
			EditorGUILayout.BeginVertical ();

			GUIStyle myStyle = new GUIStyle ();
			myStyle.fontSize = 15;

			EditorGUILayout.LabelField (title, myStyle);

			if (theList.Count == 0)
			{
				GUI.color = AddButtonColor;
				if (GUILayout.Button ("Add", GUILayout.Width (70)))
				{
					pressed = true;

					theList.Add (default(T));

					if (AddCallback != null)
						AddCallback (0);
				}
				
				GUI.color = DefaultGUIColor;
			}

			EditorGUILayout.EndVertical ();
			return pressed;
		}

		/// <summary>
		/// Draws an entry in a list. Returns true if an entry was added, removed, or swapped.
		/// </summary>
		private static void DrawListEntry<T> (
			ref List<T> theList, int index,
			Callback<int> DrawEntryCallback)
		{
			EditorGUILayout.BeginVertical ();
			DrawEntryCallback (index);
			EditorGUILayout.EndVertical ();
		}

		private static bool DrawListEntryButtons<T> (
			ref List<T> theList, int index,
			Callback<int> AddCallback,
			Callback<int> DelCallback,
			Callback<int> MoveUpCallback,
			Callback<int> MoveDownCallback,
			bool horizontalButtons)
		{
			//	Add button
			if (ListAddButton<T> (ref theList, index, AddCallback))
				return true;

			if (horizontalButtons)
				GUILayout.Space (10);

			//	Delete button
			if (ListDelButton<T> (ref theList, index, DelCallback))
				return true;

			if (horizontalButtons)
				GUILayout.Space (10);

			//	Move up or down
			EditorGUILayout.LabelField ("Move: ", GUILayout.Width (55+(EditorGUI.indentLevel*5)));

			if (!horizontalButtons)
				EditorGUILayout.BeginHorizontal ();
			
			//	Move up
			if (ListMoveUpButton<T> (ref theList, index, MoveUpCallback))
				return true;

			//	Move down
			if (ListMoveDownButton<T> (ref theList, index, MoveDownCallback))
				return true;

			if (!horizontalButtons)
				EditorGUILayout.EndHorizontal ();

			return false;
		}

		private static bool ListAddButton<T> (ref List<T> theList, int index, Callback<int> AddCallback)
		{
			bool pressed = false;

			GUI.color = AddButtonColor;
			if (GUILayout.Button ("Add", GUILayout.Width (45)))
			{
				pressed = true;

				theList.Insert (index+1, default(T));

				if (AddCallback != null)
					AddCallback (index+1);
			}

			GUI.color = DefaultGUIColor;
			return pressed;
		}

		private static bool ListDelButton<T> (ref List<T> theList, int index, Callback<int> DelCallback)
		{
			bool pressed = false;

			GUI.color = DelButtonColor;
			if (GUILayout.Button ("Del", GUILayout.Width (35)))
			{
				pressed = true;

				if (DelCallback != null)
					DelCallback (index);

				theList.RemoveAt (index);
			}

			GUI.color = DefaultGUIColor;
			return pressed;
		}

		private static bool ListMoveUpButton<T> (ref List<T> theList, int index, Callback<int> MoveUpCallback)
		{
			bool pressed = false;

			if (GUILayout.Button (UpArrow, GUILayout.Width (20)))
			{
				pressed = true;

				if (MoveUpCallback != null)
					MoveUpCallback (index);

				theList.Swap (index, index-1);
			}

			return pressed;
		}

		private static bool ListMoveDownButton<T> (ref List<T> theList, int index, Callback<int> MoveDownCallback)
		{
			bool pressed = false;

			if (GUILayout.Button (DownArrow, GUILayout.Width (20)))
			{
				pressed = true;

				if (MoveDownCallback != null)
					MoveDownCallback (index);

				theList.Swap (index, index+1);
			}

			return pressed;
		}

		#endregion

	}//End Class


}//End Namespace


//endif UNITY_EDITOR
#endif
