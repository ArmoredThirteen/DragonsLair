using UnityEngine;
using System.Collections;


namespace Ate
{


	public static class DebugLog
	{

		#region Simple

		/// <summary>
		/// Logs the name then argOne.ToString ().
		/// </summary>
		public static void Simple (string nameOne, object argOne)
		{
			Debug.Log (nameOne + argOne.ToString ());
		}

		/// <summary>
		/// Logs the name then argX.ToString ().
		/// </summary>
		public static void Simple (string nameOne, object argOne, string nameTwo, object argTwo, string separator = ", ")
		{
			Debug.Log (nameOne + argOne.ToString () + separator + nameTwo + argTwo.ToString ());
		}

		/// <summary>
		/// Logs the name then argX.ToString ().
		/// </summary>
		public static void Simple (string nameOne, object argOne, string nameTwo, object argTwo, string nameThree, object argThree, string separator = ", ")
		{
			Debug.Log (nameOne + argOne.ToString () + separator + nameTwo + argTwo.ToString () + separator + nameThree + argThree.ToString ());
		}

		#endregion


		#region Specific Helpers

		public static void Vector3 (string title, Vector3 vector)
		{
			Simple (title + "X: ", vector.x, "Y: ", vector.y, "Z: ", vector.z);
		}

		#endregion

	}//End Class


}//End Namespace
