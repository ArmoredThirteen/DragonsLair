﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Ate.Hud;


namespace Ate.GameSystems
{


	public class HudText : GameSystem
	{
		public List<TextBoxSettings> hudBoxSettings = new List<TextBoxSettings> ();

		private List<HudTextBox> _hudTextBoxes = new List<HudTextBox> ();
		private int _nextHudTextID = int.MinValue;


		public override void Initialize ()
		{
			_nextHudTextID = int.MinValue;

			for (int i = 0; i < hudBoxSettings.Count; i++)
			{
				_hudTextBoxes.Add (new HudTextBox (hudBoxSettings[i]));
			}
		}

		public override void SceneLoaded (){}
		public override void SystemUpdate (){}
		public override void SystemLateUpdate (){}


		void OnGUI ()
		{
			for (int i = 0; i < _hudTextBoxes.Count; i++)
			{
				_hudTextBoxes[i].DrawGUI ();
			}
		}


		/// <summary>
		/// Clears all the text within a specified HudTextBoxType.
		/// </summary>
		public void ClearHudTextBox (TextBoxType boxType)
		{
			HudTextBox theBox = GetHudTextOfType (boxType);
			theBox.RemoveAllText ();
		}


		public int AddHudText (TextBoxType boxType, string text)
		{
			HudTextBox theBox = GetHudTextOfType (boxType);
			//TODO: DANGEROUS HARD-CODED NUMBER
			//TODO: DANGEROUS HARD-CODED NUMBER
			if (theBox == null)
			{
				Debug.LogError ("Attempted AddHudText but could not find requested HudTextBoxType: " + boxType.ToString ());
				//TODO: DANGEROUS HARD-CODED NUMBER
				return int.MaxValue;
			}
			//TODO: DANGEROUS HARD-CODED NUMBER
			//TODO: DANGEROUS HARD-CODED NUMBER
			
			int ID = _nextHudTextID;
			_nextHudTextID++;

			theBox.AddText (ID, text);

			return ID;
		}

		public void RemoveHudText (TextBoxType boxType, int ID)
		{
			HudTextBox theBox = GetHudTextOfType (boxType);
			if (theBox == null)
				return;

			theBox.RemoveText (ID);
		}


		private HudTextBox GetHudTextOfType (TextBoxType boxType)
		{
			for (int i = 0; i < _hudTextBoxes.Count; i++)
			{
				if (_hudTextBoxes[i].boxType == boxType)
					return _hudTextBoxes[i];
			}

			return null;
		}
	}//End Class


}//End Namespace
