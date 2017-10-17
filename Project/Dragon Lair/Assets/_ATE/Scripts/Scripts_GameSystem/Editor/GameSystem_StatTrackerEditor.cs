using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;


namespace Ate
{


	[CustomEditor (typeof(GameSystem_StatTracker))]
	public class GameSystem_StatTrackerEditor : Editor
	{
		private GameSystem_StatTracker _target;


		public override void OnInspectorGUI()
		{
			_target = (GameSystem_StatTracker)target;
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
			GameSystem_StatTracker.StatData curStat = _target.stats[index];

			curStat.id    = (TrackedStatType)EditorGUILayout.EnumPopup ("ID", curStat.id);
			curStat.value = EditorGUILayout.FloatField ("Start Amount", curStat.value);
		}


		private void RebuildUsedStats ()
		{
			List<GameSystem_StatTracker.StatData> newStats = new List<GameSystem_StatTracker.StatData> ();

			List<TrackedStatType> curEnumVals = new List<TrackedStatType>
				((TrackedStatType[])System.Enum.GetValues (typeof (TrackedStatType)));

			for (int i = 0; i < curEnumVals.Count; i++)
			{
				if (GetStatDataOfType (newStats, curEnumVals[i]) != null)
					continue;

				GameSystem_StatTracker.StatData existingStat = GetStatDataOfType (_target.stats, curEnumVals[i]);
				newStats.Add (existingStat != null ? existingStat : new GameSystem_StatTracker.StatData (curEnumVals[i], 0));
			}

			_target.stats.Clear ();
			_target.stats = newStats;
		}

		private GameSystem_StatTracker.StatData GetStatDataOfType (List<GameSystem_StatTracker.StatData> datas, TrackedStatType statType)
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
