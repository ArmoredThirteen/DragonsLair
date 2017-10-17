using UnityEngine;
using System.Collections;


namespace Ate
{


	public class GameSystem_ResolutionManager : GameSystem
	{
		public int width = 960;
		public int height = 600;


		#region GameSystem

		public override void Initialize ()
		{
			Screen.SetResolution (width, height, false);
		}

		public override void SceneInitialize (){}
		public override void SystemUpdate (){}
		public override void SystemLateUpdate (){}

		#endregion

	}//End Class


}//End Namespace
