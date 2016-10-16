using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[System.Serializable]
public class HudTextBoxSettings
{
	public HudTextBoxType boxType = HudTextBoxType.Informative;

	public bool areaRelativeToScreen = true;
	public Rect textArea;

	public GUIStyle guiStyle;
}


public class HudTextBox
{
	#region Set By HudTextBoxSettings

	public HudTextBoxType boxType;

	public bool areaRelativeToScreen;
	public Rect textArea;

	public GUIStyle guiStyle;

	#endregion

	private string _curText = "";
	private List<HudTextData> _textDatas = new List<HudTextData> ();


	public HudTextBox (HudTextBoxSettings boxSettings)
	{
		this.boxType = boxSettings.boxType;

		this.areaRelativeToScreen = boxSettings.areaRelativeToScreen;
		this.textArea = boxSettings.textArea;

		this.guiStyle = boxSettings.guiStyle;
	}


	public void AddText (int ID, string text)
	{
		_textDatas.Add (new HudTextData (ID, text));

		RefreshHudText ();
	}


	public void RemoveAllText ()
	{
		_textDatas.Clear ();
		RefreshHudText ();
	}

	public void RemoveText (int ID)
	{
		for (int i = 0; i < _textDatas.Count; i++)
		{
			if (_textDatas[i].ID == ID)
			{
				_textDatas.RemoveAt (i);
				break;
			}
		}

		RefreshHudText ();
	}


	/// <summary>
	/// Rebuilds the _curText string based on all the _textDatas.
	/// Adds a newline between each text data.
	/// </summary>
	public void RefreshHudText ()
	{
		_curText = "";

		for (int i = 0; i < _textDatas.Count; i++)
		{
			_curText += _textDatas[i].text;

			if (i != _textDatas.Count - 1)
				_curText += "\r\n";
		}
	}


	public void DrawGUI ()
	{
		if (!areaRelativeToScreen)
		{
			GUI.Label (textArea, _curText);
			return;
		}

		//TODO: Optimized to be cached somehow
		//	Doing it like this for now so it live updates resolution changes
		float x      = textArea.x      * Screen.width;
		float y      = textArea.y      * Screen.height;
		float width  = textArea.width  * Screen.width;
		float height = textArea.height * Screen.height;

		Rect relativeTextArea = new Rect (x, y, width, height);
		GUI.Label (relativeTextArea, _curText, guiStyle);
	}
}
