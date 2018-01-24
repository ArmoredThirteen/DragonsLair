using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif


namespace Ate
{
	
	
	/// <summary>
	/// Meant to keep a GameObject alive during scene changes.
	/// </summary>
	public class KeepOnLoad : AteComponent
	{
		
		#region Public Variables

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

		protected override void AteAwake ()
		{
			KeepOnLoad[] theseObjects = Object.FindObjectsOfType<KeepOnLoad> ();

			if (theseObjects.Length > 1)
			{
				Destroy (this.gameObject);
				return;
			}

			DontDestroyOnLoad (this.gameObject);
		}


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
