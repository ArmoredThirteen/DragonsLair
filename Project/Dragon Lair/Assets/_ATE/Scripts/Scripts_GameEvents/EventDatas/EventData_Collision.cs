using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CollisionSystem;
using Collider = CollisionSystem.AteCollider;


public class EventData_Collision : EventData
{
	private AteCollider _colOne;
	private AteCollider _colTwo;


	public AteCollider ColliderOne
	{
		get {return _colOne;}
		set {_colOne = value;}
	}

	public AteCollider ColliderTwo
	{
		get {return _colTwo;}
		set {_colTwo = value;}
	}
}

