using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class EventData_UI : EventData
{
	public int buttonID;
	public Vector3 clickedScreenCoords = new Vector3 ();
	public Object clickedObject = null;


	public EventData_UI ()
	{
		
	}

	public EventData_UI (int buttonID, Vector3 clickedScreenCoords)
	{
		this.buttonID = buttonID;
		this.clickedScreenCoords = clickedScreenCoords;
	}

}

