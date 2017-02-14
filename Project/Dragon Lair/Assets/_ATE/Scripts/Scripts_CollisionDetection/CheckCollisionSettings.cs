using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif


namespace CollisionSystem
{


	/// <summary>
	/// Description
	/// </summary>
	public class CheckCollisionSettings
	{

		public CheckCollisionSettings (float maxCheckDistance, VectorAxis upAxis)
		{
			this.maxCheckDistance = maxCheckDistance;
			this.upAxis = upAxis;
		}


		#region Public Variables

		/// <summary>
		/// If two colliders are further from each other than this they never collide.
		/// </summary>
		public float maxCheckDistance = 50;

		/// <summary>
		/// Controls alignment of Collider2D checks.
		/// </summary>
		public VectorAxis upAxis = VectorAxis.Y;

		#endregion


		#region Private Variables

		#endregion


		#region Public Methods

		#endregion


		#region Private Methods

		#endregion

	}//end class


}//end namespace