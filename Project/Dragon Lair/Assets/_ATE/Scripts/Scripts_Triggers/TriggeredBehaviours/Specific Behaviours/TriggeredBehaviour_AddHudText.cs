using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Ate.GameSystems;
using Ate.Hud;

#if UNITY_EDITOR
using UnityEditor;
using Ate.EditorHelpers;
#endif


namespace Ate
{


	public class TriggeredBehaviour_AddHudText : TriggeredBehaviour
	{
		//	Variables for designers.
		//	Shown in editor with DrawInspector() at bottom.
		#region Public Variables

		public enum TextOrder
		{
			AllAtOnce = 0,
			Random    = 100,

			//InOrder_Once      = 110,
			//InOrder_RepeatEnd = 120,
			InOrder_RepeatAll  = 130,
		}

		public TextBoxType boxType = TextBoxType.Informative;
		public TextOrder textOrder = TextOrder.AllAtOnce;
		public List<string> texts = new List<string> ();

		public List<TriggeredBehaviour_RemoveHudText> sendTextIDsForRemoval = new List<TriggeredBehaviour_RemoveHudText> ();

		#endregion


		#if UNITY_EDITOR

		/// <summary>
		/// Called by parent class for drawing specific variables at top.
		/// Parent class should automatically check for when it is dirty.
		/// </summary>
		protected override void DrawChildInspector ()
		{
			boxType = (TextBoxType)EditorGUILayout.EnumPopup ("Box Type", boxType);
			textOrder = (TextOrder)EditorGUILayout.EnumPopup ("Text Order", textOrder);

			bool drawList_texts             = true;
			bool drawList_sendIDsForRemoval = true;

			EditorHelper.DrawResizableList<string> ("Texts", ref drawList_texts, ref texts, DrawEntry_Text);
			EditorHelper.DrawResizableList<TriggeredBehaviour_RemoveHudText>
				("Send Removal IDs", ref drawList_sendIDsForRemoval, ref sendTextIDsForRemoval, DrawEntry_SendForRemoval);
		}

		private void DrawEntry_Text (int index)
		{
			texts[index] = EditorGUILayout.TextField (("Text #"+index), texts[index]);
		}

		private void DrawEntry_SendForRemoval (int index)
		{
			sendTextIDsForRemoval[index] = EditorGUILayout.ObjectField
				("Send ID to", sendTextIDsForRemoval[index], typeof (TriggeredBehaviour_RemoveHudText), true)
				as TriggeredBehaviour_RemoveHudText;
		}

		#endif


		#region Awake/Start

		/// <summary>
		/// Called by parent class at the end of its AteAwake().
		/// </summary>
		protected override void OnAwake ()
		{
			
		}

		/// <summary>
		/// Called by AteObject at end of its Awake().
		/// </summary>
		protected override void AteStart ()
		{
			
		}

		#endregion


		#region OnRequested

		/// <summary>
		/// For resetting to a more 'factory default' version.
		/// For things like only playing a sequencer once, then
		/// resetting it from a different behaviour somewhere
		/// else so it can be played again.
		/// </summary>
		protected override void OnDataReset()
		{

		}


		/// <summary>
		/// Called when parent class had a request to play.
		/// If inactive and cancelRequestsWhileInactive is true, won't be called.
		/// </summary>
		protected override void OnRequestedPlaying (AteObject triggerer)
		{

		}

		/// <summary>
		/// Called when parent class had a request to complete.
		/// If inactive and cancelRequestsWhileInactive is true, won't be called.
		/// </summary>
		protected override void OnRequestedComplete ()
		{

		}

		/// <summary>
		/// Called when parent class had a request to reset.
		/// If inactive and cancelRequestsWhileInactive is true, won't be called.
		/// </summary>
		protected override void OnRequestedPlayReset ()
		{

		}

		#endregion


		#region OnEntered

		/// <summary>
		/// Called when behaviour enters the Ready state.
		/// Currently it starts in Ready, but the enter callback
		/// only happens when it switches to Ready.
		/// So for now it can be thought more as 'OnReset'.
		/// </summary>
		protected override void OnEnteredReady (TriggeredState prevState)
		{
			
		}

		/// <summary>
		/// Called when behaviour enters the Playing state.
		/// For instant-fire behaviours, this is where 99% of the logic will go.
		/// </summary>
		protected override void OnEnteredPlaying (TriggeredState prevState)
		{
			List<string> nextTexts = new List<string> ();

			switch (textOrder)
			{
				case TextOrder.AllAtOnce :
					nextTexts = texts;
					break;
				case TextOrder.Random :
					nextTexts.Add (texts[Random.Range (0, (texts.Count))]);
					break;
				case TextOrder.InOrder_RepeatAll :
					nextTexts.Add (texts[ResetCount % texts.Count]);
					break;
			}

			for (int i = 0; i < nextTexts.Count; i++)
			{
				int textID = GameManager.HudText.AddHudText (boxType, nextTexts[i]);
				SendTextIDForRemoval (textID);
			}

			//	Called at end of this method for an instant-fire behaviour
			RequestComplete ();
		}

		/// <summary>
		/// Called when behaviour enters the Complete state.
		/// Happens after a RequestComplete() call and CanSwitchToComplete is true.
		/// </summary>
		protected override void OnEnteredComplete (TriggeredState prevState)
		{
			
		}

		#endregion


		#region OnUpdate

		/// <summary>
		/// Called every frame behaviour is in the Ready state.
		/// </summary>
		protected override void OnUpdateReady ()
		{
			
		}

		/// <summary>
		/// Called every frame behaviour is in the Playing state.
		/// For over-time behaviours, this is where most of the logic will go.
		/// </summary>
		protected override void OnUpdatePlaying ()
		{
			//	Called when an end-condition happens (such as a timer)
			//RequestComplete ();
		}

		/// <summary>
		/// Called every frame behaviour is in the Complete state.
		/// This will happen after Playing until it is Reset or canceled.
		/// </summary>
		protected override void OnUpdateComplete ()
		{
			
		}

		#endregion


		#region CanSwitch

		/// <summary>
		/// After parent class determines if a switch was requested,
		/// it uses this as an extra check if it can switch yet.
		/// </summary>
		protected override bool CanSwitchToPlaying ()
		{
			return true;
		}

		/// <summary>
		/// After parent class determines if a switch was requested,
		/// it uses this as an extra check if it can switch yet.
		/// </summary>
		protected override bool CanSwitchToComplete ()
		{
			return true;
		}

		/// <summary>
		/// After parent class determines if a switch was requested,
		/// it uses this as an extra check if it can switch yet.
		/// </summary>
		protected override bool CanPlayReset ()
		{
			return true;
		}

		#endregion


		#region Helper Methods

		private void SendTextIDForRemoval (int textID)
		{
			if (sendTextIDsForRemoval == null)
				return;
			if (sendTextIDsForRemoval.Count <= 0)
				return;

			for (int i = 0; i < sendTextIDsForRemoval.Count; i++)
			{
				//	Happens from unclean design
				if (sendTextIDsForRemoval[i] == null)
					continue;

				sendTextIDsForRemoval[i].AddIDToRemoveList (textID);
			}
		}

		#endregion

	}//End Class


}//End Namespace
