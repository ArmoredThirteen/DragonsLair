using UnityEngine;
using System.Collections;
using System;


public static class ExtFloat
{
	public static bool IsInRange (this float val, float min, float max)
	{
		return (val > min) && (val < max);
	}

	public static bool IsInRange (this float val, FloatRange range)
	{
		return (val > range.v1) && (val < range.v2);
	}
}
