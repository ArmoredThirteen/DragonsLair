using UnityEngine;
using System.Collections;

public class PlatformGenerator_Patrol_Direct : MonoBehaviour {
	public GameObject platformObject;
	private GameObject thePlatform;

	public Transform endNode;

	public float platformSpeed = 5.0f;

	
	// Use this for initialization
	void Start () {
		// create and child the platform
		thePlatform = (GameObject)Instantiate(platformObject, transform.position, transform.rotation);
		thePlatform.transform.parent = transform;
	}


	// Update is called once per frame
	void Update () {

	}


	void FixedUpdate() {
		// get direction and speed of platform
		Vector3 dir = endNode.position - transform.position;
		float distance = Vector3.Distance(transform.position, endNode.transform.position);
		float speedActual = platformSpeed/distance * Time.time;

		// move the platform
		thePlatform.transform.position = transform.position + (dir + dir*Mathf.Sin(speedActual))/2;
	}
	
}
