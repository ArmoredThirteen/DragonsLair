using UnityEngine;
using System.Collections;
using System;


public static class ExtInt
{
	//	Example:
	//	5.LoopsOf (() => Debug.Log ("sup"));
	public static void LoopsOf (this int count, Action action)
	{
		for (int i = 0; i < count; i++)
			action();
	}

	//	Example:
	//	5.LoopsOf (i => Debug.Log ("sup " + i));
	//	myArr.Length.LoopsOf (i => sumVal += myArr[i]);
	public static void LoopsOf (this int count, Action<int> action)
	{
		for (int i = 0; i < count; i++)
			action(i);
	}
}
