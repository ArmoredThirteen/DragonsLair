using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif


namespace Ate.Collision
{
	

	/// <summary>
	/// A 2D circle collider.
	/// </summary>
	public class Collider_Circle : Collider
	{

		#region Public Variables

		public bool flattenAxisIn3D = false;
		public float radius = 1;

		#endregion


		#region Private Variables

		#endregion


		#if UNITY_EDITOR

		public override void DrawInspector ()
		{
			base.DrawInspector ();

			flattenAxisIn3D = EditorGUILayout.Toggle ("Flatten Axis in 3D", flattenAxisIn3D);
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
