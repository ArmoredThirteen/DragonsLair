using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif


[System.Serializable]
public class IndexChooser
{
	public enum ChooseType
	{
		/// <summary> Goes from 0 to indexCount-1, then rolls over. </summary>
		InOrder = 0,
		/// <summary> Chooses any random index. </summary>
		Random_Repeating = 100,
		/// <summary>
		/// Chooses a random index that hasn't been chosen yet.
		/// Resets after all entries are chosen once.
		/// </summary>
		Random_NoRepeats = 150,
	}


	public int indexCount = 0;

	public ChooseType indexChooseType = ChooseType.InOrder;


	private int _curIndex = 0;

	private List<int> chosenIndexes = new List<int> ();



	#region GetIndexData

	/// <summary>
	/// Returns the entry in dataList at the current index.
	/// If the entry is out of range or otherwise improper, returns defaultValue instead.
	/// If automaticIncrement is true, increments the index after finding a valid entry at curIndex.
	/// </summary>
	public T GetCurIndexData<T> (List<T> dataList, T defaultValue = default(T), bool automaticIncrement = false)
	{
		if (dataList == null)
			return defaultValue;
		if (dataList.Count <= 0)
			return defaultValue;

		int curIndex = GetCurIndex (false);
		if (curIndex < 0)
			return defaultValue;
		if (curIndex >= dataList.Count)
			return defaultValue;

		if (automaticIncrement)
			IncrementIndex ();

		return dataList[curIndex];
	}

	/// <summary>
	/// Gets the current index. If automaticIncrement is true,
	/// increments the index after storing the current index
	/// (which, of course, is returned after incrementing).
	/// </summary>
	public int GetCurIndex (bool automaticIncrement = false)
	{
		int curIndex = _curIndex;

		if (automaticIncrement)
			IncrementIndex ();

		return curIndex;
	}

	#endregion


	#region Initializing

	/// <summary>
	/// Should be called before the first index is chosen.
	/// While not detrimental if it isn't called, it means
	/// on random repeat index 0 will be called first.
	/// </summary>
	public void Initialize ()
	{
		switch (indexChooseType)
		{
			case ChooseType.InOrder :
				Initialize_InOrder ();
				break;
			case ChooseType.Random_Repeating :
				Initialize_RandomRepeating ();
				break;
			case ChooseType.Random_NoRepeats :
				Initialize_RandomNoRepeats ();
				break;
		}
	}

	private void Initialize_InOrder ()
	{
		_curIndex = 0;
	}

	private void Initialize_RandomRepeating ()
	{
		IncrementIndex_RandomRepeating ();
	}

	private void Initialize_RandomNoRepeats ()
	{
		IncrementIndex_RandomNoRepeats ();
	}

	#endregion


	#region Incrementing

	public void IncrementIndex ()
	{
		switch (indexChooseType)
		{
			case ChooseType.InOrder :
				IncrementIndex_InOrder ();
				break;
			case ChooseType.Random_Repeating :
				IncrementIndex_RandomRepeating ();
				break;
			case ChooseType.Random_NoRepeats :
				IncrementIndex_RandomNoRepeats ();
				break;
		}
	}

	private void IncrementIndex_InOrder ()
	{
		_curIndex = (_curIndex+1) % indexCount;
	}

	private void IncrementIndex_RandomRepeating ()
	{
		_curIndex = Random.Range (0, indexCount);
	}

	private void IncrementIndex_RandomNoRepeats ()
	{
		//	Put at top to prevent an infinite loop with a size 0 indexCount.
		if (chosenIndexes.Count >= indexCount)
		{
			chosenIndexes.Clear ();
			//	If there are a few possible indexes, seed the new starting chosenIndexes
			//	with the last _curIndex. This is to prevent the same result from rolling
			//	twice in a row when the possible indexes rolls over.
			if (indexCount >= 3)
			{
				chosenIndexes.Add (_curIndex);
			}
		}
		
		_curIndex = Random.Range (0, indexCount);
		while (chosenIndexes.Contains (_curIndex))
		{
			_curIndex = Random.Range (0, indexCount);
		}

		chosenIndexes.Add (_curIndex);
	}

	#endregion


	#if UNITY_EDITOR

	public void DrawIndexChooser ()
	{
		indexCount = EditorGUILayout.IntField ("Index Count", indexCount);

		indexChooseType = (ChooseType)EditorGUILayout.EnumPopup ("Index Choose Type", indexChooseType);
	}

	#endif
}
