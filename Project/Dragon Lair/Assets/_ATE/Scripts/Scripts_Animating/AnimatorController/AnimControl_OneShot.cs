using UnityEngine;
using System.Collections;

public class AnimControl_OneShot : AteComponent
{
	private string _animName = "Playing";

	private bool _hasPlayed = false;
	private Animator _animator;


	protected override void AteAwake ()
	{
		_animator = GetComponent<Animator> () as Animator;

		#if UNITY_EDITOR
		if (_animator == null)
			Debug.LogError ("AnimControl_OneShot has no Animator component, will not function.");
		#endif
	}

	protected override void AteUpdate ()
	{
		
	}


	public void PlayAnimation ()
	{
		if (_hasPlayed)
			return;
		if (_animator == null)
			return;

		_hasPlayed = true;
		_animator.SetBool ("StartPlaying", true);
	}

}
