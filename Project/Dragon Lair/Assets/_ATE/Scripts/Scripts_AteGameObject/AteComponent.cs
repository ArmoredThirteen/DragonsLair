using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif


namespace Ate
{


	[RequireComponent(typeof(AteObject))]
	public abstract class AteComponent : MonoBehaviour
	{

		#region Fields

		/// <summary>
		/// The controlling AteObject.
		/// Made public for serialization reasons.
		/// Other scripts should use MyObject and SetMyObject().
		/// </summary>
		public AteObject _myObject = null;

		#endregion


		#if UNITY_EDITOR

		/// <summary>
		/// Draws the inspector.
		/// Child classes overriding this should call base.DrawInspector() at the top.
		/// </summary>
		public virtual void DrawInspector ()
		{
			//	Shouldn't happen if designers are using the custom add/remove buttons
			if (_myObject == null)
				_myObject = gameObject.GetComponent<AteObject> () as AteObject;

			if (_myObject == null)
				return;
			if (_myObject.CanRemoveComponents ())
				DrawRemoveOption ();
		}

		private void DrawRemoveOption ()
		{
			if (GUILayout.Button ("Remove Component"))
			{
				_myObject.DestroyComponent (this);
				return;
			}
		}

		#endif


		#region Properties

		public AteObject MyObject
		{
			get {return _myObject;}
		}

		#if UNITY_EDITOR
		public void SetMyObject (AteObject myObject)
		{
			_myObject = myObject;
		}
		#endif

		public int InstanceID
		{
			get { return MyObject.InstanceID; }
		}

		public int GOInstanceID
		{
			get { return MyObject.GOInstanceID; }
		}


		/// <summary>
		/// Cached copy of gameObject.transform for performance.
		/// Cached during Awake(), and is safe to use in AteAwake().
		/// </summary>
		public Transform MyTransform
		{
			get { return MyObject.MyTransform; }
		}

		/// <summary>
		/// The position of cached MyTransform.
		/// Safe to use during AteAwake(), but not Awake().
		/// </summary>
		public Vector3 Position
		{
			get { return MyObject.Position; }
			set { MyObject.Position = value; }
		}

		/// <summary>
		/// The local position of cached MyTransform.
		/// Safe to use during AteAwake(), but not Awake().
		/// </summary>
		public Vector3 LocalPosition
		{
			get { return MyObject.LocalPosition; }
			set { MyObject.LocalPosition = value; }
		}

		/// <summary>
		/// The rotation of cached MyTransform.
		/// Safe to use during AteAwake(), but not Awake().
		/// </summary>
		public Quaternion Rotation
		{
			get { return MyObject.Rotation; }
			set { MyObject.Rotation = value; }
		}

		/// <summary>
		/// The local rotation of cached MyTransform.
		/// Safe to use during AteAwake(), but not Awake().
		/// </summary>
		public Quaternion LocalRotation
		{
			get { return MyObject.LocalRotation; }
			set { MyObject.LocalRotation = value; }
		}

		#endregion


		#region MonoBehaviour

		public void Awake ()
		{
			AteAwake ();
		}

		void Start ()
		{
			BaseRegisterEvents ();
			AteStart ();
		}

		void OnDestroy ()
		{
			BaseUnregisterEvents ();
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
			/*GameManager.Events.Register<EventType_Updates, EventData_Updates>
				((int)EventType_Updates.UpdateOne, ManagedUpdateOne);
			GameManager.Events.Register<EventType_Updates, EventData_Updates>
				((int)EventType_Updates.LateUpdateOne, ManagedLateUpdateOne);*/


			RegisterEvents ();
		}

		private void BaseUnregisterEvents ()
		{
			/*GameManager.Events.Unregister<EventType_Updates, EventData_Updates>
				((int)EventType_Updates.UpdateOne, ManagedUpdateOne);
			GameManager.Events.Unregister<EventType_Updates, EventData_Updates>
				((int)EventType_Updates.LateUpdateOne, ManagedLateUpdateOne);*/

			UnregisterEvents ();
		}

		#endregion


		/// <summary>
		/// Called by the controlling AteObject.
		/// That AteObject is responding to EventManager events.
		/// </summary>
		public void ManagedUpdateOne (EventData_Updates eventData)
		{
			AteUpdate ();
		}

		/// <summary>
		/// Called by the controlling AteObject.
		/// That AteObject is responding to EventManager events.
		/// </summary>
		public void ManagedLateUpdateOne (EventData_Updates eventData)
		{
			AteLateUpdate ();
		}

	}//End Class


}//End Namespace
