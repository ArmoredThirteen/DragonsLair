using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif


namespace CollisionSystem
{
	

	/// <summary>
	/// A collection of AteColliders that mimics functionality of single colliders.
	/// </summary>
	public class CollisionArea : AteGameObject
	{
		
		#region Public Variables

		//TODO: Make private, and have it populate on Awake() or Start() using editor scripts
		//		Should be things like 'checkSelf', 'checkChildren', and 'checkOther'
		public List<AteCollider> collidersToCheck = new List<AteCollider> ();

		#endregion


		#region Private Variables

		private List<AteCollider> _currentCollisions = new List<AteCollider> ();

		#endregion


		#if UNITY_EDITOR

		public override void DrawInspector ()
		{
			base.DrawInspector ();
			EditorHelper.DrawResizableList ("Colliders to Check", ref collidersToCheck, DrawEntry_Collider);
		}

		private void DrawEntry_Collider (int index)
		{
			collidersToCheck[index] = EditorGUILayout.ObjectField ("Collider", collidersToCheck[index], typeof(AteCollider), true) as AteCollider;
		}

		#endif


		#region AteGameObject

		protected override void RegisterEvents ()
		{
			GameManager.Events.Register<EventType_Collision, EventData_Collision>
				((int)EventType_Collision.CollisionBegan, OnCollisionBegan);

			GameManager.Events.Register<EventType_Collision, EventData_Collision>
				((int)EventType_Collision.CollisionEnded, OnCollisionEnded);
		}

		protected override void UnregisterEvents ()
		{
			GameManager.Events.Unregister<EventType_Collision, EventData_Collision>
				((int)EventType_Collision.CollisionBegan, OnCollisionBegan);

			GameManager.Events.Unregister<EventType_Collision, EventData_Collision>
				((int)EventType_Collision.CollisionEnded, OnCollisionEnded);
		}

		protected override void AteUpdate ()
		{
			
		}

		#endregion


		#region Public Methods

		#endregion


		#region Private Methods

		private void OnCollisionBegan (EventData_Collision eventData)
		{
			bool colOneIsInArea = collidersToCheck.Contains (eventData.ColliderOne);
			bool colTwoIsInArea = collidersToCheck.Contains (eventData.ColliderTwo);

			//	Irrelevant collision
			if (!colOneIsInArea && !colTwoIsInArea)
			{
				return;
			}

			//	Self-collision
			if (colOneIsInArea && colTwoIsInArea)
			{
				return;
			}

			bool isFirstCollider = _currentCollisions.Count == 0;

			if (colOneIsInArea)
			{
				_currentCollisions.Add (eventData.ColliderTwo);
				if (isFirstCollider)
					SendCollisionAreaBegan (eventData.ColliderOne, eventData.ColliderTwo);
			}
			else if (colTwoIsInArea)
			{
				_currentCollisions.Add (eventData.ColliderOne);
				if (isFirstCollider)
					SendCollisionAreaBegan (eventData.ColliderTwo, eventData.ColliderOne);
			}
		}

		private void OnCollisionEnded (EventData_Collision eventData)
		{
			bool colOneIsInArea = collidersToCheck.Contains (eventData.ColliderOne);
			bool colTwoIsInArea = collidersToCheck.Contains (eventData.ColliderTwo);

			//	Irrelevant collision
			if (!colOneIsInArea && !colTwoIsInArea)
			{
				return;
			}

			//	Self-collision
			if (colOneIsInArea && colTwoIsInArea)
			{
				return;
			}

			bool isLastCollider = _currentCollisions.Count == 1;

			if (colOneIsInArea)
			{
				_currentCollisions.Remove (eventData.ColliderTwo);
				if (isLastCollider)
					SendCollisionAreaEnded (eventData.ColliderOne, eventData.ColliderTwo);
			}
			else if (colTwoIsInArea)
			{
				_currentCollisions.Remove (eventData.ColliderOne);
				if (isLastCollider)
					SendCollisionAreaEnded (eventData.ColliderTwo, eventData.ColliderOne);
			}
		}


		private void SendCollisionAreaBegan (AteCollider ourCollider, AteCollider hittingCollider)
		{
			DebugLog.Simple ("Collision Began, Col One: ", ourCollider, "Col Two: ", hittingCollider);
		}

		private void SendCollisionAreaEnded (AteCollider ourCollider, AteCollider hittingCollider)
		{
			DebugLog.Simple ("Collision Ended, Col One: ", ourCollider, "Col Two: ", hittingCollider);
		}

		#endregion

	}//end class


}//end namespace
