using UnityEngine;
using System.Collections;


public static class ExtVector3
{
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
	
	
	public static Vector3 GetDir_RandomInCircle (this Vector3 arg, float magnitude = 1)
	{
		Vector2 randVect = Random.insideUnitCircle.normalized * magnitude;
		return new Vector3 (randVect.x, 0, randVect.y);
	}
	
	public static Vector3 GetDir_RandomInSphere (this Vector3 arg, float magnitude = 1)
	{
		Vector3 randVect = Random.insideUnitSphere.normalized * magnitude;
		return new Vector3 (randVect.x, randVect.y, randVect.z);
	}
	
	#endregion
}
