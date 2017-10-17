using UnityEngine;
using System.Collections;
using UnityEditor;


namespace Ate
{


	[CustomEditor (typeof(AteGameObject), true)]
	public class AteGameObjectEditor : Editor
	{
		private AteGameObject _target;


		public override void OnInspectorGUI ()
		{
			_target = (AteGameObject)target;

			_target.DrawInspector ();

			EditorHelper.SetDirtyIfChanged (_target);
		}

	}//End CLass


}//End Namespace
