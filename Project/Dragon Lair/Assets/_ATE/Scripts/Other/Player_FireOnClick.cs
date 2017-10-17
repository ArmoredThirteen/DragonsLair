using UnityEngine;
using System.Collections;
using Ate.GameSystems;

#if UNITY_EDITOR
using UnityEditor;
#endif


namespace Ate
{


	public class Player_FireOnClick : AteComponent
	{
		public float fireDelay = 1;
		public ProjectileShooter defaultShooter;

		private bool _isMousePressed = false;

		private float _timer_lastShot;


		#if UNITY_EDITOR

		public override void DrawInspector ()
		{
			base.DrawInspector ();

			fireDelay = EditorGUILayout.FloatField ("Fire Speed", fireDelay);

			defaultShooter = EditorGUILayout.ObjectField
				("Default Shooter", defaultShooter, typeof(ProjectileShooter), true)
				as ProjectileShooter;
		}

		#endif


		protected override void AteAwake ()
		{
			_timer_lastShot = fireDelay;
		}

		protected override void AteUpdate ()
		{
			_timer_lastShot += Time.deltaTime;
			AttemptFiring ();
		}


		protected override void RegisterEvents ()
		{
			GameManager.Events.Register<EventType_UI, EventData_UI> ((int)EventType_UI.Clicked,  OnMouseClicked);
			GameManager.Events.Register<EventType_UI, EventData_UI> ((int)EventType_UI.Released, OnMouseReleased);
		}

		protected override void UnregisterEvents ()
		{
			GameManager.Events.Unregister<EventType_UI, EventData_UI> ((int)EventType_UI.Clicked,  OnMouseClicked);
			GameManager.Events.Unregister<EventType_UI, EventData_UI> ((int)EventType_UI.Released, OnMouseReleased);
		}


		private void OnMouseClicked (EventData_UI data)
		{
			_isMousePressed = true;
			AttemptFiring ();
		}

		private void OnMouseReleased (EventData_UI data)
		{
			_isMousePressed = false;
		}


		private void AttemptFiring ()
		{
			if (!_isMousePressed)
				return;
			if (_timer_lastShot < fireDelay)
				return;

			defaultShooter.FireProjectile ();

			_timer_lastShot = 0;
		}

	}//End Class


}//End Namespace
