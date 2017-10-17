using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif


namespace Ate.Collision
{
	

	/// <summary>
	/// A 3D sphere collider.
	/// </summary>
	public class Collider_Sphere : Collider
	{

		#region Public Variables

		public float radius = 1;

		#endregion


		#region Private Variables

		#endregion


		#if UNITY_EDITOR

		public override void DrawInspector ()
		{
			base.DrawInspector ();

			//zeroUpAxis = EditorGUILayout.Toggle ("Zero Up Axis", zeroUpAxis);
			radius = EditorGUILayout.FloatField ("Radius", radius);
		}

		#endif


		#region AteComponent

		protected override void AteUpdate ()
		{
			
		}

		#endregion


		#region Public Methods

		/// <summary>
		/// Returns the world position with any modifications, such as offset.
		/// </summary>
		public override Vector3 GetPosition ()
		{
			return transform.position;
		}

		#endregion


		#region Private Methods

		#endregion

	}//End Class


}//End Namespace
