using UnityEngine;
using System.Collections;


public class Player_FireOnClick : AteGameObject
{
	public float fireSpeed = 1;
	public ProjectileShooter defaultShooter;

	private bool isMousePressed = false;


	protected override void AteUpdate ()
	{
		
	}


	protected override void RegisterEvents ()
	{
		GameManager.Events.Register<EventType_UI, EventData_UI> ((int)EventType_UI.Clicked,  OnMouseClicked);
		GameManager.Events.Register<EventType_UI, EventData_UI> ((int)EventType_UI.Released, OnMouseReleased);
	}

	protected override void UnregisterEvents ()
	{
		GameManager.Events.Unregister<EventType_UI, EventData_UI> ((int)EventType_UI.Clicked,  OnMouseClicked);
		GameManager.Events.Unregister<EventType_UI, EventData_UI> ((int)EventType_UI.Released, OnMouseReleased);
	}


	private void OnMouseClicked (EventData_UI data)
	{
		isMousePressed = true;
	}

	private void OnMouseReleased (EventData_UI data)
	{
		isMousePressed = false;
	}

}
