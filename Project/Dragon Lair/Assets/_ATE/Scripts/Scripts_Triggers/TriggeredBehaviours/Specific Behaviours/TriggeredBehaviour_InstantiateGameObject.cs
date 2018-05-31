using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif


namespace Ate
{


	public class TriggeredBehaviour_InstantiateGameObject : TriggeredBehaviour
	{
		public enum TargetType
		{
			//Default = 0,
			Self    = 100,
			Target  = 200,
			TargetByName = 300,
		}

		//	Variables for designers.
		//	Shown in editor with DrawChildInspector() at bottom.
		#region Public Variables

		public GameObject objectToInstantiate = null;

		public TargetType parentTargetType   = TargetType.TargetByName;
		public TargetType locationTargetType = TargetType.Self;

		public Transform parentTransform   = null;
		public Transform locationTransform = null;

		public string parentName   = "Container_Default";
		public string locationName = "DefaultLocation";

		#endregion


		#region Private Variables

		#endregion


		#if UNITY_EDITOR

		/// <summary>
		/// Called by parent class for drawing specific variables at top.
		/// Parent class should automatically check for when it is dirty.
		/// </summary>
		protected override void DrawChildInspector ()
		{
			objectToInstantiate = EditorGUILayout.ObjectField
				("Instantiate Prefab", objectToInstantiate, typeof(GameObject), false)
				as GameObject;

			parentTargetType   = (TargetType)EditorGUILayout.EnumPopup ("Parent Object",        parentTargetType);
			locationTargetType = (TargetType)EditorGUILayout.EnumPopup ("Instantiate Location", locationTargetType);

			parentTransform = EditorGUILayout.ObjectField
				("Parent Transform", parentTransform, typeof(Transform), true)
				as Transform;

			locationTransform = EditorGUILayout.ObjectField
				("Location Transform", locationTransform, typeof(Transform), true)
				as Transform;

			parentName   = EditorGUILayout.TextField ("Parent Name",   parentName);
			locationName = EditorGUILayout.TextField ("Location Name", locationName);
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
			InitializeParentTransform ();
			InitializeLocationTransform ();
		}

		private void InitializeParentTransform ()
		{
			//	Only TargetByName needs to initialize data
			if (parentTargetType != TargetType.TargetByName)
				return;

			GameObject theObj = GameObject.Find (parentName);

			if (theObj != null)
				parentTransform = theObj.transform;

			if (parentTransform = null)
				parentTargetType = TargetType.Self;
		}

		private void InitializeLocationTransform ()
		{
			//	Only TargetByName needs to initialize data
			if (locationTargetType != TargetType.TargetByName)
				return;

			GameObject theObj = GameObject.Find (locationName);

			if (theObj != null)
				locationTransform = theObj.transform;

			if (locationTransform = null)
				locationTargetType = TargetType.Self;
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
			SetParentTransform ();
			SetLocationTransform ();

			GameObject gameObj = GameObject.Instantiate (objectToInstantiate);
			gameObj.SetPosition (locationTransform.position);
			gameObj.transform.SetParent (parentTransform);

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


		private void SetParentTransform ()
		{
			switch (parentTargetType)
			{
				case TargetType.Self:
					parentTransform = MyTransform;
					break;

				case TargetType.Target:
					// Don't change parentTransform, it was designer-set
					break;

				case TargetType.TargetByName:
					// Don't change locationTransform, it was set during AteAwake()
					break;
			}
		}

		private void SetLocationTransform ()
		{
			switch (locationTargetType)
			{
				case TargetType.Self:
					locationTransform = MyTransform;
					break;

				case TargetType.Target:
					// Don't change locationTransform, it was designer-set
					break;

				case TargetType.TargetByName:
					// Don't change locationTransform, it was set during AteAwake()
					break;
			}
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
