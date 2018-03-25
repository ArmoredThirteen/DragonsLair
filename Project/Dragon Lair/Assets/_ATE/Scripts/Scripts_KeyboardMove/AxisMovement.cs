using UnityEngine;
using System.Collections;
using Ate.GameSystems;
using System.Collections.Generic;

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

		//TODO: Hacky live-updating list is inefficient and prone to abuse
		/// <summary>
		/// Keys clicked this frame. Intention is so if you quickly click
		/// then release a key during a framelength you still move once.
		/// </summary>
		private List<KeyCode> _keysClickedThisFrame = new List<KeyCode> ();

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

			GameManager.Events.Register<EventType_UI, EventData_UI> ((int)EventType_UI.Clicked, OnKeyClicked);
			GameManager.Events.Register<EventType_UI, EventData_UI> ((int)EventType_UI.Released, OnKeyReleased);
		}

		protected override void UnregisterEvents ()
		{
			base.UnregisterEvents ();

			GameManager.Events.Unregister<EventType_UI, EventData_UI> ((int)EventType_UI.Clicked, OnKeyClicked);
			GameManager.Events.Unregister<EventType_UI, EventData_UI> ((int)EventType_UI.Released, OnKeyReleased);
		}


		protected override void AteAwake ()
		{
			base.AteAwake ();
		}


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
			Vector3 curPos = Position;

			float newX = curPos.x;
			float newY = curPos.y;
			float newZ = curPos.z;

			for (int i = 0; i < axisDatas.Length; i++)
			{
				AxisData theData = axisDatas[i];
				if (!theData.isUsed)
					continue;
				if (!theData.IsKeyClicked && !_keysClickedThisFrame.Contains (theData.activateKey))
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

			_keysClickedThisFrame.Clear ();
		}

		#endregion


		#region Public Methods

		#endregion


		#region Private Methods

		private void OnKeyClicked (EventData_UI eventData)
		{
			for (int i = 0; i < axisDatas.Length; i++)
			{
				if (eventData.TheKey == axisDatas[i].activateKey)
				{
					axisDatas[i].OnKeyClicked ();
				}
			}

			if (!_keysClickedThisFrame.Contains (eventData.TheKey))
			{
				_keysClickedThisFrame.Add (eventData.TheKey);
			}
		}

		private void OnKeyReleased (EventData_UI eventData)
		{
			for (int i = 0; i < axisDatas.Length; i++)
			{
				if (eventData.TheKey == axisDatas[i].activateKey)
				{
					axisDatas[i].OnKeyReleased ();
				}
			}
		}

		#endregion

	}//End Class
	
	
}//End Namespace
