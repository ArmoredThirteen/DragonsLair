using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Ate.Pooling;
using Ate.GameSystems;
using Ate.Projectiles;
using Ate.Enemies;

#if UNITY_EDITOR
using UnityEditor;
using Ate.EditorHelpers;
#endif


namespace Ate.Spawning
{
	
	
	/// <summary>
	/// Description
	/// </summary>
	public class SpawnerEnemy : Spawner
	{
		
		#region Public Variables

		#endregion


		#region Private Variables

		#endregion


		#region EnemyShipMovement Variables

		public float moveDistance  = 1;
		public int   maxMoveSteps  = 1;
		public bool  useRandomSeed = true;
		public int   seed          = 666;

		#endregion


		#region EnemyShipFiring Variables

		public int minFireFrames = 12;
		public int maxFireFrames = 24;

		#endregion


		#region ProjectileShooter Variables

		public Projectile projectilePrefab = null;
		//public enum TargetingType and the related targeting variables
		public float fireStartVariance = 0;
		public float autoFireDelay     = 0;
		public float targetVariance    = 0;
		public List<Vector3> barrelOffsets = new List<Vector3> ();

		#endregion

		#region Collision
		#endregion

		#region Graphics
		#endregion


		#if UNITY_EDITOR

		public override void DrawInspector ()
		{
			base.DrawInspector ();
			EditorGUILayout.Space ();

			EditorGUILayout.Space ();

			EditorGUILayout.LabelField ("Enemy Ship Movement", EditorStyles.boldLabel);
			EditorGUI.indentLevel++;
			DrawEnemyShipMovement ();
			EditorGUI.indentLevel--;
			EditorGUILayout.Space ();

			EditorGUILayout.LabelField ("Enemy Ship Firing", EditorStyles.boldLabel);
			EditorGUI.indentLevel++;
			DrawEnemyShipFiring ();
			EditorGUI.indentLevel--;
			EditorGUILayout.Space ();

			EditorGUILayout.LabelField ("Projectile Shooter", EditorStyles.boldLabel);
			EditorGUI.indentLevel++;
			DrawProjectileShooter ();
			EditorGUI.indentLevel--;
			EditorGUILayout.Space ();
		}


		private void DrawEnemyShipMovement ()
		{
			moveDistance  = EditorGUILayout.FloatField ("Move Distance", moveDistance);
			maxMoveSteps  = EditorGUILayout.IntField   ("Max Move Steps", maxMoveSteps);

			useRandomSeed = EditorGUILayout.Toggle     ("Use Random Seed", useRandomSeed);
			if (!useRandomSeed)
				seed = EditorGUILayout.IntField ("Seed", seed);
		}

		private void DrawEnemyShipFiring ()
		{
			minFireFrames = EditorGUILayout.IntField ("Min Fire Frames", minFireFrames);
			maxFireFrames = EditorGUILayout.IntField ("Max Fire Frames", maxFireFrames);
		}

		private void DrawProjectileShooter ()
		{
			projectilePrefab = EditorGUILayout.ObjectField
				("Projectile Prefab", projectilePrefab, typeof (Projectile), false)
				as Projectile;
			
			fireStartVariance = EditorGUILayout.FloatField ("Fire Start Variance", fireStartVariance);
			targetVariance    = EditorGUILayout.FloatField ("Target Variance",     targetVariance);
			autoFireDelay     = EditorGUILayout.FloatField ("Autofire Delay",      autoFireDelay);

			EditorHelper.DrawResizableList<Vector3> ("Barrel Offsets", ref barrelOffsets, DrawEntry_BarrelOffset);
		}


		private void DrawEntry_BarrelOffset (int index)
		{
			float xVal = barrelOffsets[index].x;
			float yVal = barrelOffsets[index].y;
			float zVal = barrelOffsets[index].z;

			EditorGUILayout.BeginHorizontal ();

			EditorGUILayout.LabelField ("X", GUILayout.Width (45));
			xVal = EditorGUILayout.FloatField (xVal, GUILayout.Width (100));

			EditorGUILayout.LabelField ("Y", GUILayout.Width (45));
			yVal = EditorGUILayout.FloatField (yVal, GUILayout.Width (100));

			EditorGUILayout.LabelField ("Z", GUILayout.Width (45));
			zVal = EditorGUILayout.FloatField (zVal, GUILayout.Width (100));

			EditorGUILayout.EndHorizontal ();

			barrelOffsets[index] = new Vector3 (xVal, yVal, zVal);
		}

		#endif


		#region AteComponent

		protected override void AteStart ()
		{
			Spawn ();
		}

		protected override void AteUpdate ()
		{
			
		}

		#endregion


		#region Public Methods

		public override void Spawn ()
		{
			PoolableEnemy spawnedObject = GameManager.Pooling.UnpoolObject (PoolID.Enemy_01) as PoolableEnemy;
			spawnedObject.transform.position = transform.position;

			SetData_EnemyShipMovement (spawnedObject);
			SetData_EnemyShipFiring   (spawnedObject);
			SetData_ProjectileShooter (spawnedObject);
		}

		#endregion


		#region Private Methods

		private void SetData_EnemyShipMovement (PoolableEnemy spawnedObject)
		{
			EnemyShipMovement theComponent = spawnedObject.enemyShipMovement;

			theComponent.moveDistance  = moveDistance;
			theComponent.maxMoveSteps  = maxMoveSteps;
			theComponent.useRandomSeed = useRandomSeed;
			theComponent.seed          = seed;

			theComponent.BuildRandomFromSeed ();
		}

		private void SetData_EnemyShipFiring (PoolableEnemy spawnedObject)
		{
			EnemyShipFiring theComponent = spawnedObject.enemyShipFiring;

			theComponent.minFireFrames = minFireFrames;
			theComponent.maxFireFrames = maxFireFrames;
		}

		private void SetData_ProjectileShooter (PoolableEnemy spawnedObject)
		{
			ProjectileShooter theComponent = spawnedObject.projectileShooter;

			theComponent.projectilePrefab  = projectilePrefab;
			theComponent.fireStartVariance = fireStartVariance;
			theComponent.targetVariance    = targetVariance;
			theComponent.autoFireDelay     = autoFireDelay;

			int index = Mathf.Min (theComponent.barrels.Count, barrelOffsets.Count);
			for (int i = 0; i < index; i++)
			{
				theComponent.barrels[i].transform.localPosition = barrelOffsets[i];
			}
		}

		#endregion

	}//End Class
	
	
}//End Namespace
