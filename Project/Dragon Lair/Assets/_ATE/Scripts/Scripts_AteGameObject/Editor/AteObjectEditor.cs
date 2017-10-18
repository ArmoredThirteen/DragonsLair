using UnityEngine;
using System.Collections;
using UnityEditor;
using Ate.EditorHelpers;


namespace Ate
{


	[CustomEditor (typeof(AteObject), true)]
	public class AteObjectEditor : Editor
	{
		private AteObject _target;


		public override void OnInspectorGUI ()
		{
			_target = (AteObject)target;

			_target.DrawInspector ();

			EditorHelper.SetDirtyIfChanged (_target);
		}

	}//End Class


}//End Namespace
