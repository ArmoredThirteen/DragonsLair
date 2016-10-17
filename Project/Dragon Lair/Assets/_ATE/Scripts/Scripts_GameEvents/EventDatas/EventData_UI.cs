using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class EventData_UI : EventData
{
	public KeyCode theKey;
	public Vector3 clickedScreenCoords = new Vector3 ();
	public Object clickedObject = null;


	public EventData_UI ()
	{
		
	}

	public EventData_UI (KeyCode theKey, Vector3 clickedScreenCoords)
	{
		this.theKey = theKey;
		this.clickedScreenCoords = clickedScreenCoords;
	}

}

