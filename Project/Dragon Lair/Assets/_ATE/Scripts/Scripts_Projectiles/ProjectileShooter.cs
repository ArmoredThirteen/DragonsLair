using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif


public class ProjectileShooter : AteGameObject
{
	public bool isAutoFire = false;

	public Projectile projectilePrefab;
	public Transform projectileParent;

	//TODO: Needs to be modifyable through code not just design
	public Transform projectileTarget;

	public float fireStartVariance = 5;
	public float targetVariance = 5;

	/// <summary> If isAutofiring, attempt to fire every delay seconds </summary>
	public float autoFireDelay = 0;
	private float _timer_autoFire = 0;

	//TODO: Hacky
	private string projectileParentName = "ProjectileContainer";


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
		if (projectileTarget == null)
			return;
		
		Projectile newObject = GameObject.Instantiate<Projectile> (projectilePrefab) as Projectile;
		if (newObject == null)
			return;
		
		newObject.gameObject.SetPosition (Position.GetLoc_RandomInCircle_UpY (fireStartVariance));
		//newObject.MyTransform.rotation = MyTransform.rotation;
		newObject.MyTransform.parent = projectileParent;
		//Projectile asProjectile = newObject.GetComponent<Projectile> () as Projectile;
		/*if (newObject == null)
			return;*/

		//	Roughly fire toward the target
		newObject.Fire (projectileTarget.position.GetLoc_RandomInCircle_UpY (targetVariance));
	}

}
