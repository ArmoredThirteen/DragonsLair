using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif


namespace Ate
{


	public class Projectile : AteComponent
	{
		public float maxLifetime = 60;
		public float moveSpeed = 1;

		public bool faceTarget = true;

		private float _timer_lifetime = 0;
		private bool isFired = false;

		private Vector3 targetDirection;
		//private Transform target;


		#if UNITY_EDITOR

		public override void DrawInspector ()
		{
			base.DrawInspector ();

			maxLifetime = EditorGUILayout.FloatField ("Max Lifetime", maxLifetime);
			moveSpeed = EditorGUILayout.FloatField ("Move Speed", moveSpeed);

			faceTarget = EditorGUILayout.Toggle ("Face Target", faceTarget);
		}

		#endif


		public void Fire (Vector3 targetPos)
		{
			targetDirection = Position.GetDir_To (targetPos, moveSpeed);

			if (faceTarget)
			{
				transform.LookAt (Position+targetDirection);
			}

			isFired = true;
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
			transform.position = Position + (targetDirection * Time.deltaTime);
		}

	}//End Class


}//End Namespace
