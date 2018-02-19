using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Ate.Pooling;


namespace Ate.GameSystems
{


	/// <summary>
	/// Keeps lists of pooled objects of different types.
	/// Creates new default objects if pool is empty and
	/// a new object is requested.
	/// </summary>
	public class PoolManager : GameSystem
	{

		[System.Serializable]
		public class PoolData
		{
			public PoolID theID = PoolID.None;

			public Transform      poolContainer = null;
			public PoolableObject defaultPrefab = null;

			public int startSize = 5;
			public int maxSize   = 50;


			public List<PoolableObject> pooledObjects = new List<PoolableObject> ();
		}


		#region Public Variables

		public List<PoolData> poolDatas = new List<PoolData> ();

		#endregion


		#region Private Variables

		private Dictionary<PoolID,PoolData> thePools = new Dictionary<PoolID, PoolData> ();

		#endregion


		#region GameSystem

		public override void Initialize ()
		{
			
		}

		public override void SceneInitialize ()
		{
			thePools.Clear ();

			for (int i = 0; i < poolDatas.Count; i++)
			{
				if (poolDatas[i].theID == PoolID.None)
				{
					#if UNITY_EDITOR
					Debug.LogError ("PoolManager has a PoolData with an ID of 'None'.\r\n" +
						"Pools and Poolable objects need to specify an ID.\r\n" +
						"This entry is being ignored and will not function.");
					#endif
					continue;
				}

				for (int k = 0; k < poolDatas[i].startSize; k++)
				{
					poolDatas[i].pooledObjects.Add (InstantiateDefaultObject (poolDatas[i]));
				}

				thePools.Add (poolDatas[i].theID, poolDatas[i]);
			}
		}


		public override void SystemUpdate (){}
		public override void SystemLateUpdate (){}

		#endregion


		#region GameObject

		/*public void Awake ()
		{
			RegisterEvents ();
		}

		public void Start ()
		{
			
		}

		public void OnDestroy ()
		{
			UnregisterEvents ();
		}*/

		#endregion


		#region Public Methods

		public void PoolObject (PoolableObject theObject)
		{
			PoolData thePool = GetPoolByID (theObject.theID);

			if (thePool.pooledObjects.Count > thePool.maxSize)
			{
				GameObject.Destroy (theObject.gameObject);
				return;
			}

			theObject.Pool ();
			thePool.pooledObjects.Add (theObject);
		}

		public PoolableObject UnpoolObject (PoolID theID)
		{
			PoolData thePool = GetPoolByID (theID);

			PoolableObject theObject = null;

			if (thePool.pooledObjects.Count == 0)
			{
				theObject = InstantiateDefaultObject (thePool);
			}
			else
			{
				theObject = thePool.pooledObjects[0];
				thePool.pooledObjects.RemoveAt (0);
			}

			theObject.Unpool ();
			return theObject;
		}

		#endregion


		#region Private Methods

		private PoolableObject InstantiateDefaultObject (PoolData thePool)
		{
			PoolableObject theObject =
				GameObject.Instantiate (thePool.defaultPrefab, transform.position, transform.rotation)
				as PoolableObject;

			Transform newParent = transform;
			if (thePool.poolContainer != null)
				newParent = thePool.poolContainer;
			theObject.transform.parent = newParent;

			theObject.Pool ();

			return theObject;
		}

		private PoolData GetPoolByID (PoolID theID)
		{
			for (int i = 0; i < poolDatas.Count; i++)
			{
				if (theID == poolDatas[i].theID)
				{
					return poolDatas[i];
				}
			}

			#if UNITY_EDITOR
			Debug.LogError ("PoolManager attempted to GetPoolByID using an ID that is not in the poolDatas.");
			#endif
			return null;
		}

		#endregion


	}//End Class


}//End Namespace
