using UnityEngine;
using System.Collections;
using UnityEditor;
using Ate.EditorHelpers;


namespace Ate.GameText
{


	[CustomEditor (typeof(SpriteAlphabet), true)]
	public class AteComponentEditor : Editor
	{
		private SpriteAlphabet _target;


		public override void OnInspectorGUI ()
		{
			_target = (SpriteAlphabet)target;

			_target.DrawInspector ();

			EditorHelper.SetObjectDirtyIfChanged (_target);
		}

	}//End Class


}//End Namespace
