using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif


namespace CollisionSystem
{
	//TODO: The size of this class will BLOW UP with anything more than circle and sphere colliders!
	//TODO: The size of this class will BLOW UP with anything more than circle and sphere colliders!
	//TODO: The size of this class will BLOW UP with anything more than circle and sphere colliders!


	/// <summary>
	/// A collection of methods for checking collisions between different types of colliders.
	/// </summary>
	public static class CheckCollisionMethods
	{
		
		public static CollisionDetails CheckCollision (CheckCollisionSettings settings, Pair<Collider_Circle,Collider_Circle> colPair)
		{
			return CheckCollision (settings, colPair.v1, colPair.v2);
		}

		public static CollisionDetails CheckCollision (CheckCollisionSettings settings, Collider_Circle colOne, Collider_Circle colTwo)
		{
			Vector3 colOnePos = colOne.GetPosition ();
			Vector3 colTwoPos = colTwo.GetPosition ();

			//	Zero one of the axis based on the upAxis for 2D collision
			colOnePos = new Vector3 (
				(settings.upAxis == VectorAxis.X) ? 0 : colOnePos.x,
				(settings.upAxis == VectorAxis.Y) ? 0 : colOnePos.y,
				(settings.upAxis == VectorAxis.Z) ? 0 : colOnePos.z);

			//	Zero one of the axis based on the upAxis for 2D collision
			colTwoPos = new Vector3 (
				(settings.upAxis == VectorAxis.X) ? 0 : colTwoPos.x,
				(settings.upAxis == VectorAxis.Y) ? 0 : colTwoPos.y,
				(settings.upAxis == VectorAxis.Z) ? 0 : colTwoPos.z);

			float distance = Vector3.Distance (colTwoPos, colOnePos);
			float totalRadius = colOne.radius + colTwo.radius;

			if (distance > totalRadius)
				return null;

			CollisionDetails details = new CollisionDetails (colOne, colTwo);
			return details;
		}


		public static CollisionDetails CheckCollision (CheckCollisionSettings settings, Pair<Collider_Sphere,Collider_Sphere> colPair)
		{
			return CheckCollision (settings, colPair.v1, colPair.v2);
		}

		public static CollisionDetails CheckCollision (CheckCollisionSettings settings, Collider_Sphere colOne, Collider_Sphere colTwo)
		{
			Vector3 colOnePos = colOne.GetPosition ();
			Vector3 colTwoPos = colTwo.GetPosition ();

			float distance = Vector3.Distance (colTwoPos, colOnePos);
			float totalRadius = colOne.radius + colTwo.radius;

			if (distance > totalRadius)
				return null;

			CollisionDetails details = new CollisionDetails (colOne, colTwo);
			return details;
		}


		public static CollisionDetails CheckCollision (CheckCollisionSettings settings, Pair<Collider_Circle,Collider_Sphere> colPair)
		{
			return CheckCollision (settings, colPair.v1, colPair.v2);
		}

		public static CollisionDetails CheckCollision (CheckCollisionSettings settings, Pair<Collider_Sphere,Collider_Circle> colPair)
		{
			return CheckCollision (settings, colPair.v1, colPair.v2);
		}

		public static CollisionDetails CheckCollision (CheckCollisionSettings settings, Collider_Circle colOne, Collider_Sphere colTwo)
		{
			return CheckCollision (settings, colTwo, colOne);
		}

		public static CollisionDetails CheckCollision (CheckCollisionSettings settings, Collider_Sphere colOne, Collider_Circle colTwo)
		{
			Vector3 colOnePos = colOne.GetPosition ();
			Vector3 colTwoPos = colTwo.GetPosition ();

			//	If it is supposed to be flattened,
			//	zero one of the axis based on the upAxis for 2D collision
			if (colTwo.flattenAxisIn3D)
			{
				colOnePos = new Vector3 (
					(settings.upAxis == VectorAxis.X) ? 0 : colOnePos.x,
					(settings.upAxis == VectorAxis.Y) ? 0 : colOnePos.y,
					(settings.upAxis == VectorAxis.Z) ? 0 : colOnePos.z);
			}

			//	Zero one of the axis based on the upAxis for 2D collision
			colTwoPos = new Vector3 (
				(settings.upAxis == VectorAxis.X) ? 0 : colTwoPos.x,
				(settings.upAxis == VectorAxis.Y) ? 0 : colTwoPos.y,
				(settings.upAxis == VectorAxis.Z) ? 0 : colTwoPos.z);

			float distance = Vector3.Distance (colTwoPos, colOnePos);
			float totalRadius = colOne.radius + colTwo.radius;

			if (distance > totalRadius)
				return null;

			CollisionDetails details = new CollisionDetails (colOne, colTwo);
			return details;
		}


	}//end class


}//end namespace
