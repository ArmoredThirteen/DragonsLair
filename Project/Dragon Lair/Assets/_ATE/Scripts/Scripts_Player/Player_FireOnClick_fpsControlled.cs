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

		#region Public Variables

		public List<KeyCode> fireKeys = new List<KeyCode> ();

		public int framesPerFire = 2;
		public ProjectileShooter defaultShooter;

		#endregion


		#region Private Variables

		private int _timer_framesSinceFire = int.MaxValue/2;//arbitrarily high so first shot is always reactive

		private int _keysClickedCount = 0;

		//TODO: Hacky live-updating list is inefficient and prone to abuse
		/// <summary>
		/// Keys clicked this frame. Intention is so if you quickly click
		/// then release a key during a framelength you still fire once.
		/// </summary>
		private List<KeyCode> _keysClickedThisFrame = new List<KeyCode> ();

		#endregion


		#if UNITY_EDITOR

		public override void DrawInspector ()
		{
			base.DrawInspector ();

			bool drawList = true;
			EditorHelper.DrawResizableList<KeyCode> ("Firing Keys", ref drawList, ref fireKeys, DrawEntry_KeyCode);

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


		protected override void AteAwake ()
		{
			_keysClickedCount = 0;
		}


		// Updates every game frame
		protected override void AteUpdate ()
		{
			
		}

		// Updates 24 times per second
		protected override void FpsUpdate24 ()
		{
			
		}

		// Updates once per framelength, which is one or more FpsUpdate## calls
		protected override void UpdateFrameLength ()
		{
			_timer_framesSinceFire++;

			if (_timer_framesSinceFire >= framesPerFire)
			{
				FireIfRequested ();
			}

			_keysClickedThisFrame.Clear ();
		}

		#endregion


		private void OnKeyClicked (EventData_UI eventData)
		{
			if (!fireKeys.Contains (eventData.TheKey))
				return;
			
			FireKeyClicked ();

			if (!_keysClickedThisFrame.Contains (eventData.TheKey))
			{
				_keysClickedThisFrame.Add (eventData.TheKey);
			}
		}

		private void OnKeyReleased (EventData_UI eventData)
		{
			if (fireKeys.Contains (eventData.TheKey))
			{
				FireKeyReleased ();
			}
		}

		private void FireKeyClicked ()
		{
			//Debug.Log ("Key Clicked");
			_keysClickedCount++;
		}

		private void FireKeyReleased ()
		{
			//Debug.Log ("Key Released");
			_keysClickedCount--;
		}


		private void FireIfRequested ()
		{
			if (_keysClickedCount <= 0 && _keysClickedThisFrame.Count == 0)
				return;

			defaultShooter.FireProjectile ();

			_timer_framesSinceFire = 0;
		}

	}//End Class


}//End Namespace
