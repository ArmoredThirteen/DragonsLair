using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif


namespace Ate.GameText
{
	
	
	/// <summary>
	/// A single letter as part of a text box.
	/// </summary>
	public class TextLetter : AteComponent
	{
		
		#region Public Variables

		public SpriteRenderer renderer       = null;
		public Sprite         startingSprite = null;

		#endregion


		#region Private Variables

		#endregion


		#if UNITY_EDITOR

		public override void DrawInspector ()
		{
			base.DrawInspector ();
		}

		#endif


		#region AteComponent

		protected override void AteUpdate ()
		{
			
		}

		#endregion


		#region Public Methods

		#endregion


		#region Private Methods

		#endregion

	}//End Class
	
	
}//End Namespace
