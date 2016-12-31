using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif


public class ControlPosition : AteGameObject
{
	public enum ControlType
	{
		None = 0,

		SetWorld    = 10,
		LocalFollow = 20,
	}


	[System.Serializable]
	public class AxisPositionControl
	{
		public ControlType control = ControlType.None;

		public float setWorldVal;

		public Transform localFollowTarget = null;
		public float     localFollowOffset;
	}


	public AxisPositionControl controlX = new AxisPositionControl ();
	public AxisPositionControl controlY = new AxisPositionControl ();
	public AxisPositionControl controlZ = new AxisPositionControl ();

	private Vector3 _startPos;


	#if UNITY_EDITOR

	public override void DrawInspector ()
	{
		base.DrawInspector ();

		DrawControl ("Control X", controlX);
		DrawControl ("Control Y", controlY);
		DrawControl ("Control Z", controlZ);

		EditorHelper.SetDirtyIfChanged (this);
	}

	private void DrawControl (string controlTitle, AxisPositionControl theControl)
	{
		EditorGUILayout.Space ();

		EditorGUILayout.LabelField (controlTitle);
		EditorGUI.indentLevel++;

		theControl.control = (ControlType)EditorGUILayout.EnumPopup ("Control Type", theControl.control);

		switch (theControl.control)
		{
			case ControlType.SetWorld :
				DrawControl_SetWorld (theControl);
				break;
			case ControlType.LocalFollow :
				DrawControl_LocalFollow (theControl);
				break;
		}

		EditorGUI.indentLevel--;
	}

	private void DrawControl_SetWorld (AxisPositionControl theControl)
	{
		theControl.setWorldVal = EditorGUILayout.FloatField ("Set World Position", theControl.setWorldVal);
	}

	private void DrawControl_LocalFollow (AxisPositionControl theControl)
	{
		theControl.localFollowTarget = EditorGUILayout.ObjectField ("Transform to Follow", theControl.localFollowTarget, typeof(Transform), true) as Transform;
		theControl.localFollowOffset = EditorGUILayout.FloatField  ("Set Offset Amount",   theControl.localFollowOffset);
	}

	#endif


	protected override void AteAwake ()
	{
		StartFollowing ();
	}

	private void StartFollowing ()
	{
		_startPos = Position;
	}


	protected override void AteUpdate ()
	{
		
	}

	protected override void AteLateUpdate ()
	{
		float targXPos = controlX.localFollowTarget == null ? _startPos.x : controlX.localFollowTarget.position.x;
		float targYPos = controlY.localFollowTarget == null ? _startPos.y : controlY.localFollowTarget.position.y;
		float targZPos = controlZ.localFollowTarget == null ? _startPos.z : controlZ.localFollowTarget.position.z;

		float newX = GetNewCord (controlX, Position.x, targXPos);
		float newY = GetNewCord (controlY, Position.y, targYPos);
		float newZ = GetNewCord (controlZ, Position.z, targZPos);

		Position = new Vector3 (newX, newY, newZ);
	}


	private float GetNewCord (AxisPositionControl theControl, float curVal, float targPos)
	{
		float newVal = curVal;

		switch (theControl.control)
		{
			case ControlType.SetWorld :
				newVal = theControl.setWorldVal;
				break;
			case ControlType.LocalFollow :
				newVal = targPos + theControl.localFollowOffset;
				break;
		}

		return newVal;
	}
	
}