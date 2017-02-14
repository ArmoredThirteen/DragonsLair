using UnityEngine;
using System.Collections;


namespace CollisionSystem
{
	

	/// <summary>
	/// Description
	/// </summary>
	public class CollisionDetails
	{

		public CollisionDetails (Collider colOne, Collider colTwo)
		{
			this.colOne = colOne;
			this.colTwo = colTwo;
		}

		public CollisionDetails (Pair<Collider,Collider> colPair)
		{
			this.colOne = colPair.v1;
			this.colTwo = colPair.v2;
		}


		#region Public Variables

		public Collider colOne;
		public Collider colTwo;

		#endregion


		#region Private Variables

		#endregion


		#region Public Methods

		#endregion


		#region Private Methods

		#endregion

	}//end class


}//end namespace
