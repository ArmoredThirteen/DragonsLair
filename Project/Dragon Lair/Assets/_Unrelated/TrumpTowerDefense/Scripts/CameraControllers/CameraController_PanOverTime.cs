using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif


public class CameraController_PanOverTime : CameraController
{
	public bool panImmediately = false;
	public float panDuration = 5;
	public Transform panTarget;


	private bool _isPanning;
	private Vector3 _startPosition;

	private float _timer_panning;


	#if UNITY_EDITOR

	public override void DrawInspector ()
	{
		base.DrawInspector();

		panImmediately = EditorGUILayout.Toggle     ("Pan Immediately", panImmediately);
		panDuration    = EditorGUILayout.FloatField ("Pan Duration",    panDuration);

		panTarget = EditorGUILayout.ObjectField
			("Pan Target", panTarget, typeof(Transform), true)
			as Transform;
	}

	#endif


	protected override void AteStart ()
	{
		if (panImmediately)
			BeginPanning ();
	}

	protected override void AteUpdate ()
	{
		if (!_isPanning)
			return;
		
		_timer_panning += Time.deltaTime;
		float timerRatio = _timer_panning / panDuration;

		theCam.transform.position = Vector3.Lerp (_startPosition, panTarget.position.yOf (theCam.transform.position.y), timerRatio);
	}


	public void BeginPanning ()
	{
		if (_isPanning)
			return;

		_startPosition = theCam.transform.position;
		_timer_panning = 0;
		_isPanning = true;
	}

}
