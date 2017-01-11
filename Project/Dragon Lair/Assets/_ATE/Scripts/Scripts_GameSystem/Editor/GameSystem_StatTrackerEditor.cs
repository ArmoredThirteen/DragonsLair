using UnityEngine;
using System.Collections;
using UnityEditor;


[CustomEditor (typeof(GameSystem_StatTracker))]
public class GameSystem_StatTrackerEditor : Editor
{
	private GameSystem_StatTracker _target;


	public override void OnInspectorGUI()
	{
		_target = (GameSystem_StatTracker)target;
		if (_target == null)
			return;

		_target.enumData.DrawInspector (true);
	}

}
