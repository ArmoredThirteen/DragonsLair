using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif


namespace Ate.ScenePersistance
{
	
	
	/// <summary>
	/// Description
	/// </summary>
	public class ScenePersistent_Audio : AteComponent_fpsControlled
	{

		#region Singleton
		public static ScenePersistent_Audio AudioInstance
		{
			get; private set;
		}
		#endregion


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
			base.AteAwake ();

			if (AudioInstance == null)
			{
				AudioInstance = this;
				DontDestroyOnLoad (gameObject);
			}
			else if (AudioInstance != this)
			{
				DestroyImmediate (gameObject);
				return;
			}
		}

		// Updates every game frame
		protected override void AteUpdate ()
		{
			
		}


		// Updates 24 times per second
		protected override void FpsUpdate24 ()
		{
			
		}

		// Updates once per framelength, which is one or more FpsUpdate## calls
		protected override void UpdateFrameLength ()
		{
			
		}

		#endregion


		#region Public Methods

		#endregion


		#region Private Methods

		#endregion

	}//End Class
	
	
}//End Namespace
