using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Ate.GameSystems;
using Ate.Enemies;
using Ate.Projectiles;
using Ate.Killing;

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

		public KillableObject    killableObject    = null;

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

			killableObject = EditorGUILayout.ObjectField
				("Killable Object", killableObject, typeof (KillableObject), true)
				as KillableObject;

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


		// Updates every game frame
		protected override void AteUpdate ()
		{
			base.AteUpdate ();
		}

		// Updates 24 times per second
		protected override void FpsUpdate24 ()
		{

		}

		// Updates once per framelength, which is one or more FpsUpdate## calls
		protected override void UpdateFrameLength ()
		{

		}

		#endregion


		#region PoolableObject

		public override void Pool ()
		{
			base.Pool ();

			//TODO: HACKY!! Important read!
			//	KillableObject and PoolableObject are poorly tangled together.
			//	Sometimes the PoolManager is forcing a cleanup of pooled objects,
			//	sometimes it is forcing pooling of newly made default objects,
			//	and sometimes KillableObjects are causing pooling calls.
			//	Thing is, something that isn't the PoolManager needs to clean up
			//	the scene, and first it should kill all KillableObjects then it
			//	should pool all poolable non-killable objects.
			//	THIS CALL on the other hand is intended to make it so no matter
			//	how the pooling is called, it sends a double-check kill request.
			//	So if it is part of default spawning or scene cleanup the proper
			//	kill calls go through. But if it is from a KillableObject getting
			//	killed it will call this method, this method will call a kill
			//	request again, and that double request will be a failed request
			//	since it is already in a dead state, or it will go from a dying
			//	state to an immediately cleaned up and dead state.
			//Debug.LogError ("Sup");
			//killableObject.RequestKillImmediate ();
		}

		public override void Unpool ()
		{
			base.Unpool ();

			//killableObject.RequestRevive ();
		}

		#endregion


		#region Public Methods

		#endregion


		#region Private Methods

		#endregion

	}//End Class
	
	
}//End Namespace
