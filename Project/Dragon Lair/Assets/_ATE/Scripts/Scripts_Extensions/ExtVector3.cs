using UnityEngine;
using System.Collections;


public static class ExtVector3
{
	#region Distance

	public static float SqrDistanceTo (this Vector3 val, Vector3 target)
	{
		return (val-target).sqrMagnitude;
	}

	/// <summary>
	/// Returns true if given vectors are further apart than distance.
	/// Squares the distance and compares against val.SqrDistance(target).
	/// </summary>
	/*public static bool IsTargFurtherThanDist (this Vector3 val, Vector3 target, float distance)
	{
		return (distance*distance) > val.SqrDistanceTo (target);
	}*/

	#endregion


	#region Changed Value Hooks

	public static Vector3 xOf (this Vector3 val, float x)
	{
		return new Vector3 (x, val.y, val.z);
	}
	
	public static Vector3 yOf (this Vector3 val, float y)
	{
		return new Vector3 (val.x, y, val.z);
	}
	
	public static Vector3 zOf (this Vector3 val, float z)
	{
		return new Vector3 (val.x, val.y, z);
	}

	
	public static Vector3 noX (this Vector3 val)
	{
		return new Vector3 (0, val.y, val.z);
	}
	
	public static Vector3 noY (this Vector3 val)
	{
		return new Vector3 (val.x, 0, val.z);
	}
	
	public static Vector3 noZ (this Vector3 val)
	{
		return new Vector3 (val.x, val.y, 0);
	}

	#endregion


	#region Direction Hooks
	
	public static Vector3 GetDir_To (this Vector3 arg, Vector3 targetPosition)
	{
		return (targetPosition - arg);
	}
	
	public static Vector3 GetDir_To (this Vector3 arg, Vector3 targetPosition, float magnitude)
	{
		return (targetPosition - arg).normalized * magnitude;
	}
	
	
	public static Vector3 GetDir_AwayFrom (this Vector3 arg, Vector3 targetPosition)
	{
		return (arg - targetPosition);
	}
	
	public static Vector3 GetDir_AwayFrom (this Vector3 arg, Vector3 targetPosition, float magnitude)
	{
		return (arg - targetPosition).normalized * magnitude;
	}
	

	/// <summary>
	/// Returns the Vector offset by a random amount, along the y/z axis.
	/// </summary>
	public static Vector3 GetLoc_RandomInCircle_UpX (this Vector3 arg, float magnitude = 1)
	{
		Vector2 randVect = Random.insideUnitCircle.normalized * magnitude;
		return new Vector3 (arg.x+0, arg.y+randVect.x, arg.z+randVect.y);
	}

	/// <summary>
	/// Returns the Vector offset by a random amount, along the x/z axis.
	/// </summary>
	public static Vector3 GetLoc_RandomInCircle_UpY (this Vector3 arg, float magnitude = 1)
	{
		Vector2 randVect = Random.insideUnitCircle.normalized * magnitude;
		return new Vector3 (arg.x+randVect.x, arg.y+0, arg.z+randVect.y);
	}

	/// <summary>
	/// Returns the Vector offset by a random amount, along the x/y axis.
	/// </summary>
	public static Vector3 GetLoc_RandomInCircle_UpZ (this Vector3 arg, float magnitude = 1)
	{
		Vector2 randVect = Random.insideUnitCircle.normalized * magnitude;
		return new Vector3 (arg.x+randVect.x, arg.y+randVect.y, arg.z+0);
	}

	
	public static Vector3 GetDir_RandomInSphere (this Vector3 arg, float magnitude = 1)
	{
		Vector3 randVect = Random.insideUnitSphere.normalized * magnitude;
		return new Vector3 (randVect.x, randVect.y, randVect.z);
	}
	
	#endregion
}
