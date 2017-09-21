using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif


public abstract class CameraController : AteGameObject
{
	public Camera theCam;


	#if UNITY_EDITOR

	public override void DrawInspector ()
	{
		base.DrawInspector();

		theCam = EditorGUILayout.ObjectField
			("Camera", theCam, typeof (Camera), true)
			as Camera;
	}

	#endif


	/*protected override void AteUpdate ()
	{
		
	}*/

}
