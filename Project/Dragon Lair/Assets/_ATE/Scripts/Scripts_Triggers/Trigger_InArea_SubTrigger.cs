using UnityEngine;
using System.Collections;


[System.Obsolete ("Ill-defined class that causes odd tangles with the Trigger_InArea class.")]
public class Trigger_InArea_SubTrigger : MonoBehaviour
{
	public Trigger_InArea toTrigger;


	void OnTriggerEnter (Collider theCollider)
	{
		#if UNITY_EDITOR
		Debug.LogError ("Trigger_InArea_SubTrigger is obsolete. Use ATEColliders with CollisionDetectors on object: " + gameObject.name);
		#endif

		if (toTrigger == null)
		{
			#if UNITY_EDITOR
			Debug.LogError ("SubTrigger attempted OnTriggerEnter with no toTrigger object.");
			#endif
			return;
		}

		AteGameObject triggerer = theCollider.gameObject.AteGameObject ();
		if (triggerer != null)
		{
			toTrigger.ManualEnterArea (triggerer);
		}
	}


	void OnTriggerExit (Collider theCollider)
	{
		#if UNITY_EDITOR
		Debug.LogError ("Trigger_InArea_SubTrigger is obsolete. Use ATEColliders with CollisionDetectors on object: " + gameObject.name);
		#endif

		if (toTrigger == null)
		{
			#if UNITY_EDITOR
			Debug.LogError ("SubTrigger attempted OnTriggerEnter with no toTrigger object.");
			#endif
			return;
		}

		AteGameObject triggerer = theCollider.gameObject.AteGameObject ();
		if (triggerer != null)
		{
			toTrigger.ManualExitArea (triggerer);
		}
	}

}
