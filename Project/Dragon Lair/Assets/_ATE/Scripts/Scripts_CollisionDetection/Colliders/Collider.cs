using UnityEngine;
using System.Collections;
using Ate.GameSystems;

#if UNITY_EDITOR
using UnityEditor;
#endif


namespace Ate.Collision
{
	

	/// <summary>
	/// Description
	/// </summary>
	public abstract class Collider : AteComponent
	{
		
		#region Public Variables

		public CollisionArea MyArea
		{
			get {return _myArea;}
			set {_myArea = value;}
		}

		#endregion


		#region Private Variables

		private CollisionArea _myArea = null;

		#endregion


		#if UNITY_EDITOR

		public override void DrawInspector ()
		{
			base.DrawInspector ();
		}

		#endif


		#region AteComponent

		protected override void AteStart ()
		{
			RegisterCollider ();
		}

		protected override void AteUpdate ()
		{
			
		}

		//	Not actually from AteObject, will hopefully be eventually though
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
		public CollisionDetails CheckCollision (CheckCollisionSettings settings, Collider other)
		{
			if (other == null)
				return null;

			Collider_Circle thisCircle  = this as Collider_Circle;
			Collider_Sphere thisSphere  = this as Collider_Sphere;

			Collider_Circle otherCircle = other as Collider_Circle;
			Collider_Sphere otherSphere = other as Collider_Sphere;

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

	}//End Class


}//End Namespace
