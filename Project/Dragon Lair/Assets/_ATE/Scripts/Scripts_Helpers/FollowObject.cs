using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif


public class FollowObject : AteGameObject
{
	public Transform transformToFollow;
	public Vector3 followOffset;

	private Vector3 _lastToFollowPosition;


	#if UNITY_EDITOR

	public override void DrawInspector ()
	{
		base.DrawInspector ();

		transformToFollow = EditorGUILayout.ObjectField ("Transform to Follow", transformToFollow, typeof(Transform), true) as Transform;
	}

	#endif


	protected override void AteAwake ()
	{
		StartFollowing (transformToFollow);
	}

	protected override void AteUpdate ()
	{
		
	}

	//protected override void AteLateUpdate ()
	void LateUpdate ()
	{
		if (transformToFollow == null)
			return;

		if (transformToFollow.position != _lastToFollowPosition)
			MoveToFollow ();
	}


	/// <summary>
	/// Starts following given transform.
	/// Can be null, in which case scipt stops following things.
	/// </summary>
	public void StartFollowing (Transform theTransform)
	{
		transformToFollow = theTransform;
		if (transformToFollow == null)
			return;
		
		_lastToFollowPosition = transformToFollow.position;
		followOffset = _lastToFollowPosition.GetDir_To (Position);
	}

	private void MoveToFollow ()
	{
		Position = transformToFollow.position + followOffset;
	}

}
