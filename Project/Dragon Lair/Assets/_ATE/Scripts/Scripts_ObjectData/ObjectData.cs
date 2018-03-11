using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif


namespace Ate
{
	
	
	/// <summary>
	/// Description
	/// </summary>
	public class ObjectData : AteComponent_fpsControlled
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

		// Updates every game frame
		protected override void AteUpdate ()
		{
			
		}


		// Updates 24 times per second (may be different if UpdateBroadcaster gets rewritten)
		protected override void UpdateBaseFps ()
		{
			
		}

		// Updates once per framelength, which is one or more AteUpdateBaseFps
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
