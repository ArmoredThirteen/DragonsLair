using UnityEngine;
using System.Collections;
using Ate.GameSystems;
using Ate.Projectiles;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
using Ate.EditorHelpers;
#endif


namespace Ate
{


	public class Player_FireOnClick_fpsControlled : AteComponent_fpsControlled
	{
		public List<KeyCode> fireKeys = new List<KeyCode> ();

		public int framesPerFire = 2;
		public ProjectileShooter defaultShooter;


		private int _timer_framesSinceFire = int.MaxValue/2;//arbitrarily high so first shot is always reactive

		private bool _fireRequested = false;
		private bool _isMousePressed = false;


		#if UNITY_EDITOR

		public override void DrawInspector ()
		{
			base.DrawInspector ();

			EditorHelper.DrawResizableList<KeyCode> ("Firing Keys", ref fireKeys, DrawEntry_KeyCode);

			framesPerFire = EditorGUILayout.IntField ("Frames per Fire", framesPerFire);

			defaultShooter = EditorGUILayout.ObjectField
				("Default Shooter", defaultShooter, typeof(ProjectileShooter), true)
				as ProjectileShooter;
		}

		private void DrawEntry_KeyCode (int index)
		{
			fireKeys[index] = (KeyCode)EditorGUILayout.EnumPopup ("KeyCode", fireKeys[index]);
		}

		#endif


		#region Ate Component

		protected override void RegisterEvents ()
		{
			base.RegisterEvents ();

			GameManager.Events.Register<EventType_UI, EventData_UI> ((int)EventType_UI.Clicked,  OnKeyClicked);
			GameManager.Events.Register<EventType_UI, EventData_UI> ((int)EventType_UI.Released, OnKeyReleased);
		}

		protected override void UnregisterEvents ()
		{
			base.UnregisterEvents ();

			GameManager.Events.Unregister<EventType_UI, EventData_UI> ((int)EventType_UI.Clicked,  OnKeyClicked);
			GameManager.Events.Unregister<EventType_UI, EventData_UI> ((int)EventType_UI.Released, OnKeyReleased);
		}


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

		#endregion


		private void OnKeyClicked (EventData_UI eventData)
		{
			if (fireKeys.Contains (eventData.TheKey))
				FireKeyClicked ();
		}

		private void OnKeyReleased (EventData_UI eventData)
		{
			if (fireKeys.Contains (eventData.TheKey))
				FireKeyReleased ();
		}

		private void FireKeyClicked ()
		{
			_isMousePressed = true;
		}

		private void FireKeyReleased ()
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
