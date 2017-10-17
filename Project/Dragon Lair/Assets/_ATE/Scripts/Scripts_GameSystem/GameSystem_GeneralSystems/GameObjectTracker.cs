using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace Ate.GameSystems
{


	public class GameObjectTracker : GameSystem
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
			/*foreach (KeyValuePair<GOType, List<AteObject>> entry in _registeredAteObjects)
			{
				Debug.Log ("Key: " + entry.Key.ToString () + " ||| Count: " + entry.Value.Count);
			}*/

			//Debug.Log ("None Count: " + GetAteObjects (GOType.None).Count);
			//Debug.Log ("Tile Count: " + GetAteObjects (GOType.Tile).Count);

			//Debug.Log ("Tile Count: " + GetAteObjects<Tile> (GOType.Tile).Count);
		}

		public override void SystemLateUpdate (){}

		#endregion


		#region Game Object Registration

		public void RegisterAteObject (AteObject theObject)
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

		public void UnregisterAteObject (AteObject theObject)
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
		/// Returns a list of all registered AteObjects that match given GOType.
		/// </summary>
		public List<AteObject> GetAteObjects (GOType theType)
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
		/// Returns a list of all registered AteObjects that match given GOType.
		/// Results are limited to classes matching given class type.
		/// </summary>
		public List<T> GetAteObjects<T> (GOType theType) where T : AteComponent
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

	}//End Class


}//End Namespace
