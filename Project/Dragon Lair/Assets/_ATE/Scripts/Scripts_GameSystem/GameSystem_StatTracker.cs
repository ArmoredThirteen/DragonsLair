using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class GameSystem_StatTracker : GameSystem
{
	//TODO: Make it enum based not string based
	//TODO: Make it abstract with custom inspectors and things
	//		So the value can be an int, float, or whatever
	[System.Serializable]
	public class Stat
	{
		public string name = "Stat";
		public float value = 0;
	}

	public enum ModType
	{
		Set  = 0,
		Add  = 10,
		Mult = 20,
	}


	public List<Stat> stats = new List<Stat> ();


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
	public float GetStatValue (string name)
	{
		Stat theStat = GetStat (name);
		if (theStat == null)
			return float.MinValue;
		
		return theStat.value;
	}

	/// <summary>
	/// Returns the stat matching the given string.
	/// Returns null if no stat matches.
	/// </summary>
	public Stat GetStat (string name)
	{
		for (int i = 0; i < stats.Count; i++)
		{
			if (stats[i].name.Equals (name))
				return stats[i];
		}

		return null;
	}

	//TODO: Setup to be through events, that will make
	//		it MUCH easier to do things like update text
	//		displaying a given stat.
	#region ModStats

	public void ModStat (string name, ModType modType, float value)
	{
		Stat theStat = GetStat (name);
		if (theStat == null)
		{
			#if UNITY_EDITOR
			Debug.LogError ("StatTracker.ModStat() could not find stat with given name: " + name);
			#endif
			return;
		}
		
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
	}

	#endregion

}

