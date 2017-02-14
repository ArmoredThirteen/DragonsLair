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
	public abstract class Collider : AteGameObject
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


		#region AteGameObject

		protected override void AteUpdate ()
		{
			
		}

		#endregion


		#region Public Methods

		/// <summary>
		/// Returns the world position with any modifications, such as offset.
		/// </summary>
		public abstract Vector3 GetPosition ();

		/// <summary>
		/// Checks if this collider and the given collider collided.
		/// Returns CollisionDetails about any detected collision.
		/// Returns null if no collision was detected.
		/// </summary>
		public abstract CollisionDetails CheckCollision (CheckCollisionSettings settings, Collider other);


		/// <summary>
		/// Returns true if both Colliders' InstanceIDs are the same.
		/// </summary>
		public override bool Equals (object obj)
		{
			if (obj == null)
				return false;
			if (this.GetType () != obj.GetType ())
				return false;

			Collider col = (Collider)obj;
			return this.InstanceID.Equals (col.InstanceID);
		}

		public static bool operator == (Collider colOne, Collider colTwo)
		{
			bool oneIsNull = object.ReferenceEquals (colOne, null);
			bool twoIsNull = object.ReferenceEquals (colTwo, null);

			//	Equal if both are null
			//if (colOne.Equals(null) && colTwo.Equals(null))
			if (oneIsNull && twoIsNull)
				return true;
			//	Only colOne is null, so they are unequal
			//else if (colOne.Equals(null))
			else if (oneIsNull)
				return false;
			
			//	Above if/elseif ensures colOne is not null
			//	Collider.Equals returns false if given condition is null
			return colOne.Equals (colTwo);
		}

		public static bool operator != (Collider colOne, Collider colTwo)
		{
			return !(colOne == colTwo);
		}

		#endregion


		#region Private Methods

		#endregion

	}//end class


}//end namespace
