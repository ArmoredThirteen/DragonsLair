using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace Ate
{


	public static class ExtList
	{
		/// <summary>
		/// Returns true if theList has an entry at index.
		/// </summary>
		public static bool HasIndex<T> (this List<T> theList, int index)
		{
			if (theList.Count == 0 || index < 0 || index >= theList.Count)
				return false;
			
			return true;
		}


		/// <summary>
		/// Debugs the ToString() result of each list entry.
		/// Each debug is on a new line.
		/// </summary>
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

	}//End Class


}//End Namespace
