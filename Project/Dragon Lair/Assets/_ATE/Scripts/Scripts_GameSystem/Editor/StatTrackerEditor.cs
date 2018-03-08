using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using Ate.GameSystems;
using Ate.EditorHelpers;


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

			EditorHelper.DrawResizableList ("Used Stats", ref _target.sceneStats, DrawEntry_StatData);
		}

		private void DrawEntry_StatData (int index)
		{
			StatTracker.StatData curStat = _target.sceneStats[index];

			curStat.id = (TrackedStatType)EditorGUILayout.EnumPopup ("ID", curStat.id);

			curStat.startValue   = EditorGUILayout.FloatField ("Start Value", curStat.startValue);
			//	Only change if not playing. Otherwise if watching values the current
			//	value constantly gets overwritten by the default value.
			if (!EditorApplication.isPlaying)
				curStat.currentValue = curStat.startValue;

			EditorGUILayout.BeginHorizontal ();
			EditorGUILayout.LabelField ("Current: " + curStat.currentValue, GUILayout.Width (100));
			EditorGUILayout.LabelField ("Persistent: " + curStat.persistentValue, GUILayout.Width (100));
			EditorGUILayout.EndHorizontal ();
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

				StatTracker.StatData existingStat = GetStatDataOfType (_target.sceneStats, curEnumVals[i]);
				newStats.Add (existingStat != null ? existingStat : new StatTracker.StatData (curEnumVals[i], 0));
			}

			_target.sceneStats.Clear ();
			_target.sceneStats = newStats;
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
