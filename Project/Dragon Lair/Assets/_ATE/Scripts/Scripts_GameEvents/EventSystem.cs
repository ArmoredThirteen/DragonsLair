/*
 * Butchered from:
 * Advanced C# messenger by Ilya Suzdalnitski. V1.0
 * Based on Rod Hyde's "CSharpMessenger" and Magnus Wolffelt's "CSharpMessenger Extended".
 */

using System;
using System.Collections.Generic;
using UnityEngine;


//TODO: I believe this script suffers from boxing as explained here:
//TODO: http://www.somasim.com/blog/2015/08/c-performance-tips-for-unity-part-2-structs-and-enums/


public class EventSystem<EventType, EventDataType>	where EventType:		struct, IConvertible, IComparable
													where EventDataType:	EventData
{
	private Dictionary<int, Delegate> _eventTable = new Dictionary<int, Delegate> ();


	/// <summary>
	/// Registers a method to be called whenever the given event type
	/// is sent through a Broadcast() call.
	/// </summary>
	public void Register (EventType eventType, Callback<EventDataType> callback)
	{
		//TODO: Make it so receiver does something
		//		Perhaps add to a list for an "unregister all" option.
		//		For now it is really just here to ensure that
		//	people only try and register things from IEventUsers.

		int eventInt = Convert.ToInt32 (eventType);

		//	Add key if unexisting
		if (!_eventTable.ContainsKey (eventInt))
		{
			_eventTable.Add (eventInt, null);
		}

		//	And then add to our delegate
		_eventTable[eventInt] = (Callback<EventDataType>)_eventTable[eventInt] + callback;
	}


	public void Unregister (EventType eventType, Callback<EventDataType> callback)
	{
		int eventInt = Convert.ToInt32 (eventType);

		//	We have no key, so nothing to unregister from
		if (!_eventTable.ContainsKey (eventInt))
			return;

		_eventTable[eventInt] = (Callback<EventDataType>)_eventTable[eventInt] - callback;

		//	Remove key if unused
		if (_eventTable[eventInt] == null)
		{
			_eventTable.Remove (eventInt);
		}
	}


	//TODO: Delayed? Maybe not? Sounds dangerous to me
	/*public void Broadcast (float delay, EventType eventType, EventDataType eventData, GameObject sender = null)
	{
		
	}*/

	public void Broadcast (EventType eventType, EventDataType eventData, GameObject sender = null)
	{
		int eventInt = Convert.ToInt32 (eventType);

		if (!_eventTable.ContainsKey (eventInt))
			return;

		//	Add in our automatic event data
		if (eventData != null)
		{
			//	Sender history
			if (sender != null)
				eventData.senders.Add (sender);

			//	Timestamps
			if (eventData.timestamp_firstSent < 0)
				eventData.timestamp_firstSent = Time.time;
			eventData.timestamp_lastSent = Time.time;
		}

		//	Find and call the delegate
		Delegate d;
		if (_eventTable.TryGetValue (eventInt, out d))
		{
			Callback<EventDataType> callback = d as Callback<EventDataType>;
			
			if (callback != null)
			{
				callback(eventData);
			}
		}
	}

}

