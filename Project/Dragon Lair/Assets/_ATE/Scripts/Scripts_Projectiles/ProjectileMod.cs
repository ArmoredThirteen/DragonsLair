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
	public abstract class ProjectileMod : AteComponent
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
