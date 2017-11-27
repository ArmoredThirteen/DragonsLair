using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Ate.Collision;
using Collider = Ate.Collision.Collider;

#if UNITY_EDITOR
using UnityEditor;
using Ate.EditorHelpers;
#endif


namespace Ate.Collision
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
	public class CollisionProcessor : AteComponent
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


		/// <summary>
		/// If true, draw circles around 2d circle colliders.
		/// </summary>
		public bool drawCircleColliders = true;

		/// <summary>
		/// If true, draw spheres around 2d sphere colliders.
		/// </summary>
		public bool drawSphereColliders = true;

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


		#region AteComponent

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

		//TODO: This way of doing things is memory inefficient. High complexity.
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
		/// Checks registered collider pairs for active collisions.
		/// Cleans the registered colliders and collider pairs of null values before running.
		/// </summary>
		public List<CollisionDetails> FindCollisionDetails ()
		{
			CleanRegistrationData ();

			List<CollisionDetails> returnDetails = new List<CollisionDetails> ();

			//	For now, do not cache these so they can be tweaked with live in editor
			CheckCollisionSettings settings = new CheckCollisionSettings (maxCheckDistance, upAxis);
			//TODO: Eventually add #if UnityEditor for changing settings in editor but caching settings in game

			float sqrMaxCheckDistance = maxCheckDistance * maxCheckDistance;

			for (int i = 0; i < _colliderPairs.Count; i++)
			{
				//	Skip if they're too far away to compare
				float distance = _colliderPairs[i].v1.GetPosition ().SqrDistanceTo (_colliderPairs[i].v2.GetPosition ());
				if (distance > sqrMaxCheckDistance)
					continue;

				//	If particular collision should be ignored
				if (_colliderPairs[i].v1.MyArea != null && _colliderPairs[i].v2.MyArea != null)
				{
					if (_colliderPairs[i].v1.ignoreAreas.Contains (_colliderPairs[i].v2.MyArea))
						continue;
					if (_colliderPairs[i].v2.ignoreAreas.Contains (_colliderPairs[i].v1.MyArea))
						continue;
				}

				CollisionDetails details = _colliderPairs[i].v1.CheckCollision (settings, _colliderPairs[i].v2);

				if (details != null)
					returnDetails.Add (details);
			}

			return returnDetails;
		}

		#endregion


		#region Private Methods

		#endregion


		#region Drawing Gizmos

		#if UNITY_EDITOR

		private void OnDrawGizmos ()
		{
			Color startColor = UnityEditor.Handles.color;
			UnityEditor.Handles.color = Color.green;

			if (drawCircleColliders)
			{
				OnDrawCircles ();
			}

			if (drawSphereColliders)
			{
				OnDrawSpheres ();
			}

			UnityEditor.Handles.color = startColor;
		}

		private void OnDrawCircles ()
		{
			Collider_Circle[] colliders =
				FindObjectsOfType (typeof (Collider_Circle)) as Collider_Circle[];

			Vector3 upVector = new Vector3 ();
			switch (upAxis)
			{
				case VectorAxis.X:
					upVector = Vector3.right;
					break;

				case VectorAxis.Y:
					upVector = Vector3.up;
					break;

				case VectorAxis.Z:
					upVector = Vector3.forward;
					break;
			}

			for (int i = 0; i < colliders.Length; i++)
			{
				if (!colliders[i].isActiveAndEnabled)
					continue;

				float colliderRadius = colliders[i].radius * colliders[i].gameObject.transform.lossyScale.x;

				UnityEditor.Handles.DrawWireDisc (colliders[i].transform.position, upVector, colliderRadius);
			}
		}

		private void OnDrawSpheres ()
		{
			Collider_Sphere[] colliders =
				FindObjectsOfType (typeof (Collider_Sphere)) as Collider_Sphere[];

			Vector3 upVector  = new Vector3 ();
			Vector3 vectTwo   = new Vector3 ();
			Vector3 vectThree = new Vector3 ();

			switch (upAxis)
			{
				case VectorAxis.X:
					upVector  = Vector3.right;
					vectTwo   = Vector3.up;
					vectThree = Vector3.forward;
					break;

				case VectorAxis.Y:
					upVector = Vector3.up;
					vectTwo   = Vector3.right;
					vectThree = Vector3.forward;
					break;

				case VectorAxis.Z:
					upVector = Vector3.forward;
					vectTwo   = Vector3.right;
					vectThree = Vector3.up;
					break;
			}

			for (int i = 0; i < colliders.Length; i++)
			{
				if (!colliders[i].isActiveAndEnabled)
					continue;

				float colliderRadius = colliders[i].radius * colliders[i].gameObject.transform.lossyScale.x;

				UnityEditor.Handles.DrawWireDisc (colliders[i].transform.position, upVector,  colliderRadius);

				Color defColor = UnityEditor.Handles.color;
				UnityEditor.Handles.color = Color.red;

				UnityEditor.Handles.DrawWireDisc (colliders[i].transform.position, vectTwo,   colliderRadius);
				UnityEditor.Handles.DrawWireDisc (colliders[i].transform.position, vectThree, colliderRadius);

				UnityEditor.Handles.color = defColor;
			}
		}

		#endif

		#endregion

	}//End Class


}//End Namespace
