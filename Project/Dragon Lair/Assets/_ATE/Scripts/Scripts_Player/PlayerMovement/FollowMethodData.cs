using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif


namespace Ate
{
	
	
	/// <summary>
	/// Serializable DATA ONLY class for FollowMethods.
	/// Unity sucks at serializing abstract classes so this
	/// creates a non-abstract way to store data. Then the
	/// FollowMethod_Child classes can reference this instead
	/// of relying on their own non-serializable data.
	/// </summary>
	[System.Serializable]
	public class FollowMethodData
	{
		
		#region Public Variables

		public float moveDistancePerFrame = 0.5f;

		#endregion


		#region Private Variables

		#endregion


		#if UNITY_EDITOR

		public void DrawInspector ()
		{
			moveDistancePerFrame = EditorGUILayout.FloatField ("Move Dist / Frame", moveDistancePerFrame);
		}

		#endif


		#region Public Methods

		#endregion


		#region Private Methods

		#endregion

	}//End Class
	
	
}//End Namespace
