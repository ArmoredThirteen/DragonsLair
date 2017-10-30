using UnityEngine;
using System.Collections;
using Ate.GameSystems;

#if UNITY_EDITOR
using UnityEditor;
#endif


namespace Ate.Projectiles
{


	public class Projectile : AteComponent
	{
		public float maxLifetime = 60;
		public float moveSpeed = 1;

		public bool faceTarget = true;

		public int pixelsPerUnit = 8;

		/// <summary>
		/// If true, updates frame when internal update count matches frame length.
		/// If false, updates frame when event data update count matches frame length.
		/// </summary>
		public bool localUpdate = false;
		public int frameLength = 6;


		private Vector3 _targetPosition = new Vector3 ();
		private int _totalFramesPlayed = 0;

		private float _timer_lifetime = 0;
		private bool isFired = false;

		private Vector3 targetDirection;
		//private Transform target;


		#if UNITY_EDITOR

		public override void DrawInspector ()
		{
			base.DrawInspector ();

			maxLifetime = EditorGUILayout.FloatField ("Max Lifetime", maxLifetime);
			moveSpeed   = EditorGUILayout.FloatField ("Move Speed",   moveSpeed);

			faceTarget = EditorGUILayout.Toggle ("Face Target", faceTarget);

			pixelsPerUnit = EditorGUILayout.IntField ("Pixels per Unit", pixelsPerUnit);
			localUpdate   = EditorGUILayout.Toggle   ("Local Update",    localUpdate);
			frameLength   = EditorGUILayout.IntField ("Frame Length",    frameLength);
		}

		#endif


		public void Fire (AteObject firer, Vector3 targetPos)
		{
			_targetPosition = Position;
			targetDirection = Position.GetDir_To (targetPos, moveSpeed);

			if (faceTarget)
			{
				transform.LookAt (Position+targetDirection);
			}

			isFired = true;
		}


		protected override void RegisterEvents ()
		{
			GameManager.Events.Register<EventType_Updates, EventData_Updates>
			((int)EventType_Updates.fpsUpdate24, OnFpsUpdate24);
		}

		protected override void UnregisterEvents ()
		{
			GameManager.Events.Unregister<EventType_Updates, EventData_Updates>
			((int)EventType_Updates.fpsUpdate24, OnFpsUpdate24);
		}


		protected override void AteUpdate ()
		{
			if (!isFired)
				return;

			_timer_lifetime += Time.deltaTime;
			if (_timer_lifetime >= maxLifetime)
			{
				_timer_lifetime = 0;
				Destroy (gameObject);
				return;
			}

			//TODO: Live rotation to target if needed
			//		(useful when it uses a transform target not just a vector)

			//transform.position = Vector3.Lerp (Position, targetDirection, moveSpeed);
			_targetPosition = _targetPosition + (targetDirection * Time.deltaTime);
		}


		private void OnFpsUpdate24 (EventData_Updates eventData)
		{
			bool shouldUpdate = false;

			if (localUpdate)
			{
				if ((_totalFramesPlayed % frameLength) == 0)
					shouldUpdate = true;
			}
			else
			{
				if ((eventData.updateIndex % frameLength) == 0)
					shouldUpdate = true;
			}

			if (shouldUpdate)
			{
				UpdateLocation ();
			}

			_totalFramesPlayed += 1;
		}

		private void UpdateLocation ()
		{
			float desiredX = _targetPosition.x;
			float desiredY = _targetPosition.y;
			float desiredZ = _targetPosition.z;

			desiredX = Mathf.Round (desiredX * pixelsPerUnit) / pixelsPerUnit;
			desiredY = Mathf.Round (desiredY * pixelsPerUnit) / pixelsPerUnit;
			desiredZ = Mathf.Round (desiredZ * pixelsPerUnit) / pixelsPerUnit;

			Position = new Vector3 (desiredX, desiredY, desiredZ);
		}

	}//End Class


}//End Namespace
