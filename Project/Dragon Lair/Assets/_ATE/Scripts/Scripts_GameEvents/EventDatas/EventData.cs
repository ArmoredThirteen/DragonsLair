using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public abstract class EventData
{
	/// <summary>
	/// History of who has sent this event. It is ultimately
	/// added to through EventSystem.Broadcast(). Due to the
	/// nature of game objects keep in mind that any of these
	/// entries could be null at any given time.
	/// </summary>
	public List<GameObject> senders = new List<GameObject> ();

	public float timestamp_firstSent = -1;
	public float timestamp_lastSent;


	/// <summary>
	/// Returns the first sender. Due to the nature of both
	/// GameObjects and the senders list, this could
	/// definitely come back as null.
	/// </summary>
	public GameObject FirstSender ()
	{
		if (senders.Count > 0)
			return senders[0];
		return null;
	}

	/// <summary>
	/// Returns the first found non-null sender.
	/// Returns null if sender list is empty.
	/// </summary>
	public GameObject FirstNonNullSender ()
	{
		for (int i = 0; i < senders.Count; i++)
		{
			if (senders[i] != null)
				return senders[i];
		}

		return null;
	}

	/// <summary>
	/// Returns a new list made from all the non-null items
	/// in this.senders. This probably produces some garbage
	/// but you won't have to worry about nulls. Due to the
	/// nature of GameObjects if you're caching this then
	/// the future list could have entries that become null.
	/// If you plan on doing this use this method's overload
	/// so that you can prevent a bit of garbage.
	/// </summary>
	public List<GameObject> NonNullSenders ()
	{
		List<GameObject> returnList = new List<GameObject> ();

		for (int i = 0; i < senders.Count; i++)
		{
			if (senders[i] != null)
				returnList.Add (senders[i]);
		}

		return returnList;
	}

	/// <summary>
	/// Returns a new list made from all the non-null items
	/// in this.senders. This probably produces some garbage
	/// but you won't have to worry about nulls. Due to the
	/// nature of GameObjects if you're caching this then
	/// the future list could have entries that become null.
	/// That is where this method works better than the one
	/// without a parameter that builds/returns a new list.
	/// </summary>
	public void NonNullSenders (ref List<GameObject> theList)
	{
		theList.Clear ();

		for (int i = 0; i < senders.Count; i++)
		{
			if (senders[i] != null)
				theList.Add (senders[i]);
		}
	}
}

