using UnityEngine;
using System.Collections;
using UnityEditor;
using Ate.EditorHelpers;


namespace Ate
{


	[CustomEditor (typeof(AteComponent), true)]
	public class AteComponentEditor : Editor
	{
		private AteComponent _target;


		public override void OnInspectorGUI ()
		{
			_target = (AteComponent)target;

			_target.DrawInspector ();

			EditorHelper.SetDirtyIfChanged (_target);
		}

	}//End Class


}//End Namespace
