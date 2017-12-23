using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Ate.ScriptGeneration;


namespace Ate.GameSystems
{


	public class StatTracker : GameSystem
	{
		//TODO: Make it enum based not string based
		//TODO: Make it abstract with custom inspectors and things
		//		So the value can be an int, float, or whatever
		[System.Serializable]
		public class StatData
		{
			public TrackedStatType id = TrackedStatType.None;

			public float value = 0;


			public StatData ()
			{
				this.id = TrackedStatType.None;
				this.value = 0;
			}

			public StatData (TrackedStatType id, float value)
			{
				this.id = id;
				this.value = value;
			}
		}

		public enum ModType
		{
			Set  = 0,
			Add  = 10,
			Mult = 20,
		}


		public GenerateEnum_Data enumData = new GenerateEnum_Data ();

		public List<StatData> stats = new List<StatData> ();


		#region GameSystem

		public override void Initialize (){}
		public override void SceneInitialize (){}
		public override void SystemUpdate (){}
		public override void SystemLateUpdate (){}

		#endregion


		/// <summary>
		/// Gets the stat value.
		/// If no stat exists, returns float.MinValue.
		/// </summary>
		public float GetStatValue (TrackedStatType statType)
		{
			StatData theStat = GetStat (statType);
			if (theStat == null)
				return float.MinValue;
			
			return theStat.value;
		}

		/// <summary>
		/// Returns the stat matching the given string.
		/// Returns null if no stat matches.
		/// </summary>
		public StatData GetStat (TrackedStatType statType)
		{
			for (int i = 0; i < stats.Count; i++)
			{
				if (stats[i].id.Equals (statType))
					return stats[i];
			}

			return null;
		}

		//TODO: Setup to be through events, that will make
		//		it MUCH easier to do things like update text
		//		displaying a given stat.
		#region ModStats

		public void ModStat (TrackedStatType statType, ModType modType, float value)
		{
			StatData theStat = GetStat (statType);
			if (theStat == null)
			{
				#if UNITY_EDITOR
				Debug.LogError ("StatTracker.ModStat() could not find stat with given name: " + statType.ToString ());
				#endif
				return;
			}

			float oldStat = theStat.value;

			switch (modType)
			{
				case ModType.Set :
					theStat.value = value;
					break;
				case ModType.Add :
					theStat.value += value;
					break;
				case ModType.Mult :
					theStat.value *= value;
					break;
			}

			float newStat = theStat.value;

			EventData_Gameplay eventData = new EventData_Gameplay ();
			eventData.OldStatValue = oldStat;
			eventData.NewStatValue = newStat;
			GameManager.Events.Broadcast<EventType_Gameplay> ((int)EventType_Gameplay.StatTracker_StatModified, eventData);
		}

		#endregion

	}//End Class


}//End Namespace
