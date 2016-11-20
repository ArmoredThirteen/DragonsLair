using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif


public class ProjectileShooter : AteGameObject
{
	public Projectile projectilePrefab;
	public Transform projectileParent;

	//TODO: Needs to be modifyable through code not just design
	public Transform projectileTarget;

	public float fireStartVariance = 5;
	public float targetVariance = 5;

	/// <summary> If greater than 0, will attempt to fire every delay seconds. </summary>
	public float autoFireDelay = 0;
	private float _timer_autoFire = 0;


	protected override void AteAwake ()
	{
		if (projectileParent == null)
			projectileParent = MyTransform;
	}

	protected override void AteUpdate ()
	{
		if (autoFireDelay > 0)
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
		if (projectileTarget == null)
			return;
		
		Projectile newObject = GameObject.Instantiate<Projectile> (projectilePrefab) as Projectile;
		if (newObject == null)
			return;
		
		newObject.gameObject.SetPosition (Position.GetDir_RandomInCircle_UpZ (fireStartVariance));
		newObject.MyTransform.rotation = MyTransform.rotation;
		newObject.MyTransform.parent = projectileParent;
		//Projectile asProjectile = newObject.GetComponent<Projectile> () as Projectile;
		/*if (newObject == null)
			return;*/

		//	Roughly fire toward the target
		newObject.Fire (projectileTarget.position.GetDir_RandomInCircle_UpZ (targetVariance));
	}



	#if UNITY_EDITOR

	public override void DrawInspector ()
	{
		projectilePrefab = EditorGUILayout.ObjectField
			("Projectile Prefab", projectilePrefab, typeof (Projectile), false)
			as Projectile;

		projectileParent = EditorGUILayout.ObjectField
			("Projectile Parent", projectileParent, typeof (Transform), true)
			as Transform;

		projectileTarget = EditorGUILayout.ObjectField
			("Projectile Target", projectileTarget, typeof (Transform), true)
			as Transform;

		fireStartVariance = EditorGUILayout.FloatField ("Fire Start Variance", fireStartVariance);
		targetVariance = EditorGUILayout.FloatField ("Target Variance", targetVariance);

		autoFireDelay = EditorGUILayout.FloatField ("Autofire Delay", autoFireDelay);
	}

	#endif
}
