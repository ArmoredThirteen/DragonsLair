using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif


public class FollowMouse_Linear : AteGameObject
{
	public Camera theCam;

	public float moveSpeed = 1;

	public bool followX = true;
	public bool followY = true;
	public bool followZ = true;


	private bool isFollowing = true;


	#if UNITY_EDITOR

	public override void DrawInspector ()
	{
		base.DrawInspector ();

		theCam = EditorGUILayout.ObjectField
			("Camera", theCam, typeof (Camera), true)
			as Camera;

		moveSpeed = EditorGUILayout.FloatField ("Move Speed", moveSpeed);

		followX = EditorGUILayout.Toggle ("Follow X", followX);
		followY = EditorGUILayout.Toggle ("Follow Y", followY);
		followZ = EditorGUILayout.Toggle ("Follow Z", followZ);
	}

	#endif


	protected override void AteAwake ()
	{
		
	}

	protected override void AteUpdate ()
	{
		if (!isFollowing)
			return;

		Vector3 mousePos = theCam.ScreenToWorldPoint (Input.mousePosition);

		mousePos = new Vector3 (
			(followX ? mousePos.x : Position.x),
			(followY ? mousePos.y : Position.y),
			(followZ ? mousePos.z : Position.z)
		);

		Position = Vector3.Lerp (Position, mousePos, moveSpeed * Time.deltaTime);
	}

}
