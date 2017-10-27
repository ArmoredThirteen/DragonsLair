using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Ate.GameSystems;
using Collider = Ate.Collision.Collider;

#if UNITY_EDITOR
using UnityEditor;
using Ate.EditorHelpers;
#endif


//#define DebugEnter
//#define DebugExit


namespace Ate.Collision
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
		public List<Collider> otherColliders = new List<Collider> ();

		#endregion


		#region Private Variables

		private List<Collider> _areaColliders     = new List<Collider> ();
		private List<Collider> _currentCollisions = new List<Collider> ();

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
			otherColliders[index] = EditorGUILayout.ObjectField ("Collider", otherColliders[index], typeof(Collider), true) as Collider;
		}

		#endif


		#region GameObject

		private void OnDestroy ()
		{
			//TODO: Send event notifying things of our destruction
		}

		#endregion


		#region AteComponent

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
			_areaColliders = new List<Collider> ();
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

			//TODO: Getting hacky here
			for (int i = 0; i < _areaColliders.Count; i++)
			{
				_areaColliders[i].MyArea = this;
			}
		}

		private void AddAreaCollidersFromGameObject (GameObject theObj)
		{
			if (theObj == null)
				return;
			
			List<Collider> theColliders = new List<Collider> (theObj.GetComponents<Collider> ());
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

			bool areaOneIsThis  = eventData.AreaOne.InstanceID == this.InstanceID;
			//bool areaTwoIsThis  = eventData.AreaTwo.InstanceID == this.InstanceID;
			bool colOneIsInArea = _areaColliders.Contains (eventData.ColliderOne);
			bool colTwoIsInArea = _areaColliders.Contains (eventData.ColliderTwo);

			if (!IsValidCollision (colOneIsInArea, colTwoIsInArea))
				return;

			CollisionArea thisArea    = this;
			CollisionArea hittingArea = areaOneIsThis ? eventData.AreaTwo : eventData.AreaOne;
			Collider hitCollider     = colOneIsInArea ? eventData.ColliderOne : eventData.ColliderTwo;
			Collider hittingCollider = colTwoIsInArea ? eventData.ColliderOne : eventData.ColliderTwo;

			bool isFirstCollider = !_currentCollisions.Contains (hittingCollider);
			_currentCollisions.Add (hittingCollider);

			if (isFirstCollider || !collidersOnlyEnterOnFirst)
				SendCollisionAreaBegan (thisArea, hittingArea, hitCollider, hittingCollider);
		}

		private void OnCollisionEnded (EventData_Collision eventData)
		{
			CleanCurrentCollisions ();

			bool colOneIsInArea = _areaColliders.Contains (eventData.ColliderOne);
			bool colTwoIsInArea = _areaColliders.Contains (eventData.ColliderTwo);

			if (!IsValidCollision (colOneIsInArea, colTwoIsInArea))
				return;

			Collider ourCollider     = colOneIsInArea ? eventData.ColliderOne : eventData.ColliderTwo;
			Collider hittingCollider = colOneIsInArea ? eventData.ColliderTwo : eventData.ColliderOne;

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


		private void SendCollisionAreaBegan (CollisionArea thisArea, CollisionArea hittingArea, Collider hitCollider, Collider hittingCollider)
		{
			#if DebugEnter
			DebugLog.Simple ("<color=green>Collision Began</color>\r\nCol One: ", ourCollider, "Col Two: ", hittingCollider);
			#endif

			EventData_Collision eventData = new EventData_Collision ();
			eventData.HitArea         = thisArea;
			eventData.HittingArea     = hittingArea;
			eventData.HitCollider     = hitCollider;
			eventData.HittingCollider = hittingCollider;

			GameManager.Events.Broadcast<EventType_Collision> ((int)EventType_Collision.AreaCollisionBegan, eventData);
		}

		private void SendCollisionAreaEnded (Collider ourCollider, Collider hittingCollider)
		{
			#if DebugExit
			DebugLog.Simple ("<color=red>Collision Ended</color>\r\nCol One: ", ourCollider, "Col Two: ", hittingCollider);
			#endif

			EventData_Collision eventData = new EventData_Collision ();
			eventData.HitArea = this;
			eventData.HitCollider      = ourCollider;
			eventData.HittingCollider   = hittingCollider;

			GameManager.Events.Broadcast<EventType_Collision> ((int)EventType_Collision.AreaCollisionEnded, eventData);
		}

		#endregion

	}//End Class


}//End Namespace
