using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class EventData_Updates : EventData
{
	public int updateIndex;

	public EventData_Updates (int updateIndex)
	{
		this.updateIndex = updateIndex;
	}
}

