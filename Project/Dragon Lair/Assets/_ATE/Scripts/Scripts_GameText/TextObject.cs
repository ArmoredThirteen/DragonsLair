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
	public class TextObject : AteComponent
	{
		
		#region Public Variables

		public SpriteAlphabet theAlphabet = null;

		public IntStoredString theString = new IntStoredString ();

		public List<SpriteRenderer> renderers = new List<SpriteRenderer> ();

		#endregion


		#region Private Variables

		#if UNITY_EDITOR
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

			theString.maxTextSize = renderers.Count;

			bool stringChanged = theString.DrawInspector ("String", theAlphabet);
			if (stringChanged)
				UpdateText ();

			_displayRenderers = EditorGUILayout.Toggle ("Display Renderers", _displayRenderers);
			if (_displayRenderers)
				OnDrawRenderers ();
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

		public void UpdateText ()
		{
			for (int i = 0; i < renderers.Count; i++)
			{
				if (i < theString.textIndexes.Length)
					SetLetter (renderers[i], theString.textIndexes[i]);
				else
					renderers[i].sprite = null;
			}
		}

		#endregion


		#region Private Methods

		private void SetLetter (SpriteRenderer letterRenderer, int characterIndex)
		{
			letterRenderer.sprite = theAlphabet.GetSpriteByIndex (characterIndex);
		}

		#endregion

	}//End Class
	
	
}//End Namespace
