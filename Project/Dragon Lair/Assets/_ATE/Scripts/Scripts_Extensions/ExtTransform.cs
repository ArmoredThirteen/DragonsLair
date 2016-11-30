using UnityEngine;
using System.Collections;
using System;


public static class ExtTransform
{
	public static void LookAt_AroundZ (this Transform val, Transform target)
	{
		Vector3 targetPosition = target.position.zOf (val.position.z);
		val.LookAt (targetPosition);
	}

}
