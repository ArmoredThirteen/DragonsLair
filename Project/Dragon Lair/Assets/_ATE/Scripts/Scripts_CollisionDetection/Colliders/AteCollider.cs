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
	public abstract class AteCollider : AteGameObject
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

		protected override void AteStart ()
		{
			RegisterCollider ();
		}

		protected override void AteUpdate ()
		{
			
		}

		//	Not actually from AteGameObject, will hopefully be eventually though
		void OnDestroy ()
		{
			UnregisterCollider ();
		}

		#endregion


		#region Public Methods

		public void RegisterCollider ()
		{
			EventData_Collision theData = new EventData_Collision ();
			theData.ColliderOne = this;

			GameManager.Events.Broadcast<EventType_Collision> ((int)EventType_Collision.RegisterCollider, theData);
		}

		public void UnregisterCollider ()
		{
			EventData_Collision theData = new EventData_Collision ();
			theData.ColliderOne = this;

			GameManager.Events.Broadcast<EventType_Collision> ((int)EventType_Collision.UnregisterCollider, theData);
		}

		/// <summary>
		/// Returns the world position with any modifications, such as offset.
		/// </summary>
		public abstract Vector3 GetPosition ();

		/// <summary>
		/// Checks if this collider and the given collider collided.
		/// Returns CollisionDetails about any detected collision.
		/// Returns null if no collision was detected.
		/// </summary>
		//public abstract CollisionDetails CheckCollision (CheckCollisionSettings settings, Collider other);
		public CollisionDetails CheckCollision (CheckCollisionSettings settings, AteCollider other)
		{
			if (other == null)
				return null;

			AteCollider_Circle thisCircle  = this as AteCollider_Circle;
			AteCollider_Sphere thisSphere  = this as AteCollider_Sphere;

			AteCollider_Circle otherCircle = other as AteCollider_Circle;
			AteCollider_Sphere otherSphere = other as AteCollider_Sphere;

			//TODO: Hairy hairy hairy
			if (thisCircle != null)
			{
				if (otherCircle != null)
				{
					return CheckCollisionMethods.CheckCollision (settings, thisCircle, otherCircle);
				}
				else if (otherSphere != null)
				{
					return CheckCollisionMethods.CheckCollision (settings, thisCircle, otherSphere);
				}
			}
			else if (thisSphere != null)
			{
				if (otherCircle != null)
				{
					return CheckCollisionMethods.CheckCollision (settings, thisSphere, otherCircle);
				}
				else if (otherSphere != null)
				{
					return CheckCollisionMethods.CheckCollision (settings, thisSphere, otherSphere);
				}
			}

			return null;
		}

		#endregion


		#region Equality

		/// <summary>
		/// Returns true if both Colliders' InstanceIDs are the same.
		/// </summary>
		public override bool Equals (object obj)
		{
			if (obj == null)
				return false;
			if (this.GetType () != obj.GetType ())
				return false;

			AteCollider col = (AteCollider)obj;
			return this.InstanceID.Equals (col.InstanceID);
		}

		public static bool operator == (AteCollider colOne, AteCollider colTwo)
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

		public static bool operator != (AteCollider colOne, AteCollider colTwo)
		{
			return !(colOne == colTwo);
		}

		#endregion


		#region Private Methods

		#endregion

	}//end class


}//end namespace
