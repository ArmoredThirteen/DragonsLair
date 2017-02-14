using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif


namespace CollisionSystem
{
	

	/// <summary>
	/// A 2D circle collider.
	/// </summary>
	public class Collider_Circle : Collider
	{

		#region Public Variables

		//public bool zeroUpAxis = true;
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


		#region AteGameObject

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

		/// <summary>
		/// Checks if this collider and the given collider collided.
		/// Returns CollisionDetails about any detected collision.
		/// Returns null if no collision was detected.
		/// </summary>
		public override CollisionDetails CheckCollision (CheckCollisionSettings settings, Collider other)
		{
			if (other == null)
				return null;
			
			if (other is Collider_Circle)
			{
				return CheckCollisionMethods.CheckCollision (settings, this, other as Collider_Circle);
			}
			/*else if (other is Collider_Something)
			{
				
			}*/

			return null;
		}

		#endregion


		#region Private Methods

		#endregion

	}//end class


}//end namespace
