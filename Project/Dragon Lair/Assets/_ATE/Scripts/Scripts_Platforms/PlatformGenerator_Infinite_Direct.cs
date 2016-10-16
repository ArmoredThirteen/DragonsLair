using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlatformGenerator_Infinite_Direct : MonoBehaviour {
	public GameObject platformObject;
	private List<GameObject> thePlatforms;

	public Transform endNode;
	private Transform startNode;

	public float platformSpeed = 5.0f;
	public float platformInterval = 1.0f;
	private float platformTimer;
	public bool inReverse = false;


	// Use this for initialization
	void Start () {
		// instantiate thePlatforms
		thePlatforms = new List<GameObject>();
		// set the timer
		platformTimer = Time.time -1;
	}


	// Update is called once per frame
	void Update () {

	}


	void FixedUpdate() {
		if(Time.time >= platformTimer) {
			// create platform
			GameObject newPlatform = null;
			if(inReverse)
				newPlatform = (GameObject)Instantiate(platformObject, endNode.position, endNode.rotation);
			else
				newPlatform = (GameObject)Instantiate(platformObject, transform.position, transform.rotation);

			newPlatform.transform.parent = transform;
			// add it to the platform list
			thePlatforms.Add(newPlatform);

			// reset timer
			platformTimer = Time.time + platformInterval;
		}

		// instantiate for possible deletion
		GameObject deletePlatform = null;

		// for every platform
		foreach(GameObject tempPlatform in thePlatforms) {
			// get direction and speed of platform
			Vector3 dir = endNode.position - transform.position;
			// modify if in reverse
			if(inReverse)
				dir = transform.position - endNode.position;

			// move the platform
			tempPlatform.transform.position += Vector3.ClampMagnitude(dir, platformSpeed/100);

			// mark the platform as deletable
			if((Vector3.Distance(tempPlatform.transform.position, endNode.position) <= 0.05f && !inReverse) ||
			   		(Vector3.Distance(tempPlatform.transform.position, transform.position) < 0.1f && inReverse)) {
				deletePlatform = tempPlatform;
			}
		}

		// delete the marked platform
		if(deletePlatform != null) {
			thePlatforms.Remove(deletePlatform);
			deletePlatform.SendMessage("Death");
		}
	}
	
}


