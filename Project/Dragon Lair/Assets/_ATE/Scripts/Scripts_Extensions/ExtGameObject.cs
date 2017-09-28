using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public static class ExtGameObject
{
	#region Ate Specific

	/// <summary>
	/// Uses GetComponent() to return an AteObject component.
	/// Can return null.
	/// </summary>
	public static AteObject AteGameObject (this GameObject arg)
	{
		return arg.GetComponent<AteObject> () as AteObject;
	}

	/// <summary>
	/// Uses GetComponents() to return all AteObject components.
	/// Can return null.
	/// </summary>
	public static AteObject[] AteGameObjects (this GameObject arg)
	{
		return arg.GetComponents<AteObject> () as AteObject[];
	}

	#endregion


	#region Utility

	#if UNITY_EDITOR
	/// <summary>
	/// Returns name+GameObjectID.
	/// Only usable in Editor.
	/// </summary>
	public static string name_ID (this GameObject arg)
	{
		return string.Concat (arg.name, arg.GetInstanceID ());
	}
	#endif

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

	/// <summary>
	/// Gets the children as GameObjects, does not include itself.
	/// </summary>
	public static List<GameObject> GetChildrenGameObjects (this GameObject arg)
	{
		List<GameObject> children = new List<GameObject> ();

		int childCount = arg.transform.childCount;
		for (int i = 0; i < childCount; i++)
		{
			children.Add (arg.transform.GetChild (i).gameObject);
		}

		return children;
	}

	/// <summary>
	/// Gets the children as Transforms, does not include itself.
	/// </summary>
	public static List<Transform> GetChildrenTransforms (this GameObject arg)
	{
		List<Transform> children = new List<Transform> ();

		int childCount = arg.transform.childCount;
		for (int i = 0; i < childCount; i++)
		{
			children.Add (arg.transform.GetChild (i));
		}

		return children;
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

