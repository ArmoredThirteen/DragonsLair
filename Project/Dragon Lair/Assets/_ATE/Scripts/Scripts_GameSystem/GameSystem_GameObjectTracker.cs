using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class GameSystem_GameObjectTracker : GameSystem
{
	#region Field

	private Dictionary<GOType, List<AteGameObject>> _registeredAteGameObjects = new Dictionary<GOType, List<AteGameObject>> ();

	#endregion


	#region GameSystem

	public override void Initialize (){}

	public override void SceneInitialize ()
	{
		//TODO: Something more official, this seems bug-prone
		_registeredAteGameObjects = new Dictionary<GOType, List<AteGameObject>> ();
	}


	public override void SystemUpdate ()
	{
		/*foreach (KeyValuePair<GOType, List<AteGameObject>> entry in _registeredAteGameObjects)
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

	public void RegisterAteGameObject (AteGameObject theObject)
	{
		if (theObject == null)
			return;

		//	Add key if necessary
		if (!_registeredAteGameObjects.ContainsKey (theObject.type))
			_registeredAteGameObjects.Add (theObject.type, new List<AteGameObject> ());

		//	Add object if necessary
		if (!_registeredAteGameObjects[theObject.type].Contains (theObject))
			_registeredAteGameObjects[theObject.type].Add (theObject);
	}

	public void UnregisterAteGameObject (AteGameObject theObject)
	{
		if (theObject == null)
			return;

		//	Check if key exists
		if (!_registeredAteGameObjects.ContainsKey (theObject.type))
			return;

		//	Remove object if necessary
		if (_registeredAteGameObjects[theObject.type].Contains (theObject))
			_registeredAteGameObjects[theObject.type].Remove (theObject);

		//	Remove key if necessary
		if (_registeredAteGameObjects[theObject.type].Count <= 0)
			_registeredAteGameObjects.Remove (theObject.type);
	}

	#endregion


	/// <summary>
	/// Returns a list of all registered AteGameObjects that match given GOType.
	/// </summary>
	public List<AteGameObject> GetAteGameObjects (GOType theType)
	{
		List<AteGameObject> theObjects = new List<AteGameObject> ();

		if (!_registeredAteGameObjects.ContainsKey (theType))
			return theObjects;

		for (int i = 0; i < _registeredAteGameObjects[theType].Count; i++)
		{
			theObjects.Add (_registeredAteGameObjects[theType][i]);
		}

		return theObjects;
	}

	/// <summary>
	/// Returns a list of all registered AteGameObjects that match given GOType.
	/// Results are limited to classes matching given class type.
	/// </summary>
	public List<T> GetAteGameObjects<T> (GOType theType) where T : AteGameObject
	{
		List<T> theObjects = new List<T> ();

		if (!_registeredAteGameObjects.ContainsKey (theType))
			return theObjects;

		for (int i = 0; i < _registeredAteGameObjects[theType].Count; i++)
		{
			T curObject = _registeredAteGameObjects[theType][i] as T;
			if (curObject != null)
				theObjects.Add (curObject);
		}

		return theObjects;
	}

}

