using UnityEngine;
using System.Collections;
using Ate.GameSystems;

#if UNITY_EDITOR
using UnityEditor;
#endif


namespace Ate
{
	
	
	/// <summary>
	/// Description
	/// </summary>
	public class ChangeFramelength : AteComponent
	{
		
		#region Public Variables

		public KeyCode increaseKey = KeyCode.KeypadPlus;
		public KeyCode decreaseKey = KeyCode.KeypadMinus;

		#endregion


		#region Private Variables

		UpdateBroadcaster broadcaster = null;

		#endregion


		#if UNITY_EDITOR

		public override void DrawInspector ()
		{
			base.DrawInspector ();

			increaseKey = (KeyCode)EditorGUILayout.EnumPopup ("Increase Key", increaseKey);
			decreaseKey = (KeyCode)EditorGUILayout.EnumPopup ("Decrease Key", decreaseKey);
		}

		#endif


		#region AteComponent

		protected override void RegisterEvents ()
		{
			base.RegisterEvents ();

			GameManager.Events.Register<EventType_UI, EventData_UI>
				((int)EventType_UI.Clicked, OnKeyClicked);
		}

		protected override void UnregisterEvents ()
		{
			base.UnregisterEvents ();

			GameManager.Events.Unregister<EventType_UI, EventData_UI>
				((int)EventType_UI.Clicked, OnKeyClicked);
		}


		protected override void AteStart ()
		{
			base.AteStart ();

			broadcaster = GameManager.GetGameSystem <UpdateBroadcaster> ();
		}

		protected override void AteUpdate ()
		{
			
		}

		#endregion


		#region Public Methods

		#endregion


		#region Private Methods

		private void OnKeyClicked (EventData_UI eventData)
		{
			KeyCode theKey = eventData.TheKey;

			if (theKey == increaseKey)
				ModFramelength (1);
			else if (theKey == decreaseKey)
				ModFramelength (-1);
		}

		private void ModFramelength (int amount)
		{
			broadcaster.ModUniversalFramelength (amount);
		}

		#endregion

	}//End Class
	
	
}//End Namespace
