using UnityEngine;
using System.Collections;
using Ate.Projectiles;

#if UNITY_EDITOR
using UnityEditor;
#endif


namespace Ate.Enemies
{
	
	
	/// <summary>
	/// Controls projectile firing of enemy ships.
	/// </summary>
	public class EnemyShipFiring : AteComponent_fpsControlled
	{
		
		#region Public Variables

		public ProjectileShooter theShooter = null;

		//public float minFireTime = 0.5f;
		//public float maxFireTime = 1.0f;

		public int minFireFrames = 8;
		public int maxFireFrames = 16;

		#endregion


		#region Private Variables

		//private float _timer_fireTime = 0;

		private int _timer_fireFrames = 0;

		#endregion


		#if UNITY_EDITOR

		public override void DrawInspector ()
		{
			base.DrawInspector ();

			theShooter = EditorGUILayout.ObjectField
				("The Shooter", theShooter, typeof(ProjectileShooter), true)
				as ProjectileShooter;

			DrawFireTimes ();
		}

		private void DrawFireTimes ()
		{
			EditorGUILayout.LabelField ("Fire Times");

			EditorGUILayout.BeginHorizontal ();

			GUILayout.Space (15);
			EditorGUILayout.LabelField ("Min", GUILayout.Width (30));
			//minFireTime = EditorGUILayout.FloatField (minFireTime, GUILayout.Width (50));
			minFireFrames = EditorGUILayout.IntField (minFireFrames, GUILayout.Width (50));

			GUILayout.Space (15);
			EditorGUILayout.LabelField ("Max", GUILayout.Width (30));
			//maxFireTime = EditorGUILayout.FloatField (maxFireTime, GUILayout.Width (50));
			maxFireFrames = EditorGUILayout.IntField (maxFireFrames, GUILayout.Width (50));

			EditorGUILayout.EndHorizontal ();
		}

		#endif


		#region AteComponent

		protected override void AteStart ()
		{
			base.AteStart ();

			ResetFireTimer ();
		}

		protected override void AteUpdate ()
		{
			/*_timer_fireTime = _timer_fireTime - Time.deltaTime;
			if (_timer_fireTime <= 0)
			{
				FireProjectileShooter ();
				ResetFireTimer ();
			}*/
		}

		protected override void UpdateBaseFps ()
		{
			
		}

		protected override void UpdateFrameLength ()
		{
			_timer_fireFrames = _timer_fireFrames - 1;

			if (_timer_fireFrames <= 0)
			{
				FireProjectileShooter ();
				ResetFireTimer ();
			}
		}

		#endregion


		#region Public Methods

		#endregion


		#region Private Methods

		private void FireProjectileShooter ()
		{
			theShooter.FireProjectile ();
		}

		/// <summary>
		/// Resets the timer randomly from minFireTime to MaxFireTime.
		/// </summary>
		private void ResetFireTimer ()
		{
			//_timer_fireTime = Random.Range (minFireTime, maxFireTime);
			_timer_fireFrames = Random.Range (minFireFrames, maxFireFrames);
		}

		#endregion

	}//End Class
	
	
}//End Namespace
