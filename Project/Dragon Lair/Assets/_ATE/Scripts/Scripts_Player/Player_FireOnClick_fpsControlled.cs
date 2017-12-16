using UnityEngine;
using System.Collections;
using Ate.GameSystems;
using Ate.Projectiles;

#if UNITY_EDITOR
using UnityEditor;
#endif


namespace Ate
{


	public class Player_FireOnClick_fpsControlled : AteComponent_fpsControlled
	{
		public int framesPerFire = 2;
		public ProjectileShooter defaultShooter;


		private int _timer_framesSinceFire = int.MaxValue/2;//arbitrarily high so first shot is always reactive

		private bool _fireRequested = false;
		private bool _isMousePressed = false;


		#if UNITY_EDITOR

		public override void DrawInspector ()
		{
			base.DrawInspector ();

			framesPerFire = EditorGUILayout.IntField ("Frames per Fire", framesPerFire);

			defaultShooter = EditorGUILayout.ObjectField
				("Default Shooter", defaultShooter, typeof(ProjectileShooter), true)
				as ProjectileShooter;
		}

		#endif


		protected override void AteUpdate ()
		{
			CheckFireRequest ();
		}

		protected override void UpdateBaseFps ()
		{
			
		}

		protected override void UpdateFrameLength ()
		{
			_timer_framesSinceFire++;

			if (_timer_framesSinceFire >= framesPerFire)
			{
				FireIfRequested ();
			}
		}


		protected override void RegisterEvents ()
		{
			base.RegisterEvents ();

			GameManager.Events.Register<EventType_UI, EventData_UI> ((int)EventType_UI.Clicked,  OnMouseClicked);
			GameManager.Events.Register<EventType_UI, EventData_UI> ((int)EventType_UI.Released, OnMouseReleased);
		}

		protected override void UnregisterEvents ()
		{
			base.UnregisterEvents ();

			GameManager.Events.Unregister<EventType_UI, EventData_UI> ((int)EventType_UI.Clicked,  OnMouseClicked);
			GameManager.Events.Unregister<EventType_UI, EventData_UI> ((int)EventType_UI.Released, OnMouseReleased);
		}


		private void OnMouseClicked (EventData_UI data)
		{
			_isMousePressed = true;
		}

		private void OnMouseReleased (EventData_UI data)
		{
			_isMousePressed = false;
		}


		private void CheckFireRequest ()
		{
			if (_isMousePressed)
				_fireRequested = true;
		}

		private void FireIfRequested ()
		{
			if (!_fireRequested)
				return;

			defaultShooter.FireProjectile ();

			_timer_framesSinceFire = 0;
			_fireRequested = false;
		}

	}//End Class


}//End Namespace
