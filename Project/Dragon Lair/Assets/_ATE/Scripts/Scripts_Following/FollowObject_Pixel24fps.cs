using UnityEngine;
using System.Collections;
using Ate.GameSystems;

#if UNITY_EDITOR
using UnityEditor;
#endif


namespace Ate
{


	/// <summary>
	/// Follows a target object using a more retro feel.
	/// Intended to snap to pixels on slower framerate.
	/// Uses target object as guide to determine when to snap.
	/// </summary>
	public class FollowObject_Pixel24fps : AteComponent_fpsControlled
	{

		#region Public Variables

		public GameObject targetObject = null;

		public int pixelsPerUnit = 8;

		#endregion


		#region Private Variables

		#endregion


		#if UNITY_EDITOR

		public override void DrawInspector ()
		{
			base.DrawInspector ();

			EditorGUILayout.Space ();

			targetObject = EditorGUILayout.ObjectField
				("Target", targetObject, typeof(GameObject), true)
				as GameObject;

			pixelsPerUnit = EditorGUILayout.IntField ("Pixels per Unit", pixelsPerUnit);
		}

		#endif


		#region AteComponent

		protected override void AteUpdate ()
		{

		}


		protected override void UpdateBaseFps ()
		{

		}

		protected override void UpdateFrameLength ()
		{
			UpdateLocation ();
		}

		#endregion


		#region Public Methods

		#endregion


		#region Private Methods

		private void UpdateLocation ()
		{
			float desiredX = targetObject.GetPosition ().x;
			float desiredY = targetObject.GetPosition ().y;
			float desiredZ = targetObject.GetPosition ().z;

			desiredX = Mathf.Round (desiredX * pixelsPerUnit) / pixelsPerUnit;
			desiredY = Mathf.Round (desiredY * pixelsPerUnit) / pixelsPerUnit;
			desiredZ = Mathf.Round (desiredZ * pixelsPerUnit) / pixelsPerUnit;

			Position = new Vector3 (desiredX, desiredY, desiredZ);
		}

		#endregion

	}//End Class


}//End Namespace
