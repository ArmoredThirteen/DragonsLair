using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Ate.Spawning;

#if UNITY_EDITOR
using UnityEditor;
using Ate.EditorHelpers;
#endif


namespace Ate.Killing
{
	
	
	/// <summary>
	/// Data for each state's enabling/disabling of objects
	/// and any special FX spawning.
	/// </summary>
	[System.Serializable]
	public class KillableStateData
	{
		
		#region Public Variables

		public List<Spawner>    FXspawners       = new List<Spawner> ();
		public List<GameObject> objectsToEnable  = new List<GameObject> ();
		public List<GameObject> objectsToDisable = new List<GameObject> ();

		public bool drawList_FXspawners       = false;
		public bool drawList_objectsToEnable  = false;
		public bool drawList_objectsToDisable = false;

		#endregion


		#region Private Variables

		#endregion


		#if UNITY_EDITOR

		public void OnDrawInspector ()
		{
			EditorHelper.DrawResizableList<Spawner>
				("FX Spawners", ref drawList_FXspawners, ref FXspawners, DrawEntry_FXspawners);

			EditorGUILayout.Space ();

			EditorHelper.DrawResizableList<GameObject>
				("Objects to Enable", ref drawList_objectsToEnable, ref objectsToEnable, DrawEntry_ObjectsToEnable);

			EditorGUILayout.Space ();

			EditorHelper.DrawResizableList<GameObject>
				("Objects to Disable", ref drawList_objectsToDisable, ref objectsToDisable, DrawEntry_ObjectsToDisable);
		}

		private void DrawEntry_FXspawners (int index)
		{
			FXspawners[index] = EditorGUILayout.ObjectField
				("Spawner #"+index, FXspawners[index], typeof (GameObject), true)
				as Spawner;
		}

		private void DrawEntry_ObjectsToEnable (int index)
		{
			objectsToEnable[index] = EditorGUILayout.ObjectField
				("Object #"+index, objectsToEnable[index], typeof (GameObject), true)
				as GameObject;
		}

		private void DrawEntry_ObjectsToDisable (int index)
		{
			objectsToDisable[index] = EditorGUILayout.ObjectField
				("Object #"+index, objectsToDisable[index], typeof (GameObject), true)
				as GameObject;
		}

		#endif


		#region Public Methods

		#endregion


		#region Private Methods

		#endregion

	}//End Class
	
	
}//End Namespace
