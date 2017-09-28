﻿using UnityEngine;
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
	public class CollisionArea : AteComponent
	{
		
		#region Public Variables

		//TODO: Self-collision doesn't work predictably yet. Needs work before designers can use.
		/// <summary> Can colliders in the area can react to other colliders within the same area. </summary>
		private bool canAreaSelfCollide = false;
		/// <summary> Colliders hitting the area only send feedback about the first entered area collider. </summary>
		public bool collidersOnlyEnterOnFirst = true;
		/// <summary> Colliders leaving the area only send feedback about the last exited area collider. </summary>
		public bool collidersOnlyExitOnLast   = true;

		public bool areaIncludesSelf     = true;
		public bool areaIncludesChildren = false;
		public bool areaIncludesOthers   = false;

		/// <summary> Any non-self and non-children colliders to include in _areaColliders. </summary>
		public List<AteCollider> otherColliders = new List<AteCollider> ();

		#endregion


		#region Private Variables

		private List<AteCollider> _areaColliders     = new List<AteCollider> ();
		private List<AteCollider> _currentCollisions = new List<AteCollider> ();

		#endregion


		#if UNITY_EDITOR

		public override void DrawInspector ()
		{
			base.DrawInspector ();

			//canAreaSelfCollide = EditorGUILayout.Toggle ("Can Self Collide", canAreaSelfCollide);
			collidersOnlyEnterOnFirst = EditorGUILayout.Toggle ("Only Enter on First", collidersOnlyEnterOnFirst);
			collidersOnlyExitOnLast   = EditorGUILayout.Toggle ("Only Exit on Last",   collidersOnlyExitOnLast);

			DrawAreaColliderSettings ();
		}

		private void DrawAreaColliderSettings ()
		{
			EditorGUILayout.LabelField ("Area Collider Sources");
			EditorGUI.indentLevel++;

			areaIncludesSelf     = EditorGUILayout.Toggle ("Self",     areaIncludesSelf);
			areaIncludesChildren = EditorGUILayout.Toggle ("Children", areaIncludesChildren);
			areaIncludesOthers   = EditorGUILayout.Toggle ("Others",   areaIncludesOthers);

			if (areaIncludesOthers)
			{
				EditorGUI.indentLevel++;
				EditorHelper.DrawResizableList ("Other Colliders", ref otherColliders, DrawEntry_OtherCollider);
				EditorGUI.indentLevel--;
			}

			EditorGUI.indentLevel--;
		}

		private void DrawEntry_OtherCollider (int index)
		{
			otherColliders[index] = EditorGUILayout.ObjectField ("Collider", otherColliders[index], typeof(AteCollider), true) as AteCollider;
		}

		#endif


		#region GameObject

		private void OnDestroy ()
		{
			//TODO: Send event notifying things of our destruction
		}

		#endregion


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

		protected override void AteStart ()
		{
			FillAreaColliders ();
		}

		protected override void AteUpdate ()
		{
			
		}

		#endregion


		#region Public Methods

		#endregion


		#region Private Methods

		/// <summary>
		/// Uses areaIncludesXX settings to fill _areaColliders.
		/// </summary>
		private void FillAreaColliders ()
		{
			_areaColliders = new List<AteCollider> ();
			if (areaIncludesSelf)
			{
				AddAreaCollidersFromGameObject (this.gameObject);
			}

			if (areaIncludesChildren)
			{
				List<GameObject> children = this.gameObject.GetChildrenGameObjects ();
				for (int i = 0; i < children.Count; i++)
				{
					AddAreaCollidersFromGameObject (children[i]);
				}
			}

			if (areaIncludesOthers)
			{
				for (int i = 0; i < otherColliders.Count; i++)
				{
					if (otherColliders[i] == null)
						continue;

					//	Only add if not a duplicate
					if (!_areaColliders.Contains (otherColliders[i]))
						_areaColliders.Add (otherColliders[i]);
				}
			}
		}

		private void AddAreaCollidersFromGameObject (GameObject theObj)
		{
			if (theObj == null)
				return;
			
			List<AteCollider> theColliders = new List<AteCollider> (theObj.GetComponents<AteCollider> ());
			for (int i = 0; i < theColliders.Count; i++)
			{
				if (theColliders[i] == null)
					continue;

				//	Only add if not a duplicate
				if (!_areaColliders.Contains (theColliders[i]))
					_areaColliders.Add (theColliders[i]);
			}
		}


		/// <summary>
		/// Removes any nulls from _currentCollisions.
		/// </summary>
		private void CleanCurrentCollisions ()
		{
			for (int i = _currentCollisions.Count-1; i >= 0; i--)
			{
				if (_currentCollisions[i] == null)
				{
					_currentCollisions.RemoveAt (i);
					continue;
				}
			}
		}


		//TODO: Similarities with OnCollisionEnded() could be more modular
		private void OnCollisionBegan (EventData_Collision eventData)
		{
			CleanCurrentCollisions ();

			bool colOneIsInArea = _areaColliders.Contains (eventData.ColliderOne);
			bool colTwoIsInArea = _areaColliders.Contains (eventData.ColliderTwo);

			if (!IsValidCollision (colOneIsInArea, colTwoIsInArea))
				return;
			
			AteCollider ourCollider     = colOneIsInArea ? eventData.ColliderOne : eventData.ColliderTwo;
			AteCollider hittingCollider = colTwoIsInArea ? eventData.ColliderOne : eventData.ColliderTwo;

			bool isFirstCollider = !_currentCollisions.Contains (hittingCollider);
			_currentCollisions.Add (hittingCollider);

			if (isFirstCollider || !collidersOnlyEnterOnFirst)
				SendCollisionAreaBegan (ourCollider, hittingCollider);
		}

		private void OnCollisionEnded (EventData_Collision eventData)
		{
			CleanCurrentCollisions ();

			bool colOneIsInArea = _areaColliders.Contains (eventData.ColliderOne);
			bool colTwoIsInArea = _areaColliders.Contains (eventData.ColliderTwo);

			if (!IsValidCollision (colOneIsInArea, colTwoIsInArea))
				return;

			AteCollider ourCollider     = colOneIsInArea ? eventData.ColliderOne : eventData.ColliderTwo;
			AteCollider hittingCollider = colOneIsInArea ? eventData.ColliderTwo : eventData.ColliderOne;

			_currentCollisions.Remove (hittingCollider);
			bool isLastCollider = !_currentCollisions.Contains (hittingCollider);


			if (isLastCollider || !collidersOnlyExitOnLast)
				SendCollisionAreaEnded (ourCollider, hittingCollider);
		}

		/// <summary>
		/// Checks if the collision is within the area and if any self-collisions are allowed.
		/// </summary>
		private bool IsValidCollision (bool colOneIsInArea, bool colTwoIsInArea)
		{
			//	Collision has nothing to do with this area
			if (!colOneIsInArea && !colTwoIsInArea)
				return false;

			//	Self-collision when not allowed
			if (!canAreaSelfCollide && colOneIsInArea && colTwoIsInArea)
				return false;

			return true;
		}


		private void SendCollisionAreaBegan (AteCollider ourCollider, AteCollider hittingCollider)
		{
			#if DebugEnter
			DebugLog.Simple ("<color=green>Collision Began</color>\r\nCol One: ", ourCollider, "Col Two: ", hittingCollider);
			#endif

			EventData_Collision eventData = new EventData_Collision ();
			eventData.FullCollisionArea = this;
			eventData.AreaCollider      = ourCollider;
			eventData.HittingCollider   = hittingCollider;

			GameManager.Events.Broadcast<EventType_Collision> ((int)EventType_Collision.AreaCollisionBegan, eventData);
		}

		private void SendCollisionAreaEnded (AteCollider ourCollider, AteCollider hittingCollider)
		{
			#if DebugExit
			DebugLog.Simple ("<color=red>Collision Ended</color>\r\nCol One: ", ourCollider, "Col Two: ", hittingCollider);
			#endif

			EventData_Collision eventData = new EventData_Collision ();
			eventData.FullCollisionArea = this;
			eventData.AreaCollider      = ourCollider;
			eventData.HittingCollider   = hittingCollider;

			GameManager.Events.Broadcast<EventType_Collision> ((int)EventType_Collision.AreaCollisionEnded, eventData);
		}

		#endregion

	}//end class


}//end namespace
