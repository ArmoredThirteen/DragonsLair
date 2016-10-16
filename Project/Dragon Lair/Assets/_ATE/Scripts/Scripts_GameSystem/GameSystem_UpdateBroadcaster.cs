﻿using UnityEngine;
using System.Collections;


public class GameSystem_UpdateBroadcaster : GameSystem
{
	private static EventData_Updates _data_updateOne = new EventData_Updates (0);
	private static EventData_Updates _data_updateTwo = new EventData_Updates (1);
	//private static EventData_Updates _data_updateThree = new EventData_Updates (2);


	#region GameSystem

	public override void Initialize (){}
	public override void SceneInitialize (){}

	public override void SystemUpdate ()
	{
		GameManager.Events.Broadcast<EventType_Updates> ((int)EventType_Updates.UpdateOne, _data_updateOne);
		GameManager.Events.Broadcast<EventType_Updates> ((int)EventType_Updates.UpdateTwo, _data_updateTwo);
		//GameManager.Events.Broadcast<EventType_Updates> ((int)EventType_Updates.UpdateThree, _data_updateThree);
	}

	#endregion

}

