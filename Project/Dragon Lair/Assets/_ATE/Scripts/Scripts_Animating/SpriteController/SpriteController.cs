using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif


namespace Ate.SpriteAnimation
{
	
	/// <summary>
	/// Controls a SpriteRenderer by manually
	/// changing its Sprite on a per frame basis.
	/// </summary>
	public class SpriteController : AteComponent
	{
		#region Public Variables

		public List<Sprite> sprites = new List<Sprite> ();

		public bool startPaused = false;
		public float framesPerSecond = 8;
		public int startFrame = 0;

		#endregion


		#region Private Variables

		private SpriteRenderer _spriteRenderer = null;

		private bool _drawSpriteList = false;

		private float _interval_fps;
		private float _timer_fps;

		private bool _isPaused;
		private int _curFrame = 0;

		#endregion


		#if UNITY_EDITOR

		public override void DrawInspector ()
		{
			base.DrawInspector ();

			OnDrawSpriteList ();

			startPaused = EditorGUILayout.Toggle ("Start Paused", startPaused);
			OnDrawFPS ();
			startFrame = EditorGUILayout.IntField ("Start Frame", startFrame);
		}


		private void OnDrawFPS ()
		{
			float newFPS = EditorGUILayout.FloatField ("Frames/Sec", framesPerSecond);
			if (newFPS > 0)
				framesPerSecond = newFPS;
		}

		private void OnDrawSpriteList ()
		{
			_drawSpriteList = EditorGUILayout.Toggle ("Draw Sprite List", _drawSpriteList);
			if (_drawSpriteList)
				EditorHelper.DrawResizableList ("Sprites", ref sprites, OnDrawSprite);
		}

		private void OnDrawSprite (int index)
		{
			EditorGUILayout.BeginVertical ();

			EditorGUILayout.LabelField ("Sprite "+index);

			sprites[index] = EditorGUILayout.ObjectField
				(sprites[index], typeof(Sprite), true)
				as Sprite;

			EditorGUILayout.EndVertical ();
		}

		#endif


		#region AteComponent

		protected override void AteAwake ()
		{
			_interval_fps = 1/framesPerSecond;
			_timer_fps = _interval_fps;

			_isPaused = startPaused;
			_curFrame = startFrame;
		}

		protected override void AteStart ()
		{
			_spriteRenderer = gameObject.GetComponent (typeof(SpriteRenderer)) as SpriteRenderer;
			if (_spriteRenderer == null)
			{
				#if UNITY_EDITOR
				Debug.LogError ("SpriteController has no SpriteRenderer.\r\n" +
					"SpriteController has been automatically removed.");
				#endif
				DestroyImmediate (this);
				return;
			}

			ChangeFrameTo (startFrame);

			if (startPaused)
				PauseSpriteAnimation ();
			else
				PlaySpriteAnimation ();
		}

		protected override void AteUpdate ()
		{
			if (_isPaused)
				return;

			//	So FPS can update live in editor
			#if UNITY_EDITOR
			_interval_fps = 1/framesPerSecond;
			#endif

			_timer_fps -= Time.deltaTime;
			if (_timer_fps <= 0)
			{
				_timer_fps = _interval_fps;
				int nextFrame = (_curFrame+1) % sprites.Count;
				ChangeFrameTo (nextFrame);
			}
		}

		#endregion


		#region Public Methods

		//TODO: Handle all this via state machines, since more states will be made soon
		public void PlaySpriteAnimation (float speed = 1)
		{
			_isPaused = false;
		}

		public void PauseSpriteAnimation ()
		{
			_isPaused = true;
		}

		#endregion


		#region Private Methods

		private void ChangeFrameTo (int newFrame)
		{
			_curFrame = newFrame;
			_spriteRenderer.sprite = sprites[_curFrame];
		}

		#endregion

	}


}//End AteSprite namespace
