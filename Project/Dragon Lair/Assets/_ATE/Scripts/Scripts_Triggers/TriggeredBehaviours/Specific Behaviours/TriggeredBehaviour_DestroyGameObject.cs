using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif


namespace Ate
{


	public class TriggeredBehaviour_DestroyGameObject : TriggeredBehaviour
	{
		public enum DestroyType
		{
			Target    = 0,
			Self      = 50,
			Triggerer = 100,
		}

		//	Variables for designers.
		//	Shown in editor with DrawInspector() at bottom.
		#region Public Variables

		public DestroyType destroyType = DestroyType.Target;

		public GameObject targetGameObject;

		#endregion


		#if UNITY_EDITOR

		/// <summary>
		/// Called by parent class for drawing specific variables at top.
		/// Parent class should automatically check for when it is dirty.
		/// </summary>
		protected override void DrawChildInspector ()
		{
			destroyType = (DestroyType)EditorGUILayout.EnumPopup ("Destroy Type", destroyType);

			switch (destroyType)
			{
				case DestroyType.Target :
					DrawDestroyType_Target ();
					break;
				case DestroyType.Self :
					DrawDestroyType_Self ();
					break;
				case DestroyType.Triggerer :
					DrawDestroyType_Triggerer ();
					break;
			}
		}

		private void DrawDestroyType_Target ()
		{
			targetGameObject = EditorGUILayout.ObjectField
				("Target", targetGameObject, typeof (GameObject), true)
				as GameObject;
		}

		private void DrawDestroyType_Self ()
		{

		}

		private void DrawDestroyType_Triggerer ()
		{

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
			//TODO: HACK way to switch targets
			//TODO: HACK way to switch targets
			//TODO: HACK way to switch targets
			switch (destroyType)
			{
				case DestroyType.Target :
					//	Target should be set by designer already
					break;
				case DestroyType.Self :
					targetGameObject = gameObject;
					break;
				case DestroyType.Triggerer :
					targetGameObject = triggerer == null ? null : triggerer.gameObject;
					break;
			}
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
			if (targetGameObject == null)
			{
				//Debug.Log ("targetGameObject is Null");
				return;
			}

			//	Called at end of this method for an instant-fire behaviour
			RequestComplete ();

			//	Normally code goes BEFORE RequestComplete()
			//	But because this can destroy itself, the RequestComplete()
			//		is called above to maintain anything relying on it.

			//Debug.Log ("Destroying: " + targetGameObject.name_ID ());
			Destroy (targetGameObject.gameObject);
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
