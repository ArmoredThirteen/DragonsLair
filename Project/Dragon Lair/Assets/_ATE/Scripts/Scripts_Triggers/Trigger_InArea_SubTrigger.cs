using UnityEngine;
using System.Collections;


public class Trigger_InArea_SubTrigger : MonoBehaviour
{
	public Trigger_InArea toTrigger;


	void OnTriggerEnter (Collider theCollider)
	{
		if (toTrigger == null)
		{
			#if UNITY_EDITOR
			Debug.LogError ("SubTrigger attempted OnTriggerEnter with no toTrigger object.");
			#endif
			return;
		}

		toTrigger.ManualOnTriggerEnter (theCollider);
	}


	void OnTriggerExit (Collider theCollider)
	{
		if (toTrigger == null)
		{
			#if UNITY_EDITOR
			Debug.LogError ("SubTrigger attempted OnTriggerEnter with no toTrigger object.");
			#endif
			return;
		}

		toTrigger.ManualOnTriggerExit (theCollider);
	}

}
