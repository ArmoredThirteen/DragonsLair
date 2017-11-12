using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif


namespace Ate.Players
{
	
	
	/// <summary>
	/// Description
	/// </summary>
	public abstract class FollowMethod
	{

		public enum FollowType
		{
			None     = 0,
			Mouse    = 100,
			Keyboard = 200,
			Object   = 300,
		}


		#region Public Variables

		#endregion


		#region Private Variables

		#endregion


		#if UNITY_EDITOR

		/*public virtual void DrawInspector ()
		{
			
		}*/

		#endif


		#region AteComponent

		protected abstract void Update ();

		#endregion


		#region Public Methods

		#endregion


		#region Private Methods

		#endregion

	}//End Class
	
	
}//End Namespace
