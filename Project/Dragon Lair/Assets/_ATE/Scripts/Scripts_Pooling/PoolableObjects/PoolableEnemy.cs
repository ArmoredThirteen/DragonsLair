using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Ate.GameSystems;
using Ate.Enemies;
using Ate.Projectiles;

#if UNITY_EDITOR
using UnityEditor;
#endif


namespace Ate.Pooling
{
	
	
	/// <summary>
	/// Description
	/// </summary>
	public class PoolableEnemy : PoolableObject
	{

		/*[System.Serializable]
		public class PO_Data_Enemy : PO_Data
		{
			#if UNITY_EDITOR
			public override void DrawInspector ()
			{
				
			}
			#endif
		}*/


		#region Public Variables

		//public PO_Data_Enemy theEnemyData = new PO_Data_Enemy ();

		public EnemyShipMovement enemyShipMovement = null;
		public EnemyShipFiring   enemyShipFiring   = null;
		public ProjectileShooter projectileShooter = null;

		#endregion


		#region Private Variables

		#endregion


		#if UNITY_EDITOR

		public override void DrawInspector ()
		{
			base.DrawInspector ();
			EditorGUILayout.Space ();

			//theEnemyData.DrawInspector ();

			enemyShipMovement = EditorGUILayout.ObjectField
				("Enemy Ship Movement", enemyShipMovement, typeof (EnemyShipMovement), true)
				as EnemyShipMovement;

			enemyShipFiring = EditorGUILayout.ObjectField
				("Enemy Ship Firing", enemyShipFiring, typeof (EnemyShipFiring), true)
				as EnemyShipFiring;

			projectileShooter = EditorGUILayout.ObjectField
				("Projectile Shooter", projectileShooter, typeof (ProjectileShooter), true)
				as ProjectileShooter;
		}

		#endif


		#region AteComponent

		protected override void AteAwake ()
		{
			base.AteAwake ();
		}

		protected override void AteStart ()
		{
			base.AteStart ();
		}

		#endregion


		#region PoolableObject

		public override void Pool ()
		{
			base.Pool ();
		}

		public override void Unpool ()
		{
			base.Unpool ();
		}

		#endregion


		#region Public Methods

		#endregion


		#region Private Methods

		#endregion

	}//End Class
	
	
}//End Namespace
