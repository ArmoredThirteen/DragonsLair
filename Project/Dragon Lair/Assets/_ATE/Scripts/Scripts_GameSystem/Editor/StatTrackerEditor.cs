using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using Ate.GameSystems;


namespace Ate.GameSystems
{


	[CustomEditor (typeof(StatTracker))]
	public class StatTrackerEditor : Editor
	{
		private StatTracker _target;


		public override void OnInspectorGUI()
		{
			_target = (StatTracker)target;
			if (_target == null)
				return;

			_target.enumData.DrawInspector ();

			EditorGUILayout.Space ();
			EditorGUILayout.Space ();

			DrawUsedStats ();

			EditorHelper.SetDirtyIfChanged (_target);
		}


		private void DrawUsedStats ()
		{
			if (GUILayout.Button ("Rebuild From Enum"))
			{
				RebuildUsedStats ();
				EditorHelper.UnfocusControl ();
			}

			EditorHelper.DrawResizableList ("Used Stats", ref _target.stats, DrawEntry_StatData);
		}

		private void DrawEntry_StatData (int index)
		{
			StatTracker.StatData curStat = _target.stats[index];

			curStat.id    = (TrackedStatType)EditorGUILayout.EnumPopup ("ID", curStat.id);
			curStat.value = EditorGUILayout.FloatField ("Start Amount", curStat.value);
		}


		private void RebuildUsedStats ()
		{
			List<StatTracker.StatData> newStats = new List<StatTracker.StatData> ();

			List<TrackedStatType> curEnumVals = new List<TrackedStatType>
				((TrackedStatType[])System.Enum.GetValues (typeof (TrackedStatType)));

			for (int i = 0; i < curEnumVals.Count; i++)
			{
				if (GetStatDataOfType (newStats, curEnumVals[i]) != null)
					continue;

				StatTracker.StatData existingStat = GetStatDataOfType (_target.stats, curEnumVals[i]);
				newStats.Add (existingStat != null ? existingStat : new StatTracker.StatData (curEnumVals[i], 0));
			}

			_target.stats.Clear ();
			_target.stats = newStats;
		}

		private StatTracker.StatData GetStatDataOfType (List<StatTracker.StatData> datas, TrackedStatType statType)
		{
			for (int i = 0; i < datas.Count; i++)
			{
				if (datas[i].id == statType)
					return datas[i];
			}

			return null;
		}

	}//End Class


}//End Namespace
