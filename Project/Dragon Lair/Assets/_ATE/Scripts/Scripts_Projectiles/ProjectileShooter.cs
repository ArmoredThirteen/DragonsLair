using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Ate.Collision;

#if UNITY_EDITOR
using UnityEditor;
using Ate.EditorHelpers;
#endif


namespace Ate
{


	//TODO: This class is getting frankensteined and needs a total rework
	public class ProjectileShooter : AteComponent
	{
		public enum TargetingType
		{
			/// <summary> Probably unrequired. Mostly a placeholder for 0. </summary>
			None = 0,

			RelativeLocation = 100,
			WorldLocation    = 110,

			TransformTarget  = 200,
		}


		public AteObject firer = null;

		public bool isAutoFire = false;

		public Projectile projectilePrefab;
		public Transform projectileParent;

		public TargetingType targetType = TargetingType.TransformTarget;
		//TODO: Targets need to be modifiable through code not just design
		public Vector3 targetLocation = new Vector3 ();
		public Transform targetTransform;

		public float fireStartVariance = 5;
		public float targetVariance = 5;

		/// <summary> If isAutofiring, attempt to fire every delay seconds </summary>
		public float autoFireDelay = 0;

		public List<Transform> barrels = new List<Transform> ();


		private float _timer_autoFire = 0;

		//TODO: Hacky, having a GameSystem.ProjectileManager would be better.
		private string projectileParentName = "ProjectileContainer";

		private int _barrelIndex = 0;


		#if UNITY_EDITOR

		public override void DrawInspector ()
		{
			base.DrawInspector ();

			firer = EditorGUILayout.ObjectField
				("Firer", firer, typeof(AteObject), true)
				as AteObject;

			projectilePrefab = EditorGUILayout.ObjectField
				("Projectile Prefab", projectilePrefab, typeof (Projectile), false)
				as Projectile;

			projectileParent = EditorGUILayout.ObjectField
				("Projectile Parent", projectileParent, typeof (Transform), true)
				as Transform;

			DrawTargetType ();

			fireStartVariance = EditorGUILayout.FloatField ("Fire Start Variance", fireStartVariance);
			targetVariance = EditorGUILayout.FloatField ("Target Variance", targetVariance);

			autoFireDelay = EditorGUILayout.FloatField ("Autofire Delay", autoFireDelay);

			EditorHelper.DrawResizableList<Transform> ("Barrels", ref barrels, OnDrawBarrel);
		}

		private void DrawTargetType ()
		{
			targetType = (TargetingType)EditorGUILayout.EnumPopup ("Targeting Type", targetType);

			switch (targetType)
			{
				case TargetingType.None:
					break;

				case TargetingType.RelativeLocation:
				case TargetingType.WorldLocation:
					targetLocation = EditorGUILayout.Vector3Field ("Location", targetLocation);
					break;

				case TargetingType.TransformTarget:
					targetTransform = EditorGUILayout.ObjectField
						("Transform", targetTransform, typeof (Transform), true)
						as Transform;
					break;
			}
		}

		private void OnDrawBarrel (int index)
		{
			barrels[index] = EditorGUILayout.ObjectField
				("Barrel Transform", barrels[index], typeof (Transform), true)
				as Transform;
		}

		#endif


		protected override void AteAwake ()
		{
			if (projectileParent == null)
			{
				GameObject theObj = GameObject.Find (projectileParentName);
				if (theObj != null)
					projectileParent = theObj.transform;
			}
			if (projectileParent == null)
				projectileParent = MyTransform;
		}

		protected override void AteUpdate ()
		{
			if (isAutoFire)
				AutoFire ();
		}

		private void AutoFire ()
		{
			_timer_autoFire += Time.deltaTime;

			if (_timer_autoFire < autoFireDelay)
				return;

			FireProjectile ();
			_timer_autoFire = 0;
		}


		//TODO: Eventually add in a 'mod projectile' system to reduce prefab count.
		public void FireProjectile ()
		{
			if (projectilePrefab == null)
				return;
			if (targetTransform == null)
				return;
			
			Projectile newProjectile = GameObject.Instantiate<Projectile> (projectilePrefab) as Projectile;
			if (newProjectile == null)
				return;

			//	Projectile starting location
			Transform curBarrel = barrels[_barrelIndex];
			//TODO: RandomInCircle_UpY falsely assumes Y is always up
			newProjectile.gameObject.SetPosition (curBarrel.position.GetLoc_RandomInCircle_UpY (fireStartVariance));
			_barrelIndex = (_barrelIndex + 1) % barrels.Count;

			//	Projectile parent
			newProjectile.MyTransform.parent = projectileParent;

			//	Projectile target location
			Vector3 location = new Vector3 ();

			switch (targetType)
			{
				case TargetingType.RelativeLocation:
					location.x = targetLocation.x + curBarrel.position.x;
					location.y = targetLocation.y + curBarrel.position.y;
					location.z = targetLocation.z + curBarrel.position.z;
					break;

				case TargetingType.WorldLocation:
					location.x = targetLocation.x;
					location.y = targetLocation.y;
					location.z = targetLocation.z;
					break;

				case TargetingType.TransformTarget:
					location.x = targetTransform.position.x;
					location.y = targetTransform.position.y;
					location.z = targetTransform.position.z;
					break;
			}

			ModifyProjectile (firer, newProjectile);

			//	Roughly fire toward the target
			//TODO: RandomInCircle_UpY falsely assumes Y is always up
			newProjectile.Fire (firer, location.GetLoc_RandomInCircle_UpY (targetVariance));
		}


		//TODO: This is supposed to be an entire modification system, not just ignoreArea stuff
		private void ModifyProjectile (AteObject firer, Projectile newProjectile)
		{
			CollisionArea projectileArea = newProjectile.GetComponent<CollisionArea> () as CollisionArea;
			if (projectileArea == null)
				return;

			CollisionArea firerArea = firer.GetComponent<CollisionArea> () as CollisionArea;
			if (firerArea == null)
				return;

			projectileArea.SetColliderIgnoreArea (firerArea);
		}

	}//End Class


}//End Namespace
