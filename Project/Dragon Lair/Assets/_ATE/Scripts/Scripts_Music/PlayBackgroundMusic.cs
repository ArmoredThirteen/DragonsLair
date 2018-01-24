using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif


namespace Ate
{
	
	
	/// <summary>
	/// This singleton-type object should be in only the starting scene.
	/// For playing repeating background music only.
	/// Needs to eventually be combined/replaced with
	/// a full sound and music playing system.
	/// </summary>
	public class PlayBackgroundMusic : AteComponent
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
			/*PlayBackgroundMusic[] theseObjects = Object.FindObjectsOfType<PlayBackgroundMusic> ();

			if (theseObjects.Length > 1)
			{
				Destroy (this);
				return;
			}

			DontDestroyOnLoad (this.gameObject);*/
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
