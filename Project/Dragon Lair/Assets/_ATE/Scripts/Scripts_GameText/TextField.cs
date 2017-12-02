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
	/// A text field composed of several SpriteRenderer letters.
	/// </summary>
	public class TextField : AteComponent
	{
		
		#region Public Variables

		public SpriteAlphabet theAlphabet = null;

		public List<SpriteRenderer> renderers = new List<SpriteRenderer> ();

		#endregion


		#region Private Variables

		#if UNITY_EDITOR
		private string _testString = "StR tSt!";

		private bool _displayRenderers = false;
		#endif

		#endregion


		#if UNITY_EDITOR

		public override void DrawInspector ()
		{
			base.DrawInspector ();

			theAlphabet = EditorGUILayout.ObjectField
				("The Alphabet", theAlphabet, typeof(SpriteAlphabet), false)
				as SpriteAlphabet;

			OnDrawSetTestText ();

			_displayRenderers = EditorGUILayout.Toggle ("Display Renderers", _displayRenderers);
			if (_displayRenderers)
				OnDrawRenderers ();
		}

		private void OnDrawSetTestText ()
		{
			EditorGUILayout.BeginHorizontal ();

			if (GUILayout.Button ("Set String", GUILayout.Width (75)))
			{
				ChangeText (theAlphabet.GetStringIndexes (_testString));
			}

			_testString = EditorGUILayout.TextField (_testString, GUILayout.Width (150));

			EditorGUILayout.EndHorizontal ();
		}

		private void OnDrawRenderers ()
		{
			if (GUILayout.Button ("Add Renderers From Children"))
				OnFillWithChildren ();

			EditorHelper.DrawResizableList<SpriteRenderer> ("Sprite Renderers", ref renderers, OnDrawRenderer);
		}

		private void OnFillWithChildren ()
		{
			renderers = new List<SpriteRenderer> (GetComponentsInChildren<SpriteRenderer> ());
		}

		private void OnDrawRenderer (int index)
		{
			renderers[index] = EditorGUILayout.ObjectField
				("Renderer", renderers[index], typeof(SpriteRenderer), true)
				as SpriteRenderer;
		}

		#endif


		#region AteComponent

		protected override void AteUpdate ()
		{
			
		}

		#endregion


		#region Public Methods

		/// <summary>
		/// Changes the text to match the given character indexes.
		/// The indexes relate to SpriteAlphabet's character list.
		/// These should not be found live but rather converted
		/// and stored as int indexes during editor design work.
		/// In editor you can use SpriteAlphabet.GetStringIndexes().
		/// </summary>
		public void ChangeText (List<int> characterIndexes)
		{
			for (int i = 0; i < renderers.Count; i++)
			{
				if (i < characterIndexes.Count)
					SetLetter (renderers[i], characterIndexes[i]);
				else
					renderers[i].sprite = null;
			}
		}

		#endregion


		#region Private Methods

		private void SetLetter (SpriteRenderer letterRenderer, int characterIndex)
		{
			letterRenderer.sprite = theAlphabet.sprites[characterIndex];
		}

		#endregion

	}//End Class
	
	
}//End Namespace
