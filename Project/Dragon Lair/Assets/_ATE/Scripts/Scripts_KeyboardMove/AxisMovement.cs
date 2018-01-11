using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif


namespace Ate.KeyboardMove
{
	
	
	/// <summary>
	/// Description
	/// </summary>
	public class AxisMovement : AteComponent_fpsControlled
	{

		#if UNITY_EDITOR
		//	For use in the editor to give titles to the list of axis.
		private enum AxisNames
		{
			Forward  = 0,
			Backward = 1,
			Left     = 2,
			Right    = 3,
			Up       = 4,
			Down     = 5,
		}
		#endif

		#region Public Variables

		public float movePerFrame = 0.5f;

		public AxisData[] axisDatas = new AxisData[6];

		public float maxDist_X = 10;
		public float maxDist_Y = 10;
		public float maxDist_Z = 10;

		#endregion


		#region Private Variables

		#endregion


		#region Properties

		public AxisData ForwardData
		{
			get {return axisDatas[0];}
			set {axisDatas[0] = value;}
		}

		public AxisData BackwardData
		{
			get {return axisDatas[1];}
			set {axisDatas[1] = value;}
		}

		public AxisData LeftData
		{
			get {return axisDatas[2];}
			set {axisDatas[2] = value;}
		}

		public AxisData RightData
		{
			get {return axisDatas[3];}
			set {axisDatas[3] = value;}
		}

		public AxisData UpData
		{
			get {return axisDatas[4];}
			set {axisDatas[4] = value;}
		}

		public AxisData DownData
		{
			get {return axisDatas[5];}
			set {axisDatas[5] = value;}
		}

		#endregion


		#if UNITY_EDITOR

		public override void DrawInspector ()
		{
			base.DrawInspector ();

			EditorGUILayout.Space ();

			movePerFrame = EditorGUILayout.FloatField ("Move Dist / Frame", movePerFrame);

			EditorGUILayout.Space ();

			maxDist_X = EditorGUILayout.FloatField ("Max X Distance", maxDist_X);
			maxDist_Y = EditorGUILayout.FloatField ("Max Y Distance", maxDist_Y);
			maxDist_Z = EditorGUILayout.FloatField ("Max Z Distance", maxDist_Z);

			EditorGUILayout.Space ();

			DrawAxisDatas ();
		}

		private void DrawAxisDatas ()
		{
			for (int i = 0; i < axisDatas.Length; i++)
			{
				EditorGUILayout.LabelField (System.Enum.GetName (typeof(AxisNames), i));

				EditorGUI.indentLevel++;
				axisDatas[i].DrawInspector ();
				EditorGUI.indentLevel--;

				EditorGUILayout.Space ();
			}
		}

		#endif


		#region AteComponent

		protected override void RegisterEvents ()
		{
			base.RegisterEvents ();

			for (int i = 0; i < axisDatas.Length; i++)
			{
				axisDatas[i].RegisterEvents ();
			}
		}

		protected override void UnregisterEvents ()
		{
			base.UnregisterEvents ();

			for (int i = 0; i < axisDatas.Length; i++)
			{
				axisDatas[i].UnregisterEvents ();
			}
		}


		protected override void AteAwake ()
		{
			base.AteAwake ();
		}

		protected override void AteUpdate ()
		{
			
		}

		protected override void UpdateBaseFps ()
		{
			
		}

		protected override void UpdateFrameLength ()
		{
			Vector3 curPos = Position;

			float newX = curPos.x;
			float newY = curPos.y;
			float newZ = curPos.z;

			for (int i = 0; i < axisDatas.Length; i++)
			{
				AxisData theData = axisDatas[i];
				if (!theData.isUsed)
					continue;
				if (!theData.IsKeyPressed)
					continue;

				float moveAmount = movePerFrame;
				if (theData.negativeAxis)
					moveAmount = moveAmount * -1;

				switch (theData.axisDirection)
				{
					case AxisData.AxisDir.X:
						newX = newX + moveAmount;
						break;

					case AxisData.AxisDir.Y:
						newY = newY + moveAmount;
						break;

					case AxisData.AxisDir.Z:
						newZ = newZ + moveAmount;
						break;
				}
			}

			if (maxDist_X >= 0 && newX != curPos.x)
			{
				newX = Mathf.Min (newX, maxDist_X);
				newX = Mathf.Max (newX, maxDist_X * -1);
			}
			if (maxDist_Y >= 0 && newY != curPos.y)
			{
				newY = Mathf.Min (newY, maxDist_Y);
				newY = Mathf.Max (newY, maxDist_Y * -1);
			}
			if (maxDist_Z >= 0 && newZ != curPos.z)
			{
				newZ = Mathf.Min (newZ, maxDist_Z);
				newZ = Mathf.Max (newZ, maxDist_Z * -1);
			}

			Position = new Vector3 (newX, newY, newZ);
		}

		#endregion


		#region Public Methods

		#endregion


		#region Private Methods

		#endregion

	}//End Class
	
	
}//End Namespace
