using UnityEngine;
using System.Collections;
using Ate.GameSystems;

#if UNITY_EDITOR
using UnityEditor;
#endif


namespace Ate.GameText
{
	
	
	/// <summary>
	/// Description
	/// </summary>
	public class SetTextObject_StatTrackerChange : AteComponent
	{
		
		#region Public Variables

		public TextObject textLine = null;
		public TrackedStatType trackedStat = TrackedStatType.None;

		#endregion


		#region Private Variables

		#endregion


		#if UNITY_EDITOR

		public override void DrawInspector ()
		{
			base.DrawInspector ();

			textLine = EditorGUILayout.ObjectField
				("Text Line", textLine, typeof(TextObject))
				as TextObject;
			
			trackedStat = (TrackedStatType)EditorGUILayout.EnumPopup ("Tracked Stat", trackedStat);
		}

		#endif


		#region AteComponent

		protected override void RegisterEvents ()
		{
			GameManager.Events.Register<EventType_Gameplay, EventData_Gameplay>
				((int)EventType_Gameplay.StatTracker_StatModified, OnStatChanged);
		}

		protected override void UnregisterEvents ()
		{
			GameManager.Events.Unregister<EventType_Gameplay, EventData_Gameplay>
				((int)EventType_Gameplay.StatTracker_StatModified, OnStatChanged);
		}


		protected override void AteUpdate ()
		{
			
		}

		#endregion


		#region Public Methods

		#endregion


		#region Private Methods

		private void OnStatChanged (EventData_Gameplay eventData)
		{
			if (eventData.TheStatType != trackedStat)
				return;
			
			//TODO: SetIndexesFromString is inefficient, refrain from using a lot
			textLine.theString.SetIndexesFromString (textLine.theAlphabet, eventData.NewStatValue.ToString ());
			textLine.UpdateText ();
		}

		#endregion

	}//End Class
	
	
}//End Namespace
