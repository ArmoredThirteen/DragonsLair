
#if UNITY_EDITOR

using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace Ate
{


	public class SceneObjectFinder : MonoBehaviour
	{

		public enum ObjectType
		{
			All = 0,

			AteObject,
			AteComponent,
		}

		public ObjectType objectType = ObjectType.All;
		//public LootType lootType = LootType.Equipment;


		public LayerMask theLayers = new LayerMask ();
		public List<GameObject> lastFoundObjects {get; set;}

		public delegate bool ObjectCondition (GameObject theObject);


		public List<GameObject> FindObjectsByConditions (params ObjectCondition[] checkConditions)
		{
			List<ObjectCondition> conditions = new List<ObjectCondition> (checkConditions);

			List<GameObject> objects = new List<GameObject> (FindObjectsOfType<GameObject> ());

			for (int i = objects.Count-1; i >= 0; i--)
			{
				if (!ObjectMeetsConditions (objects[i], conditions))
				{
					objects.RemoveAt (i);
					continue;
				}
			}

			return objects;
		}


		private bool ObjectMeetsConditions (GameObject theObject, List<ObjectCondition> conditions)
		{
			for (int i = 0; i < conditions.Count; i++)
			{
				if (!conditions[i] (theObject))
					return false;
			}

			return true;
		}

	}//End Class


}//End Namespace

//	endif UNITY_EDITOR
#endif

