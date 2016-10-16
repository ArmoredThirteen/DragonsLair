using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public static class ExtList
{
	public static void DebugLogAll<T> (this List<T> theList)
	{
		string log = "";

		theList.Count.LoopsOf (i =>
			log += theList[i].ToString () + "\r\n"
			);

		if (log == "")
			log = "Empty List";

		Debug.Log (log);
	}


	/// <summary>
	/// Attempts to switch items at indexOne and Two.
	/// If either index is < 0 or >= theList.Count, it does nothing.
	/// </summary>
	public static void Swap<T> (this List<T> theList, int indexOne, int indexTwo)
	{
		if (indexOne < 0 || indexOne >= theList.Count)
			return;
		if (indexTwo < 0 || indexTwo >= theList.Count)
			return;
		
		T temp = theList[indexOne];
		theList[indexOne] = theList[indexTwo];
		theList[indexTwo] = temp;
	}
}
