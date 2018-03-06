using UnityEngine;
using System.Collections;


namespace Ate.GameSystems
{


	public abstract class GameSystem : MonoBehaviour
	{

		/*
		public override void Initialize (){}
		public override void SceneInitialize (){}
		public override void SystemUpdate (){}
		public override void SystemLateUpdate (){}
		*/
		/// <summary>
		/// Called by the GameManager at the start of the game.
		/// </summary>
		public abstract void Initialize ();

		/// <summary>
		/// Called by the GameManager at the start of each scene.
		/// </summary>
		public abstract void SceneLoaded ();

		/// <summary>
		/// Called by the GameManager when it updates.
		/// So only the GM is using Unity's Update(),
		/// which has performance issues.
		/// Assume the order in which the individual SystemUpdate()
		/// calls are made is UNRELIABLE.
		/// </summary>
		public abstract void SystemUpdate ();

		/// <summary>
		/// Called by the GameManager when it late updates.
		/// So only the GM is using Unity's LateUpdate(),
		/// which has performance issues.
		/// Assume the order in which the individual SystemLateUpdate()
		/// calls are made is UNRELIABLE.
		/// </summary>
		public abstract void SystemLateUpdate ();

	}//End Class


}//End Namespace
