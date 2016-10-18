using UnityEngine;
using System.Collections;


public class RotateObject : MonoBehaviour
{
	public Transform theObject;
	public float rotateSpeedX = 0;
	public float rotateSpeedY = 0;
	public float rotateSpeedZ = 0;


	void FixedUpdate ()
	{
		if (theObject == null)
			return;

		float delta = Time.fixedDeltaTime;
		float rotX = rotateSpeedX * delta;
		float rotY = rotateSpeedY * delta;
		float rotZ = rotateSpeedZ * delta;

		theObject.Rotate (rotX, rotY, rotX);
	}

}
