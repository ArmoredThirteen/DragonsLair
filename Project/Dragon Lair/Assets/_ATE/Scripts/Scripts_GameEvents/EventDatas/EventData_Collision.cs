using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Ate.Collision;
using Collider = Ate.Collision.Collider;


namespace Ate
{


	public class EventData_Collision : EventData
	{
		private CollisionArea _colArea;

		private Collider _colOne;
		private Collider _colTwo;


		#region Base Collision

		/// <summary>
		/// During a base collision, this is arbitrarily one of the two colliders involved.
		/// </summary>
		public Collider ColliderOne
		{
			get {return _colOne;}
			set {_colOne = value;}
		}

		/// <summary>
		/// During a base collision, this is arbitrarily one of the two colliders involved.
		/// </summary>
		public Collider ColliderTwo
		{
			get {return _colTwo;}
			set {_colTwo = value;}
		}

		#endregion


		#region Area Collision

		/// <summary>
		/// During an Area Collision, this is the relevant CollisionArea that was collided with.
		/// </summary>
		public CollisionArea FullCollisionArea
		{
			get {return _colArea;}
			set {_colArea = value;}
		}

		/// <summary>
		/// During an Area Collision, this is the collider belonging to a CollisionArea.
		/// </summary>
		public Collider AreaCollider
		{
			get {return _colOne;}
			set {_colOne = value;}
		}

		/// <summary>
		/// During an Area Collision, this is the collider that interacted with a CollisionArea.
		/// </summary>
		public Collider HittingCollider
		{
			get {return _colTwo;}
			set {_colTwo = value;}
		}

		#endregion

	}//End Class


}//End Namespace
