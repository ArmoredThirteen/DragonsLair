using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif


namespace Ate
{
	
	
	/// <summary>
	/// Description
	/// </summary>
	public class SimpleFollowMouse : AteComponent_fpsControlled
	{
		
		#region Public Variables

		public Camera theCam;

		public float movePerFrame = 0.5f;

		public bool freezeAxis_x = false;
		public bool freezeAxis_y = false;
		public bool freezeAxis_z = false;

		#endregion


		#region Private Variables

		#endregion


		#if UNITY_EDITOR

		public override void DrawInspector ()
		{
			base.DrawInspector ();

			EditorGUILayout.Space ();
			theCam = EditorGUILayout.ObjectField
				("Camera", theCam, typeof (Camera), true)
				as Camera;

			EditorGUILayout.Space ();
			movePerFrame = EditorGUILayout.FloatField ("Move Dist / Frame", movePerFrame);

			EditorGUILayout.Space ();
			DrawFreezeSettings ();
		}

		private void DrawFreezeSettings ()
		{
			EditorGUILayout.LabelField ("Freeze Axis:");

			EditorGUILayout.BeginHorizontal ();

			int labelWidth  = 12;
			int toggleWidth = 20;

			EditorGUILayout.LabelField ("X", GUILayout.Width (labelWidth));
			freezeAxis_x = EditorGUILayout.Toggle (freezeAxis_x, GUILayout.Width (toggleWidth));

			EditorGUILayout.LabelField ("Y", GUILayout.Width (labelWidth));
			freezeAxis_y = EditorGUILayout.Toggle (freezeAxis_y, GUILayout.Width (toggleWidth));

			EditorGUILayout.LabelField ("Z", GUILayout.Width (labelWidth));
			freezeAxis_z = EditorGUILayout.Toggle (freezeAxis_z, GUILayout.Width (toggleWidth));

			EditorGUILayout.EndHorizontal ();
		}

		#endif


		#region AteComponent

		// Updates every game frame
		protected override void AteUpdate ()
		{
			
		}

		// Updates 24 times per second
		protected override void FpsUpdate24 ()
		{
			
		}

		// Updates once per framelength, which is one or more FpsUpdate## calls
		protected override void UpdateFrameLength ()
		{
			Vector3 curPos  = Position;
			Vector3 targPos = theCam.ScreenToWorldPoint (Input.mousePosition);

			float newX = freezeAxis_x ? curPos.x : MoveOnAxis (curPos.x, targPos.x);
			float newY = freezeAxis_y ? curPos.y : MoveOnAxis (curPos.y, targPos.y);
			float newZ = freezeAxis_z ? curPos.z : MoveOnAxis (curPos.z, targPos.z);

			Position = new Vector3 (newX, newY, newZ);
		}

		#endregion


		#region Public Methods

		#endregion


		#region Private Methods

		private float MoveOnAxis (float curVal, float targVal)
		{
			//	targVal is close enough to targVal to not have to move
			if (Mathf.Abs (curVal-targVal) < movePerFrame)
				return curVal;

			if (curVal > targVal)
				return curVal - movePerFrame;

			if (curVal < targVal)
				return curVal + movePerFrame;

			//	Shouldn't ever happen
			return curVal;
		}

		#endregion

	}//End Class
	
	
}//End Namespace
