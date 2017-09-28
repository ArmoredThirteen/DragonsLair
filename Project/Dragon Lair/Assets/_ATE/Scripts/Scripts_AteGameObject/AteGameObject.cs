using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif


public abstract class AteGameObject : MonoBehaviour
{
	#region Fields

	public GOType type = GOType.None;

	#endregion


	#if UNITY_EDITOR

	/// <summary>
	/// Draws the inspector.
	/// Child classes overriding this should call base.DrawInspector() at the top.
	/// </summary>
	public virtual void DrawInspector ()
	{
		EditorGUILayout.LabelField (gameObject.name);
		type = (GOType)EditorGUILayout.EnumPopup ("Game Object Type", type);
	}

	#endif


	#region Properties

	public int InstanceID
	{
		get { return this.GetInstanceID (); }
	}

	public int GOInstanceID
	{
		get { return gameObject.GetInstanceID (); }
	}


	/// <summary>
	/// Cached copy of gameObject.transform for performance.
	/// Cached during Awake(), and is safe to use in AteAwake().
	/// </summary>
	public Transform MyTransform
	{
		get; private set;
	}

	/// <summary>
	/// The position of cached MyTransform.
	/// Safe to use during AteAwake(), but not Awake().
	/// </summary>
	public Vector3 Position
	{
		get { return MyTransform.position; }
		set { MyTransform.position = value; }
	}

	/// <summary>
	/// The local position of cached MyTransform.
	/// Safe to use during AteAwake(), but not Awake().
	/// </summary>
	public Vector3 LocalPosition
	{
		get { return MyTransform.localPosition; }
		set { MyTransform.localPosition = value; }
	}

	/// <summary>
	/// The rotation of cached MyTransform.
	/// Safe to use during AteAwake(), but not Awake().
	/// </summary>
	public Quaternion Rotation
	{
		get { return MyTransform.rotation; }
		set { MyTransform.rotation = value; }
	}

	/// <summary>
	/// The local rotation of cached MyTransform.
	/// Safe to use during AteAwake(), but not Awake().
	/// </summary>
	public Quaternion LocalRotation
	{
		get { return MyTransform.localRotation; }
		set { MyTransform.localRotation = value; }
	}

	#endregion


	#region MonoBehaviour

	public void Awake ()
	{
		//	Caching component calls, must happen before AteAwake()
		MyTransform = transform;

		AteAwake ();
	}

	void Start ()
	{
		BaseRegisterEvents ();
		BaseRegisterTracking ();
		AteStart ();
	}

	void OnDestroy ()
	{
		BaseUnregisterEvents ();
		BaseUnregisterTracking ();
	}

	#endregion


	#region Abstract Methods

	protected abstract void AteUpdate ();

	#endregion


	#region Virtual Methods

	protected virtual void AteAwake (){}
	protected virtual void AteStart (){}

	protected virtual void AteLateUpdate (){}

	protected virtual void RegisterEvents (){}
	protected virtual void UnregisterEvents (){}

	#endregion


	#region Base Methods

	private void BaseRegisterEvents ()
	{
		GameManager.Events.Register<EventType_Updates, EventData_Updates>
			((int)EventType_Updates.UpdateOne, ManagedUpdateOne);
		GameManager.Events.Register<EventType_Updates, EventData_Updates>
			((int)EventType_Updates.LateUpdateOne, ManagedLateUpdateOne);


		RegisterEvents ();
	}

	private void BaseUnregisterEvents ()
	{
		GameManager.Events.Unregister<EventType_Updates, EventData_Updates>
			((int)EventType_Updates.UpdateOne, ManagedUpdateOne);
		GameManager.Events.Unregister<EventType_Updates, EventData_Updates>
			((int)EventType_Updates.LateUpdateOne, ManagedLateUpdateOne);

		UnregisterEvents ();
	}


	/// <summary>
	/// Registers object with GameSystem_GameObjectTracker.
	/// </summary>
	private void BaseRegisterTracking ()
	{
		//GameManager.ObjectTracker.RegisterAteGameObject (this);
	}

	/// <summary>
	/// Unregisters object with GameSystem_GameObjectTracker.
	/// </summary>
	private void BaseUnregisterTracking ()
	{
		//GameManager.ObjectTracker.UnregisterAteGameObject (this);
	}

	#endregion


	/// <summary>
	/// Registered Update Event, controlled by the UpdatesBroadcaster.
	/// </summary>
	private void ManagedUpdateOne (EventData_Updates eventData)
	{
		AteUpdate ();
	}

	/// <summary>
	/// Registered LateUpdate Event, controlled by the UpdatesBroadcaster.
	/// </summary>
	private void ManagedLateUpdateOne (EventData_Updates eventData)
	{
		AteLateUpdate ();
	}

}
