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
	public class DestroyAfterDuration : AteComponent_fpsControlled
	{
		
		#region Public Variables

		public int frameDuration = 8;

		#endregion


		#region Private Variables

		private int _framesSinceStart = 0;

		#endregion


		#if UNITY_EDITOR

		public override void DrawInspector ()
		{
			base.DrawInspector ();

			frameDuration = EditorGUILayout.IntField ("Frame Duration", frameDuration);
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
			_framesSinceStart = _framesSinceStart + 1;

			if (_framesSinceStart >= frameDuration)
			{
				GameObject.Destroy (this);
			}
		}

		#endregion


		#region Public Methods

		#endregion


		#region Private Methods

		#endregion

	}//End Class
	
	
}//End Namespace
