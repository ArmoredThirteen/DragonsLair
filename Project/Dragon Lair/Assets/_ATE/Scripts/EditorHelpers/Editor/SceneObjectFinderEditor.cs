using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;


#if UNITY_EDITOR

[CustomEditor(typeof(SceneObjectFinder))]
public class SceneObjectFinderEditor : Editor
{
	private SceneObjectFinder _target;

	private static bool _foldout_foundObjects = true;
	private static bool _foldout_modifyObjects = true;
	private static bool _foldout_currentModifications = false;

	private delegate void ObjectModification (GameObject theObject);


	public override void OnInspectorGUI ()
	{
		base.OnInspectorGUI ();
		_target = (SceneObjectFinder)target;

		if (GUILayout.Button ("Find Objects"))
		{
			FindObjects ();
		}

		int totalFound = _target.lastFoundObjects == null ? 0 : _target.lastFoundObjects.Count;
		string foundCaption = "Found " + totalFound + " Objects:";
		string modsCaption = "Programmer's Modify Object";

		//EditorHelper.DrawInspectorSectionFoldout (ref _foldout_foundObjects, foundCaption, DrawFoundObjects);
		//EditorHelper.DrawInspectorSectionFoldout (ref _foldout_modifyObjects, modsCaption, DrawModifications);
		EditorGUILayout.Space ();
		EditorGUILayout.LabelField (foundCaption);
		DrawFoundObjects ();
		EditorGUILayout.Space ();
		EditorGUILayout.LabelField (modsCaption);
		DrawModifications ();
		EditorGUILayout.Space ();

		if (GUI.changed)
			EditorUtility.SetDirty(_target);
	}


	private void DrawFoundObjects ()
	{
		if (_target.lastFoundObjects == null)
			return;
		if (_target.lastFoundObjects.Count <= 0)
			return;

		for (int i = 0; i < _target.lastFoundObjects.Count; i++)
		{
			GameObject curTarg = _target.lastFoundObjects[i];
			if (curTarg == null)
				continue;

			EditorGUILayout.BeginHorizontal ();

			//	First press selects object, second press focuses on object
			if (GUILayout.Button ("Sel", GUILayout.Width (35)))
			{
				if (Selection.activeGameObject != curTarg)
					Selection.activeGameObject = curTarg;
				else
				{
					SceneView.lastActiveSceneView.FrameSelected ();
				}
			}

			//	Modify just this object
			if (GUILayout.Button ("Mod", GUILayout.Width (35)))
			{
				ModifyObject (
					curTarg
					//, ObjMod_AddNewChildBoxCollider
					//, ObjMod_TurnChildBoxCollidersIntoComponents
					//, ObjMod_DestroyChildBoxColliderObjects
					//, ObjMod_DestroyMeshCollider
					, ObjMod_AddAteObject
					);
				EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
			}
			//	Modify just this object
			/*if (GUILayout.Button ("Mod2", GUILayout.Width (35)))
			{
				ModifyObject (
					curTarg
					//, ObjMod_AddNewChildBoxCollider
					//, ObjMod_TurnChildBoxCollidersIntoComponents
					//, ObjMod_DestroyChildBoxColliderObjects
					, ObjMod_DestroyMeshCollider
					);
				EditorApplication.MarkSceneDirty ();
			}*/

			if (Selection.activeGameObject == null ||
			   (Selection.activeGameObject != curTarg && Selection.activeGameObject.transform.parent != curTarg.transform))
				EditorGUILayout.LabelField (curTarg.name);
			else
			{
				EditorGUILayout.LabelField (curTarg.name);
			}

			EditorGUILayout.EndHorizontal ();
		}
	}


	private void DrawModifications ()
	{
		EditorGUILayout.LabelField ("Programmer-specific way to modify all the most recent Found Objects. " +
		                            "Could literally be anything, don't press unless you know FOR SURE what it does.");
		
		//EditorHelper.DrawInspectorSectionFoldout (ref _foldout_currentModifications, "Current Modifications", DrawCurrentModifications);
		EditorGUILayout.LabelField ("Current Modifications");
		DrawCurrentModifications ();
		EditorGUILayout.Space ();
		
		if (GUILayout.Button ("Modify Objects", GUILayout.Width (200)))
		{
			ModifyLastFoundObjects ();
		}
	}


	#region Finding Objects

	private void FindObjects ()
	{
		if (_target == null)
			return;
		
		_target.lastFoundObjects = _target.FindObjectsByConditions (
			ObjCond_MatchesObjectType
			//, ObjCond_MatchesLayerMask
			//, ObjCond_HasMeshCollider
			//, ObjCond_NotHasMeshCollider
			);
	}


	private bool ObjCond_MatchesObjectType (GameObject theObject)
	{
		switch (_target.objectType)
		{
			case SceneObjectFinder.ObjectType.All:
				return true;

			case SceneObjectFinder.ObjectType.AteObject:
				return GameObjectHasComponent<AteObject> (theObject);
			case SceneObjectFinder.ObjectType.AteComponent:
				return GameObjectHasComponent<AteComponent> (theObject);

			default:
				return false;
		}
	}

	private bool GameObjectHasComponent<T> (GameObject theObject) where T: Component
	{
		if (theObject == null)
			return false;

		return theObject.GetComponent<T> () as T;
	}

	/// <summary>
	/// Returns true if theObject's layer matches a layer in _target.theLayers.
	/// 
	/// Shamelessly stolen from Rafes:
	/// http://answers.unity3d.com/questions/150690/using-a-bitwise-operator-with-layermask.html
	/// </summary>
	private bool ObjCond_MatchesLayerMask (GameObject theObject)
	{
		if (theObject == null)
			return false;

		//	Convert object's layer to bitfield(?) for comparison
		int objectMask = (1 << theObject.layer);
		
		if ((_target.theLayers.value & objectMask) > 0)
			return true;
		
		return false;
	}

	/// <summary>
	/// Returns true if theObject has a mesh collider.
	/// </summary>
	private bool ObjCond_HasMeshCollider (GameObject theObject)
	{
		if (theObject == null)
			return false;

		MeshCollider theCollider = theObject.GetComponent<MeshCollider>() as MeshCollider;
		return theCollider != null;
	}

	/// <summary>
	/// Returns true if theObject does not have a mesh collider.
	/// </summary>
	private bool ObjCond_NotHasMeshCollider (GameObject theObject)
	{
		return !ObjCond_HasMeshCollider (theObject);
	}


	/// <summary>
	/// Returns true if any of the given loot's drops match the given type.
	/// </summary>
	/*private bool IsLootOfType (ProcLoot theLoot, LootType theType)
	{
		for (int i = 0; i < theLoot.drops.Count; i++)
		{
			if (theLoot.drops[i].lootType == theType)
				return true;
		}

		return false;
	}*/

	#endregion


	//TODO: This should be its own thing somehow, hackily put here for convenience....
	//TODO: This should be its own thing somehow, hackily put here for convenience....
	//TODO: This should be its own thing somehow, hackily put here for convenience....
	#region Modifying Objects

	private void DrawCurrentModifications ()
	{
		EditorGUILayout.LabelField ("None, they're all disabled for now.");
	}
	
	private void ModifyLastFoundObjects ()
	{
		if (_target.lastFoundObjects == null)
			return;
		if (_target.lastFoundObjects.Count <= 0)
			return;

		for (int i = 0; i < _target.lastFoundObjects.Count; i++)
		{
			ModifyObject (
				_target.lastFoundObjects[i]
				//, ObjMod_AddNewChildBoxCollider
				//, ObjMod_TurnChildBoxCollidersIntoComponents
				//, ObjMod_DestroyMeshCollider
				, ObjMod_AddAteObject
				);
		}

	    EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
	}

	private void ModifyObject (GameObject theObject, params ObjectModification[] modifications)
	{
		if (theObject == null)
			return;

		for (int i = 0; i < modifications.Length; i++)
		{
			modifications[i] (theObject);
		}
	}


	/// <summary>
	/// Adds a new GameObject child to theObject and gives the child a box collider component.
	/// </summary>
	private void ObjMod_AddNewChildBoxCollider (GameObject theObject)
	{
		if (theObject == null)
			return;

		GameObject newObj = new GameObject ("ScriptMade Collider", typeof(BoxCollider));
		//GameObject newObj = new GameObject ("WalkableFix", typeof(BoxCollider));
		newObj.transform.parent = theObject.transform;
		newObj.transform.localPosition = new Vector3 (0, -0.5f, 0); // hack the offset
		//newObj.transform.Rotate (0, -45, 0);

		//newObj.layer = theObject.layer;

		//	See if theObject already had a collider
		//	If so we can make the new collider child roughly the same size
		Collider existingCollider = theObject.GetComponent<Collider>() as Collider;
		if (existingCollider != null)
		{
			BoxCollider newCollider = newObj.GetComponent<BoxCollider>() as BoxCollider;
			newCollider.size = existingCollider.bounds.size;
			//newCollider.size = new Vector3 (2.83f, 1, 2.83f);

			//	Produces weird results, hack worked using localPosition offset above
			//newCollider.center = existingCollider.bounds.center;
			//newCollider.transform.localPosition = existingCollider.bounds.center;
		}
	}

	/// <summary>
	/// Takes any child BoxColliders of theObject and adds its settings
	/// to a new component in theObject. Deletes the old child BoxColliders.
	/// </summary>
	private void ObjMod_TurnChildBoxCollidersIntoComponents (GameObject theObject)
	{
		if (theObject == null)
			return;
		
		List<BoxCollider> childColliders = new List<BoxCollider>(theObject.GetComponentsInChildren<BoxCollider>());
		if (childColliders == null)
			return;
		if (childColliders.Count <= 0)
			return;

		for (int i = 0; i < childColliders.Count; i++)
		{
			BoxCollider curCollider = childColliders[i];
			if (curCollider == null)
				continue;
			if (curCollider.gameObject == theObject)
				continue;

			BoxCollider newComponent = theObject.AddComponent<BoxCollider>() as BoxCollider;

			newComponent.size = curCollider.size;
			newComponent.center = curCollider.center + curCollider.transform.localPosition;

			GameObject.DestroyImmediate (curCollider.gameObject);
		}
	}

	/// <summary>
	/// Takes any child BoxColliders of theObject and, if it 
	/// has a BoxCollider it destroys the child object.
	/// </summary>
	private void ObjMod_DestroyChildBoxColliderObjects (GameObject theObject)
	{
		if (theObject == null)
			return;
		
		List<BoxCollider> childColliders = new List<BoxCollider>(theObject.GetComponentsInChildren<BoxCollider>());
		if (childColliders == null)
			return;
		if (childColliders.Count <= 0)
			return;
		
		for (int i = 0; i < childColliders.Count; i++)
		{
			BoxCollider curCollider = childColliders[i];
			if (curCollider == null)
				continue;
			if (curCollider.gameObject == theObject)
				continue;
			
			GameObject.DestroyImmediate (curCollider.gameObject);
		}
	}

	/// <summary>
	/// Destroys the first mesh collider on theObject.
	/// Won't destroy more than the first collider it finds.
	/// </summary>
	private void ObjMod_DestroyMeshCollider (GameObject theObject)
	{
		MeshCollider theCollider = theObject.GetComponent<MeshCollider>() as MeshCollider;
		if (theCollider == null)
			return;

		GameObject.DestroyImmediate (theCollider);
	}

	/// <summary>
	/// Adds an AteObject component to theObject.
	/// Also links up any AteComponents on theObject to use
	/// the new AteObject as an internal reference.
	/// If there is already an AteObject on theObject it
	/// will relink data but not make a new AteObject.
	/// </summary>
	private void ObjMod_AddAteObject (GameObject theObject)
	{
		//	Get current AteObject or make a new one
		AteObject ateObj = theObject.GetComponent<AteObject> () as AteObject;
		if (ateObj == null)
		{
			ateObj = theObject.AddComponent<AteObject> () as AteObject;
		}

		//	Set references between the AteObject and AteComponents
		ateObj.components.Clear ();
		AteComponent[] theComps = theObject.GetComponents<AteComponent> () as AteComponent[];
		if (theComps != null)
		{
			for (int i = 0; i < theComps.Length; i++)
			{
				if (theComps[i] == null)
					continue;
				ateObj.components.Add (theComps[i]);
				theComps[i].SetMyObject (ateObj);
			}
		}

		//	Move the AteObject up in the editor window to be at the top
		for (int i = 0; i < 10; i++)
		{
			UnityEditorInternal.ComponentUtility.MoveComponentUp (ateObj);
		}
	}

	#endregion

}

//	endif UNITY_EDITOR
#endif

