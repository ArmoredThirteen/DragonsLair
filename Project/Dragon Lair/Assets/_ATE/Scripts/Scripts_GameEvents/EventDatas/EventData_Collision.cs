using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CollisionSystem;
using Collider = CollisionSystem.Collider;


public class EventData_Collision : EventData
{
	private Collider _colOne;
	private Collider _colTwo;


	public Collider ColliderOne
	{
		get {return _colOne;}
		set {_colOne = value;}
	}

	public Collider ColliderTwo
	{
		get {return _colTwo;}
		set {_colTwo = value;}
	}
}

