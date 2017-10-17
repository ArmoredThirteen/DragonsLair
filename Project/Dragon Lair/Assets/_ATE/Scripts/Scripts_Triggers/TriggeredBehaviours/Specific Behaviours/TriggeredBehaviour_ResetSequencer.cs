using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif


namespace Ate
{


	public class TriggeredBehaviour_ResetSequencer : TriggeredBehaviour
	{
		public enum ResetType
		{
			/// <summary>
			/// To reset data to defaults.
			/// Includes at least rest counts, play requests,
			/// and rebuilding the FSM.
			/// </summary>
			DataReset,

			/// <summary>
			/// To request putting FSMs from Completed to Ready again.
			/// </summary>
			PlayReset,
		}

		//	Variables for designers.
		//	Shown in editor with DrawInspector() at bottom.
		#region Public Variables

		public ResetType resetType = ResetType.DataReset;

		public List<TriggeredBehaviour_Sequencer> sequencers = new List<TriggeredBehaviour_Sequencer> ();

		#endregion


		#if UNITY_EDITOR

		/// <summary>
		/// Called by parent class for drawing specific variables at top.
		/// Parent class should automatically check for when it is dirty.
		/// </summary>
		protected override void DrawChildInspector ()
		{
			resetType = (ResetType)EditorGUILayout.EnumPopup ("Reset Type", resetType);
			EditorHelper.DrawResizableList<TriggeredBehaviour_Sequencer>
			("Sequencers", ref sequencers, DrawEntry_ActionBundle);
		}

		private void DrawEntry_ActionBundle (int index)
		{
			sequencers[index] = EditorGUILayout.ObjectField
				(("Sequence #"+index), sequencers[index], typeof (TriggeredBehaviour_Sequencer), true)
				as TriggeredBehaviour_Sequencer;
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
			switch (resetType)
			{
				case ResetType.DataReset:
					for (int i = 0; i < sequencers.Count; i++)
					{
						sequencers[i].DataReset ();
					}
					break;
				case ResetType.PlayReset:
					for (int i = 0; i < sequencers.Count; i++)
					{
						sequencers[i].RequestReset ();
					}
					break;
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

		#endregion

	}//End Class


}//End Namespace
