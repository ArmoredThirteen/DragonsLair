using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif


namespace Ate
{


	public class PlatformGenerator_Patrol_Direct : AteComponent
	{
		public bool isPatrolling = true;

		public bool platformIsPrefab = true;
		public GameObject platformObject;
		public Transform platformParent = null;

		public Transform endNode;
		public float platformSpeed = 5.0f;


		private Transform _thePlatform;

		private float _timeSinceStart = 0;


		#if UNITY_EDITOR

		public override void DrawInspector()
		{
			base.DrawInspector ();

			isPatrolling = EditorGUILayout.Toggle ("Is Patrolling", isPatrolling);

			platformIsPrefab = EditorGUILayout.Toggle ("Platform is Prefab", platformIsPrefab);
			platformObject = EditorGUILayout.ObjectField ("Platform Object", platformObject, typeof(GameObject), true) as GameObject;
			platformParent = EditorGUILayout.ObjectField ("Platform Parent", platformParent, typeof(Transform), true) as Transform;

			endNode = EditorGUILayout.ObjectField ("Platform Parent", endNode, typeof(Transform), true) as Transform;
			platformSpeed = EditorGUILayout.FloatField ("Platform Speed", platformSpeed);
		}

		#endif

		
		protected override void AteStart ()
		{
			GameObject newPlatform = platformObject;
			if (platformIsPrefab)
				newPlatform = (GameObject)Instantiate (platformObject, transform.position, transform.rotation);

			if (newPlatform == null)
				return;

			_thePlatform = newPlatform.transform;
			_thePlatform.parent = platformParent;

			if (isPatrolling)
				RequestPatrolStart ();
		}


		protected override void AteUpdate ()
		{

		}


		void FixedUpdate ()
		{
			if (!isPatrolling)
				return;
			if (_thePlatform == null)
				return;

			_timeSinceStart += Time.fixedDeltaTime;

			// Get direction and speed of platform
			Vector3 dir = endNode.position - transform.position;
			Vector3 dirAway = transform.position - endNode.position;
			float distance = Vector3.Distance (transform.position, endNode.transform.position);
			float speedActual = platformSpeed/distance * _timeSinceStart;

			// Move the platform
			_thePlatform.position = transform.position + (dir + dirAway*Mathf.Cos (speedActual)) / 2;
		}


		public void RequestPatrolStart (bool resetTimeSinceStart = true)
		{
			if (isPatrolling)
				return;
			isPatrolling = true;

			if (resetTimeSinceStart)
				_timeSinceStart = 0;
		}

		public void RequestPatrolEnd ()
		{
			if (!isPatrolling)
				return;
			isPatrolling = false;
		}

	}//End Class


}//End Namespace
