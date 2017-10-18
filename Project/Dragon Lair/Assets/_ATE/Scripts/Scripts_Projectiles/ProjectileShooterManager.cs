using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
using Ate.EditorHelpers;
#endif


namespace Ate
{


	public class ProjectileShooterManager : AteComponent
	{
		public List<ProjectileShooter> projectileShooters = new List<ProjectileShooter> ();
		public IndexChooser chooserProjectileShooters = new IndexChooser ();

		public float fireDelay = 1;
		private float _timer_fireDelay = 0;


		#if UNITY_EDITOR

		public override void DrawInspector ()
		{
			base.DrawInspector ();

			chooserProjectileShooters.DrawIndexChooser ();
			EditorHelper.DrawResizableList<ProjectileShooter> ("Projectile Shooters", ref projectileShooters, DrawProjectileShooter);
		}

		private void DrawProjectileShooter (int index)
		{
			projectileShooters[index] = EditorGUILayout.ObjectField
				("Projectile Shooter", projectileShooters[index], typeof (ProjectileShooter), true)
				as ProjectileShooter;
		}

		#endif


		protected override void AteAwake ()
		{
			chooserProjectileShooters.Initialize ();
		}


		protected override void AteUpdate ()
		{
			_timer_fireDelay += Time.deltaTime;

			if (_timer_fireDelay < fireDelay)
				return;

			FireProjectile ();
			_timer_fireDelay = 0;
		}


		private void FireProjectile ()
		{
			ProjectileShooter curShooter =
				chooserProjectileShooters.GetCurIndexData<ProjectileShooter> (projectileShooters, null, true);

			if (curShooter != null)
				curShooter.FireProjectile ();
		}

	}//End Class


}//End Namespace
