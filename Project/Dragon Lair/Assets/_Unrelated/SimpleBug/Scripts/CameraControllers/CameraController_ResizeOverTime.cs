using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif


public class CameraController_ResizeOverTime : CameraController
{
	public bool resizeImmediately = false;
	public float resizeDuration = 5;
	public float targetSize = 10;


	private bool _isResizing;
	private float _startSize;

	private float _timer_resizing;


	#if UNITY_EDITOR

	public override void DrawInspector ()
	{
		base.DrawInspector();

		resizeImmediately = EditorGUILayout.Toggle     ("Resize Immediately", resizeImmediately);
		resizeDuration    = EditorGUILayout.FloatField ("Resize Duration",    resizeDuration);

		targetSize = EditorGUILayout.FloatField ("Target Size", targetSize);
	}

	#endif


	protected override void AteStart ()
	{
		if (resizeImmediately)
			BeginResizing ();
	}

	protected override void AteUpdate ()
	{
		if (!_isResizing)
			return;

		_timer_resizing += Time.deltaTime;
		float timerRatio = _timer_resizing / resizeDuration;

		theCam.orthographicSize = Mathf.Lerp (_startSize, targetSize, timerRatio);
	}


	public void BeginResizing ()
	{
		if (_isResizing)
			return;

		_startSize = theCam.orthographicSize;
		_timer_resizing = 0;
		_isResizing = true;
	}

}
