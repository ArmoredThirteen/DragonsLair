using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Landing : MonoBehaviour {
	// used for keeping track of velocity
	Vector3 oldPos;
	Vector3 landingVelocity;
	
		// Use this for initialization
	void Start () {
		// set the oldPos
		oldPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		// update the velocity information
		landingVelocity = (transform.position - oldPos) / Time.deltaTime;
		oldPos = transform.position;
	}


	private void OnTriggerEnter(Collider other) {
		// if it has a rigid body and no parent
		if(other.GetComponent<Rigidbody> () as Rigidbody != null && other.transform.parent == null)
		{
			// add the object
			other.transform.parent = transform;
		}
	}
	
	private void OnTriggerExit(Collider other) {
		// if the transform and landing share the same parent
		if(other.transform.parent == transform) {
			// remove the object
			other.transform.parent = null;
			other.GetComponent<Rigidbody> ().velocity += landingVelocity;
		}
	}


	public void Death() {
		// remove the childing of all the resting objects
		foreach(Transform child in transform) {
			child.parent = null;
			child.GetComponent<Rigidbody> ().velocity += landingVelocity;
		}
	}
}

