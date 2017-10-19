using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif


namespace Ate.Prototypes
{
	
	
	/// <summary>
	/// Controls how a ship moves when told to move to a specified location.
	/// Includes some logic for animation changes.
	/// </summary>
	public class ShipMovement : AteComponent
	{
		
		#region Public Variables

		public float moveSpeed = 10;

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
