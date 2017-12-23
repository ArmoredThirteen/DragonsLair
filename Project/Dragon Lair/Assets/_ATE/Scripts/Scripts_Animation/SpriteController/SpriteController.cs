using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Ate.GameSystems;

#if UNITY_EDITOR
using UnityEditor;
using Ate.EditorHelpers;
#endif


namespace Ate.SpriteAnimation
{
	
	/// <summary>
	/// Controls a SpriteRenderer by manually
	/// changing its Sprite on a per frame basis.
	/// </summary>
	public class SpriteController : AteComponent_fpsControlled
	{
		#region Public Variables

		public List<Sprite> sprites = new List<Sprite> ();

		public bool startPaused = false;
		public int startFrame = 0;

		/// <summary>
		/// Number of times the sprites can be iterated through. -1 is Infinite.
		/// </summary>
		public int maxLoops = -1;
		/// <summary>
		/// Frame to set animation to after maxLoops. -1 is Null.
		/// </summary>
		public int endFrame = -1;

		#endregion


		#region Private Variables

		private SpriteRenderer _spriteRenderer = null;

		private bool _drawSpriteList = false;

		private bool _isPaused;
		private int  _curFrame;

		private int _curLoop;

		#endregion


		#if UNITY_EDITOR

		public override void DrawInspector ()
		{
			base.DrawInspector ();

			OnDrawSpriteList ();

			startPaused = EditorGUILayout.Toggle   ("Start Paused", startPaused);
			startFrame  = EditorGUILayout.IntField ("Start Frame",  startFrame);

			maxLoops = EditorGUILayout.IntField ("Max Loops", maxLoops);
			endFrame = EditorGUILayout.IntField ("End Frame", endFrame);
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
			base.AteAwake ();

			_isPaused = startPaused;
		}

		protected override void AteStart ()
		{
			base.AteStart ();

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
			_curLoop = 0;

			if (startPaused)
				PauseSpriteAnimation ();
			else
				PlaySpriteAnimation ();
		}


		protected override void AteUpdate ()
		{
			
		}

		protected override void UpdateBaseFps ()
		{
			
		}

		protected override void UpdateFrameLength ()
		{
			// TODO: Make this an FSM instead of having these various states as tangly logic
			// Loops are maxed out, exit early
			if (_curLoop >= maxLoops && maxLoops >= 0)
				return;
			
			_curFrame = _curFrame + 1;
			if (_curFrame >= sprites.Count)
			{
				_curFrame = _curFrame % sprites.Count;
				_curLoop++;
			}

			if (_curLoop >= maxLoops && maxLoops >= 0)
				_curFrame = endFrame;

			ChangeFrameTo (_curFrame);
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

		/// <summary>
		/// Changes the frame counter and the sprite to the newFrame.
		/// Value of -1 changes sprite to null.
		/// </summary>
		private void ChangeFrameTo (int newFrame)
		{
			_curFrame = newFrame;

			Sprite newSprite = null;
			if (_curFrame >= 0)
				newSprite = sprites[_curFrame];

			_spriteRenderer.sprite = newSprite;
		}

		#endregion

	}


}//End AteSprite namespace
