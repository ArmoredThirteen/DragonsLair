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
	public class SpriteController : AteComponent
	{
		#region Public Variables

		public List<Sprite> sprites = new List<Sprite> ();

		public bool startPaused = false;
		/// <summary>
		/// If true, updates frame when internal update count matches frame length.
		/// If false, updates frame when event data update count matches frame length.
		/// </summary>
		public bool localUpdate = true;
		public int frameLength = 3;
		public int startFrame = 0;

		#endregion


		#region Private Variables

		private SpriteRenderer _spriteRenderer = null;

		private bool _drawSpriteList = false;

		private bool _isPaused;
		private int _curFrame = 0;

		private int _totalFramesPlayed = 0;

		#endregion


		#if UNITY_EDITOR

		public override void DrawInspector ()
		{
			base.DrawInspector ();

			OnDrawSpriteList ();

			startPaused = EditorGUILayout.Toggle ("Start Paused", startPaused);
			localUpdate = EditorGUILayout.Toggle ("Local Update", localUpdate);
			OnDrawFPS ();
			startFrame = EditorGUILayout.IntField ("Start Frame", startFrame);
		}


		private void OnDrawFPS ()
		{
			frameLength = EditorGUILayout.IntField ("Frame Length", frameLength);
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
			_isPaused = startPaused;
			_curFrame = startFrame;
			_totalFramesPlayed = 0;
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

			randTimeToPlay = Time.time + Random.Range(0.0f, 3.0f);
		}

		private float randTimeToPlay;
		protected override void AteUpdate ()
		{
			if (Time.time > randTimeToPlay && _isPaused)
				PlaySpriteAnimation ();
		}


		protected override void RegisterEvents ()
		{
			GameManager.Events.Register<EventType_Updates, EventData_Updates>
			((int)EventType_Updates.fpsUpdate24, OnFpsUpdate24);
		}

		protected override void UnregisterEvents ()
		{
			GameManager.Events.Unregister<EventType_Updates, EventData_Updates>
			((int)EventType_Updates.fpsUpdate24, OnFpsUpdate24);
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

		private void OnFpsUpdate24 (EventData_Updates eventData)
		{
			if (_isPaused)
				return;

			bool shouldUpdate = false;

			if (localUpdate)
			{
				if ((_totalFramesPlayed % frameLength) == 0)
					shouldUpdate = true;
			}
			else
			{
				if ((eventData.updateIndex % frameLength) == 0)
					shouldUpdate = true;
			}

			if (shouldUpdate)
			{
				int newFrame = (_curFrame + 1) % sprites.Count;
				ChangeFrameTo (newFrame);
			}

			_totalFramesPlayed += 1;
		}


		private void ChangeFrameTo (int newFrame)
		{
			_spriteRenderer.sprite = sprites[_curFrame];
			_curFrame = newFrame;
		}

		#endregion

	}


}//End AteSprite namespace
