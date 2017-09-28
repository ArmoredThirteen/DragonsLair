using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class GameSystem_GameObjectTracker : GameSystem
{
	#region Field

	private Dictionary<GOType, List<AteObject>> _registeredAteObjects = new Dictionary<GOType, List<AteObject>> ();

	#endregion


	#region GameSystem

	public override void Initialize (){}

	public override void SceneInitialize ()
	{
		//TODO: Something more official, this seems bug-prone
		_registeredAteObjects = new Dictionary<GOType, List<AteObject>> ();
	}


	public override void SystemUpdate ()
	{
		/*foreach (KeyValuePair<GOType, List<AteGameObject>> entry in _registeredAteObjects)
		{
			Debug.Log ("Key: " + entry.Key.ToString () + " ||| Count: " + entry.Value.Count);
		}*/

		//Debug.Log ("None Count: " + GetAteGameObjects (GOType.None).Count);
		//Debug.Log ("Tile Count: " + GetAteGameObjects (GOType.Tile).Count);

		//Debug.Log ("Tile Count: " + GetAteGameObjects<Tile> (GOType.Tile).Count);
	}

	public override void SystemLateUpdate (){}

	#endregion


	#region Game Object Registration

	public void RegisterAteGameObject (AteObject theObject)
	{
		if (theObject == null)
			return;

		//	Add key if necessary
		if (!_registeredAteObjects.ContainsKey (theObject.type))
			_registeredAteObjects.Add (theObject.type, new List<AteObject> ());

		//	Add object if necessary
		if (!_registeredAteObjects[theObject.type].Contains (theObject))
			_registeredAteObjects[theObject.type].Add (theObject);
	}

	public void UnregisterAteGameObject (AteObject theObject)
	{
		if (theObject == null)
			return;

		//	Check if key exists
		if (!_registeredAteObjects.ContainsKey (theObject.type))
			return;

		//	Remove object if necessary
		if (_registeredAteObjects[theObject.type].Contains (theObject))
			_registeredAteObjects[theObject.type].Remove (theObject);

		//	Remove key if necessary
		if (_registeredAteObjects[theObject.type].Count <= 0)
			_registeredAteObjects.Remove (theObject.type);
	}

	#endregion


	/// <summary>
	/// Returns a list of all registered AteGameObjects that match given GOType.
	/// </summary>
	public List<AteObject> GetAteGameObjects (GOType theType)
	{
		List<AteObject> theObjects = new List<AteObject> ();

		if (!_registeredAteObjects.ContainsKey (theType))
			return theObjects;

		for (int i = 0; i < _registeredAteObjects[theType].Count; i++)
		{
			theObjects.Add (_registeredAteObjects[theType][i]);
		}

		return theObjects;
	}

	/// <summary>
	/// Returns a list of all registered AteGameObjects that match given GOType.
	/// Results are limited to classes matching given class type.
	/// </summary>
	public List<T> GetAteGameObjects<T> (GOType theType) where T : AteComponent
	{
		List<T> theObjects = new List<T> ();

		if (!_registeredAteObjects.ContainsKey (theType))
			return theObjects;

		for (int i = 0; i < _registeredAteObjects[theType].Count; i++)
		{
			T curObject = _registeredAteObjects[theType][i] as T;
			if (curObject != null)
				theObjects.Add (curObject);
		}

		return theObjects;
	}

}

