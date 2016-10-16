using UnityEngine;
using System.Collections;


public abstract class GameSystem : MonoBehaviour
{

	/*
	public override void Initialize (){}
	public override void SceneInitialize (){}
	public override void SystemUpdate (){}
	*/
	/// <summary>
	/// Called by the GameManager at the start of the game.
	/// </summary>
	public abstract void Initialize ();

	/// <summary>
	/// Called by the GameManager at the start of each scene.
	/// </summary>
	public abstract void SceneInitialize ();

	/// <summary>
	/// Called by the GameManager when it updates.
	/// So only the GM is using Unity's Update(),
	/// which has performance issues.
	/// Assume the order in which the individual SystemUpdate()
	/// calls are made is UNRELIABLE.
	/// </summary>
	public abstract void SystemUpdate ();

}

