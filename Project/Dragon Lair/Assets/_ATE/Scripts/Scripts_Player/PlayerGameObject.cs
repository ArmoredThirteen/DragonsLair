using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;


public class PlayerGameObject : AteGameObject
{
	private FirstPersonController _firstPersonController;
	private float _start_walkSpeed;


	protected override void RegisterEvents ()
	{
		GameManager.Events.Register<EventType_Gameplay, EventData_Gameplay> ((int)EventType_Gameplay.ResetPlayerWalkSpeed, OnResetWalkSpeed);
		GameManager.Events.Register<EventType_Gameplay, EventData_Gameplay> ((int)EventType_Gameplay.SetPlayerWalkSpeed,   OnSetWalkSpeed);
	}

	protected override void UnregisterEvents ()
	{
		
	}


	protected override void AteAwake()
	{
		_firstPersonController = gameObject.GetComponent<FirstPersonController> () as FirstPersonController;
		_start_walkSpeed = _firstPersonController.WalkSpeed;
	}

	protected override void AteUpdate ()
	{
		
	}


	public void ResetWalkSpeed ()
	{
		_firstPersonController.WalkSpeed = _start_walkSpeed;
	}

	public void SetWalkSpeed (float newSpeed)
	{
		_firstPersonController.WalkSpeed = newSpeed;
	}


	private void OnResetWalkSpeed (EventData_Gameplay theData)
	{
		ResetWalkSpeed ();
	}

	private void OnSetWalkSpeed (EventData_Gameplay theData)
	{
		SetWalkSpeed (theData.WalkSpeed);
	}

}
