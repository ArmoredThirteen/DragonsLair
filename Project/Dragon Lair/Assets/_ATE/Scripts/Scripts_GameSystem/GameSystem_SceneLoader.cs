using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public class GameSystem_SceneLoader : GameSystem
{
	#region GameSystem

	public override void Initialize (){}

	public override void SceneInitialize ()
	{
		GameManager.Events.Register<EventType_Gameplay, EventData_Gameplay> ((int)EventType_Gameplay.SceneLoad,   OnLoadScene);
		GameManager.Events.Register<EventType_Gameplay, EventData_Gameplay> ((int)EventType_Gameplay.SceneReload, OnReloadCurrentScene);
	}

	public override void SystemUpdate (){}

	#endregion


	private void OnLoadScene (EventData_Gameplay theData)
	{
		SceneManager.LoadScene (theData.SceneIndex);
	}

	private void OnReloadCurrentScene (EventData_Gameplay theData)
	{
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
	}

}

