using UnityEngine;
using System.Collections;


public static class ExtGameObject
{
	#region Ate Specific

	/// <summary>
	/// Uses GetComponent() to return an AteGameObject component.
	/// Can return null.
	/// </summary>
	public static AteGameObject AteGameObject (this GameObject arg)
	{
		return arg.GetComponent<AteGameObject> () as AteGameObject;
	}

	/// <summary>
	/// Uses GetComponents() to return all AteGameObject components.
	/// Can return null.
	/// </summary>
	public static AteGameObject[] AteGameObjects (this GameObject arg)
	{
		return arg.GetComponents<AteGameObject> () as AteGameObject[];
	}

	#endregion


	#region Data Getters Setters

	public static Vector3 GetPosition (this GameObject arg)
	{
		return arg.transform.position;
	}

	public static void SetPosition (this GameObject arg, Vector3 position)
	{
		arg.transform.position = position;
	}

	public static void SetPosition_Offset (this GameObject arg, Vector3 offset)
	{
		arg.transform.position += offset;
	}

	#endregion


	#region Direction Hooks, GameObject targets

	public static Vector3 GetDir_To (this GameObject arg, GameObject target)
	{
		return arg.transform.position.GetDir_To (target.transform.position);
	}
	
	public static Vector3 GetDir_To (this GameObject arg, GameObject target, float magnitude)
	{
		return arg.transform.position.GetDir_To (target.transform.position, magnitude);
	}
	
	public static Vector3 GetDir_AwayFrom (this GameObject arg, GameObject target)
	{
		return arg.transform.position.GetDir_AwayFrom (target.transform.position);
	}
	
	public static Vector3 GetDir_AwayFrom (this GameObject arg, GameObject target, float magnitude)
	{
		return arg.transform.position.GetDir_AwayFrom (target.transform.position, magnitude);
	}

	#endregion


	#region Direction Hooks, Vector3 targets
	
	public static Vector3 GetDir_To (this GameObject arg, Vector3 target)
	{
		return arg.transform.position.GetDir_To (target);
	}
	
	public static Vector3 GetDir_To (this GameObject arg, Vector3 target, float magnitude)
	{
		return arg.transform.position.GetDir_To (target, magnitude);
	}
	
	public static Vector3 GetDir_AwayFrom (this GameObject arg, Vector3 target)
	{
		return arg.transform.position.GetDir_AwayFrom (target);
	}
	
	public static Vector3 GetDir_AwayFrom (this GameObject arg, Vector3 target, float magnitude)
	{
		return arg.transform.position.GetDir_AwayFrom (target, magnitude);
	}
	
	#endregion


	#region Angle Hooks, GameObject targets
	
	public static float GetAngle_FromForward (this GameObject arg, GameObject target)
	{
		return Vector3.Angle (arg.transform.forward, arg.GetDir_To (target));
	}
	
	public static bool IsTarget_Right (this GameObject arg, GameObject target)
	{
		return Vector3.Cross (arg.transform.forward, arg.GetDir_To (target)).y > 0;
	}
	
	public static bool IsTarget_Above (this GameObject arg, GameObject target)
	{
		return arg.transform.position.y > target.transform.position.y;
	}
	
	#endregion


	#region Angle Hooks, Vector3 targets
	
	public static float GetAngle_FromForward (this GameObject arg, Vector3 target)
	{
		return Vector3.Angle (arg.transform.forward, arg.GetDir_To (target));
	}
	
	public static bool IsTarget_Right (this GameObject arg, Vector3 target)
	{
		return Vector3.Cross (arg.transform.forward, arg.GetDir_To (target)).y > 0;
	}
	
	public static bool IsTarget_Above (this GameObject arg, Vector3 target)
	{
		return arg.transform.position.y > target.y;
	}
	
	#endregion

}

