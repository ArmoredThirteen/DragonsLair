using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif


public class FollowMouse : AteGameObject
{
	public Camera theCam;

	public float moveSpeed = 1;

	public bool followX = true;
	public bool followY = true;


	private bool isFollowing = true;
	private float zPos;


	protected override void AteAwake ()
	{
		zPos = Position.z;
	}

	protected override void AteUpdate ()
	{
		if (!isFollowing)
			return;

		Vector3 mousePos = theCam.ScreenToWorldPoint (Input.mousePosition);
		mousePos = new Vector3 (
			(followX ? mousePos.x : Position.x),
			(followY ? mousePos.y : Position.y),
			zPos
		);

		transform.position = Vector2.Lerp (Position, mousePos, moveSpeed);
	}



	#if UNITY_EDITOR

	public override void DrawInspector ()
	{
		theCam = EditorGUILayout.ObjectField
			("Camera", theCam, typeof (Camera), true)
			as Camera;

		/*zPos = EditorGUILayout.FloatField ("Z Position", zPos);*/

		moveSpeed = EditorGUILayout.FloatField ("Move Speed", moveSpeed);

		followX = EditorGUILayout.Toggle ("Follow X", followX);
		followY = EditorGUILayout.Toggle ("Follow Y", followY);
	}

	#endif

}
