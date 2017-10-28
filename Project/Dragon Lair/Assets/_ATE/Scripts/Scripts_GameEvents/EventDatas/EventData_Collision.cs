using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Ate.Collision;
using Collider = Ate.Collision.Collider;


namespace Ate
{


	public class EventData_Collision : EventData
	{
		private CollisionArea _areaOne;
		private CollisionArea _areaTwo;

		private Collider _colOne;
		private Collider _colTwo;


		#region Area Collisions

		/// <summary>
		/// During an Area Collision, this is the CollisionArea that was collided with.
		/// </summary>
		public CollisionArea HitArea
		{
			get {return _areaOne;}
			set {_areaOne = value;}
		}

		/// <summary>
		/// During an Area Collision, this is the CollisionArea that was collided with.
		/// </summary>
		public CollisionArea HittingArea
		{
			get {return _areaTwo;}
			set {_areaTwo = value;}
		}

		#endregion


		#region Colliders

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


		/// <summary>
		/// During an Area Collision, this is the collider belonging to a CollisionArea.
		/// </summary>
		public Collider HitCollider
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
