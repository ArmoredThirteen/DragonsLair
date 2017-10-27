﻿using UnityEngine;
using System.Collections;
using Collider = Ate.Collision.Collider;


namespace Ate.Collision
{
	

	/// <summary>
	/// Description
	/// </summary>
	public class CollisionDetails
	{

		public CollisionDetails (Collider colOne, Collider colTwo)
		{
			this.areaOne = colOne.MyArea;
			this.areaTwo = colTwo.MyArea;
			this.colOne  = colOne;
			this.colTwo  = colTwo;
		}

		/*public CollisionDetails (Pair<Collider,Collider> colPair)
		{
			this.colOne = colPair.v1;
			this.colTwo = colPair.v2;
		}*/


		#region Public Variables

		public CollisionArea areaOne;
		public CollisionArea areaTwo;

		public Collider colOne;
		public Collider colTwo;

		#endregion


		#region Private Variables

		#endregion


		#region Public Methods

		#endregion


		#region Equality

		/// <summary>
		/// Returns true if both CollisionDetails have matching AteColliders.
		/// </summary>
		public override bool Equals (object obj)
		{
			if (obj == null)
				return false;
			if (this.GetType () != obj.GetType ())
				return false;

			CollisionDetails details = (CollisionDetails)obj;
			//	Two match checks because they are order independant, as long as both match.
			bool matchOne = (this.colOne == details.colOne) && (this.colTwo == details.colTwo);
			bool matchTwo = (this.colOne == details.colTwo) && (this.colTwo == details.colOne);
			return matchOne || matchTwo;
		}

		public static bool operator == (CollisionDetails detailsOne, CollisionDetails detailsTwo)
		{
			bool oneIsNull = object.ReferenceEquals (detailsOne, null);
			bool twoIsNull = object.ReferenceEquals (detailsTwo, null);

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
			return detailsOne.Equals (detailsTwo);
		}

		public static bool operator != (CollisionDetails detailsOne, CollisionDetails detailsTwo)
		{
			return !(detailsOne == detailsTwo);
		}

		#endregion


		#region Private Methods

		#endregion

	}//End Class


}//End Namespace
