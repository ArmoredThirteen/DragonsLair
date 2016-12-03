using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif


//TODO: 
//TODO: 
//TODO: Logic determining the orbit and the radius is getting hairy.
//TODO: Most of this class needs better optimization and seperation of powers.
//TODO: 
//TODO: 
public class FollowMouse_Orbit : AteGameObject
{
	public enum FacingType
	{
		None = 0,

		Toward = 100,
		Away   = 200,
	}


	#region Public Fields

	/// <summary> The camera to get screen coordinates from. </summary>
	public Camera theCam;

	/// <summary> The Transform that this object will orbit around. </summary>
	public Transform orbitObject;
	/// <summary> To control logic if/how it faces the orbitObject </summary>
	public FacingType faceObject = FacingType.None;

	/// <summary> If true, will attempt to align orbit with the mouse location. </summary>
	public bool followOrbit = true;
	/// <summary> How quickly an orbit is traversed. Works when toggling between set and dynamic angles. </summary>
	public float orbitSpeed = 1;
	/// <summary> Amount in degrees to offset the final _desiredAngle if followOrbit is true. </summary>
	public float angleOffset = 0;
	/// <summary> Amount in degrees to set _desiredAngle if followOrbit is false. </summary>
	public float setAngle = 0;

	/// <summary> If true, will attempt to match radius with the mouse location relative to the orbitObject. </summary>
	public bool followRadius = true;
	/// <summary> How quickly the radius changes if followRadius is true. </summary>
	public float radiusSpeed = 1;
	/// <summary> Min possible radius if followOrbit is true. </summary>
	public float minRadius = 2;
	/// <summary> Max possible radius if followOrbit is true. </summary>
	public float maxRadius = 4;
	/// <summary> The orbit radius to use if followRadius is false. </summary>
	public float setRadius = 2;

	#endregion


	#region Private Fields

	private bool _isFollowing = true;

	private float _currentAngle;
	private float _desiredAngle;

	private float _currentRadius;
	private float _desiredRadius;

	#endregion


	#if UNITY_EDITOR

	public override void DrawInspector ()
	{
		base.DrawInspector ();

		theCam = EditorGUILayout.ObjectField
			("Camera", theCam, typeof (Camera), true)
			as Camera;

		orbitObject = EditorGUILayout.ObjectField
			("Orbit Object", orbitObject, typeof(Transform), true)
			as Transform;

		faceObject = (FacingType)EditorGUILayout.EnumPopup ("Face Object", faceObject);

		EditorGUILayout.Space ();

		DrawOrbitSettings ();

		EditorGUILayout.Space ();

		DrawRadiusSettings ();
	}

	private void DrawOrbitSettings ()
	{
		followOrbit = EditorGUILayout.Toggle      ("Follow Orbit", followOrbit);
		orbitSpeed  = EditorGUILayout.FloatField  ("Orbit Speed",  orbitSpeed);

		if (followOrbit)
		{
			angleOffset = EditorGUILayout.FloatField ("Angle Offset", angleOffset);
		}
		else
		{
			setAngle = EditorGUILayout.FloatField ("Set Angle To", setAngle);
		}
	}

	private void DrawRadiusSettings ()
	{
		followRadius = EditorGUILayout.Toggle     ("Follow Radius", followRadius);
		radiusSpeed  = EditorGUILayout.FloatField ("Radius Speed",  radiusSpeed);

		if (followRadius)
		{
			minRadius   = EditorGUILayout.FloatField ("Min Radius",   minRadius);
			maxRadius   = EditorGUILayout.FloatField ("Max Radius",   maxRadius);
		}
		else
		{
			setRadius = EditorGUILayout.FloatField ("Set Radius To", setRadius);
		}
	}

	#endif


	protected override void AteAwake ()
	{
		#if UNITY_EDITOR
		if (theCam == null)
			Debug.LogError ("FollowMouse_Orbit has null theCam, script will not function.");
		if (orbitObject == null)
			Debug.LogError ("FollowMouse_Orbit has null orbitObject, script will not function.");
		#endif
	}

	protected override void AteUpdate ()
	{
		if (!_isFollowing)
			return;
		if (theCam == null)
			return;
		if (orbitObject == null)
			return;

		UpdateCurrentAngle ();
		UpdateDesiredAngle ();
		//DebugLog.Simple ("CurrentAngle: ", _currentAngle);
		//DebugLog.Simple ("DesiredAngle: ", _desiredAngle);

		UpdateCurrentRadius ();
		UpdateDesiredRadius ();

		Position = FindNextPosition ();

		UpdateFacing ();
	}


	/// <summary>
	/// Finds next position using Mathf.MoveTowardsAngle to determine
	/// the next speed-modified position from the current to desired
	/// angles. MoveTowardsAngle to stay along the orbit.
	/// </summary>
	private Vector3 FindNextPosition ()
	{
		float angle  = FindNextAngle ();
		float radius = FindNextRadius ();

		Vector3 nextPosition = Quaternion.AngleAxis (angle, Vector3.down) * (Vector3.right * radius);
		nextPosition = orbitObject.position + nextPosition;

		return nextPosition;
	}

	private float FindNextAngle ()
	{
		float angle = Mathf.MoveTowardsAngle (_currentAngle, _desiredAngle, orbitSpeed * Time.deltaTime);
		return angle;
	}

	private float FindNextRadius ()
	{
		float radius = Mathf.Lerp (_currentRadius, _desiredRadius, radiusSpeed * Time.deltaTime);
		return radius;
	}


	private void UpdateFacing ()
	{
		switch (faceObject)
		{
			case FacingType.None :
				break;
			case FacingType.Toward :
				MyTransform.LookAt (orbitObject);
				break;
			case FacingType.Away :
				MyTransform.LookAt (Position + Position.GetDir_AwayFrom (orbitObject.position));
				break;
		}
	}


	private void UpdateCurrentAngle ()
	{
		Vector3 vecToThis = orbitObject.position.GetDir_To (Position);

		_currentAngle = Mathf.Atan2 (vecToThis.z, vecToThis.x) * Mathf.Rad2Deg;
	}

	private void UpdateDesiredAngle ()
	{
		if (!followOrbit)
		{
			_desiredAngle = setAngle;
			return;
		}
		
		Vector3 vecToMouse = theCam.WorldToScreenPoint (orbitObject.position);
		vecToMouse = Input.mousePosition - vecToMouse;

		_desiredAngle = Mathf.Atan2 (vecToMouse.y, vecToMouse.x) * Mathf.Rad2Deg;
		_desiredAngle += angleOffset;
	}


	private void UpdateCurrentRadius ()
	{
		_currentRadius = Vector3.Distance (Position, orbitObject.position);
	}

	private void UpdateDesiredRadius ()
	{
		if (!followRadius)
		{
			_desiredRadius = setRadius;
			return;
		}

		Vector3 mousePos = theCam.ScreenToWorldPoint (Input.mousePosition);
		_desiredRadius = Vector3.Distance (mousePos.yOf (orbitObject.position.y), orbitObject.position);

		if (_desiredRadius < minRadius)
			_desiredRadius = minRadius;
		if (_desiredRadius > maxRadius)
			_desiredRadius = maxRadius;
	}

}
