using UnityEngine;
using System.Collections;
using Ate.GameSystems;

#if UNITY_EDITOR
using UnityEditor;
#endif


namespace Ate.Projectiles
{


	public class Projectile : AteComponent_fpsControlled
	{
		public float maxLifetime = 50;
		public float moveSpeed = 1;

		public bool faceTarget = true;


		private float _timer_lifetime = 0;
		private bool _isFired = false;

		private Vector3 _targetDirection;
		//private Transform target;


		#if UNITY_EDITOR

		public override void DrawInspector ()
		{
			base.DrawInspector ();

			maxLifetime = EditorGUILayout.FloatField ("Max Lifetime", maxLifetime);
			moveSpeed   = EditorGUILayout.FloatField ("Move Speed",   moveSpeed);

			faceTarget = EditorGUILayout.Toggle ("Face Target", faceTarget);
		}

		#endif


		public void Fire (AteObject firer, Vector3 targetPos)
		{
			_targetDirection = Position.GetDir_To (targetPos, moveSpeed);

			if (faceTarget)
			{
				transform.LookAt (Position+_targetDirection);
			}

			_isFired = true;
		}


		protected override void AteUpdate ()
		{
			
		}

		protected override void UpdateBaseFps ()
		{
			
		}

		protected override void UpdateFrameLength ()
		{
			if (!_isFired)
				return;

			_timer_lifetime = _timer_lifetime + 1;
			if (_timer_lifetime >= maxLifetime)
			{
				_timer_lifetime = 0;
				Destroy (gameObject);
				return;
			}

			UpdateLocation ();
		}


		private void UpdateLocation ()
		{
			float desiredX = Position.x + _targetDirection.x;
			float desiredY = Position.y + _targetDirection.y;
			float desiredZ = Position.z + _targetDirection.z;

			Vector3 desiredPosition = new Vector3 (desiredX, desiredY, desiredZ);

			Position = desiredPosition;
		}

	}//End Class


}//End Namespace
