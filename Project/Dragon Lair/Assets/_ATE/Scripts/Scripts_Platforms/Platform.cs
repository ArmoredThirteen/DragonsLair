using UnityEngine;
using System.Collections;


public class Platform : MonoBehaviour {
	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
	
	}


	public void Death() {
		Landing[] allLandings = GetComponentsInChildren<Landing>();
		foreach(Landing tempLanding in allLandings)
			tempLanding.Death();
		
		GameObject.Destroy(gameObject);
	}
}
