using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif


namespace CollisionSystem
{


	public enum VectorAxis
	{
		X,
		Y,
		Z,
	}

	
	/// <summary>
	/// Detects collisions among given sets of colliders.
	/// </summary>
	public class CollisionProcessor : AteGameObject
	{
		
		#region Public Variables

		/// <summary>
		/// If two colliders are further from each other than this they never collide.
		/// </summary>
		public float maxCheckDistance = 50;

		/// <summary>
		/// Controls alignment of Collider2D checks.
		/// </summary>
		public VectorAxis upAxis = VectorAxis.Y;

		#endregion


		#region Private Variables

		/// <summary>
		/// All Colliders registered with the processor.
		/// </summary>
		private List<Collider> _colliders = new List<Collider> ();

		/// <summary>
		/// Pairs of all possible collisions between Colliders.
		/// </summary>
		private List<Pair<Collider,Collider>> _colliderPairs = new List<Pair<Collider, Collider>> ();

		#endregion


		#if UNITY_EDITOR

		public override void DrawInspector ()
		{
			base.DrawInspector ();

			maxCheckDistance = EditorGUILayout.FloatField ("Max Check Dist", maxCheckDistance);
			upAxis = (VectorAxis)EditorGUILayout.EnumPopup ("Up Axis", upAxis);
		}

		#endif


		#region AteGameObject

		protected override void AteUpdate ()
		{
			/*List<CollisionDetails> details = FindCollisionDetails ();

			for (int i = 0; i < details.Count; i++)
			{
				DebugLog.Simple ("Col One: ", details[i].colOne.name, "Col Two: ", details[i].colTwo.name);
			}*/
		}

		#endregion


		#region Public Methods

		/// <summary>
		/// Clears the list of registered colliders and collider pairs.
		/// </summary>
		public void ResetRegistrationData ()
		{
			_colliders.Clear ();
			_colliderPairs.Clear ();
		}

		/// <summary>
		/// Removes any nulls out of the registered colliders and collider pairs.
		/// </summary>
		public void CleanRegistrationData ()
		{
			UnregisterCollider (null);
			UnregisterColliderPairs (null);
		}

		/// <summary>
		/// Adds theCol to the list of registered colliders and collider pairs.
		/// </summary>
		public void RegisterCollider (Collider theCol)
		{
			if (theCol == null)
				return;
			if (_colliders.Contains (theCol))
				return;
			
			_colliders.Add (theCol);
			RegisterColliderPairs (theCol);
		}

		/// <summary>
		/// Using list of registered colliders, registers new pairs.
		/// </summary>
		public void RegisterColliderPairs (Collider theCol)
		{
			if (theCol == null)
				return;

			for (int i = 0; i < _colliders.Count; i++)
			{
				if (_colliders[i] == null)
					continue;
				if (_colliders[i] == theCol)
					continue;

				_colliderPairs.Add (new Pair<Collider, Collider> (theCol, _colliders[i]));
			}
		}

		/// <summary>
		/// Removes all instances of theCol from the list of registered colliders and collider pairs.
		/// </summary>
		public void UnregisterCollider (Collider theCol)
		{
			if (!_colliders.Contains (theCol))
				return;

			for (int i = _colliders.Count-1; i >= 0; i--)
			{
				if (_colliders[i] != theCol)
					continue;

				_colliders.RemoveAt (i);
			}

			UnregisterColliderPairs (theCol);
		}

		/// <summary>
		/// Using list of registered pairs, removes any referencing given Collider.
		/// Can be given null, which will remove any pairs containing a null.
		/// </summary>
		private void UnregisterColliderPairs (Collider theCol)
		{
			for (int i = _colliderPairs.Count-1; i >= 0; i--)
			{
				if (!ColliderPairContainsCollider (_colliderPairs[i], theCol))
					continue;
				
				_colliderPairs.RemoveAt (i);
			}
		}

		/// <summary>
		/// Returns true if either Collider in the given pair matches the given Collider.
		/// </summary>
		private bool ColliderPairContainsCollider (Pair<Collider,Collider> thePair, Collider theCol)
		{
			return (theCol == thePair.v1) || (theCol == thePair.v2);
		}


		/// <summary>
		/// Checks registered collider pairs for collisions.
		/// Cleans the registered colliders and collider pairs of null values before running.
		/// </summary>
		public List<CollisionDetails> FindCollisionDetails ()
		{
			CleanRegistrationData ();

			List<CollisionDetails> returnDetails = new List<CollisionDetails> ();

			//	For now, do not cache these so they can be tweaked with live in editor
			CheckCollisionSettings settings = new CheckCollisionSettings (maxCheckDistance, upAxis);
			//TODO: Eventually add #if UnityEditor for changing settings in editor but caching settings in game

			for (int i = 0; i < _colliderPairs.Count; i++)
			{
				//	Skip if they're too far away to compare
				float distance = Vector3.Distance (_colliderPairs[i].v1.GetPosition (), _colliderPairs[i].v2.GetPosition ());
				if (distance > maxCheckDistance)
					continue;

				CollisionDetails details = _colliderPairs[i].v1.CheckCollision (settings, _colliderPairs[i].v2);

				if (details != null)
					returnDetails.Add (details);
			}

			return returnDetails;
		}

		#endregion


		#region Private Methods

		#endregion

	}//end class


}//end namespace
