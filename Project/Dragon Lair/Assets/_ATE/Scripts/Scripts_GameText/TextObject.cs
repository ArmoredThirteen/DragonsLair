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

			if (GUILayout.Button ("Update Randoms", GUILayout.Width (150)))
			{
				UpdateRandomCharacters (0.5f);
			}

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

		/// <summary>
		/// Re-rolls any character that with and
		/// index of theAlphabet.RandomIndex.
		/// It is a special character indicator.
		/// If updateChance can be <1 to make each
		/// character roll to see if they change.
		/// </summary>
		public void UpdateRandomCharacters (float updateChance = 1)
		{
			for (int i = 0; i < theString.textIndexes.Length; i++)
			{
				//	If it is not a random string, ignore it
				if (theString.textIndexes[i] != theAlphabet.RandomIndex)
					continue;
				//	Roll for updateChance on each character
				if (Random.Range (0, 1.0f) > updateChance)
					continue;

				SetLetter (renderers[i], theAlphabet.RandomIndex);
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
