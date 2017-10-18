using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Ate.GameSystems;

#if UNITY_EDITOR
using UnityEditor;
using Ate.EditorHelpers;
#endif


namespace Ate
{


	public sealed class AteObject : MonoBehaviour
	{

		#region Fields

		public GOType type = GOType.None;

		public List<AteComponent> components = new List<AteComponent> ();


		#if UNITY_EDITOR
		private bool _canRemoveComponents = false;
		public bool CanRemoveComponents ()
		{
			return _canRemoveComponents;
		}

		private string _filter = "";
		private int _index_addChoice = 0;
		#endif

		#endregion


		#if UNITY_EDITOR

		/// <summary>
		/// Draws the inspector.
		/// </summary>
		public void DrawInspector ()
		{
			EditorGUILayout.LabelField (gameObject.name);
			type = (GOType)EditorGUILayout.EnumPopup ("Game Object Type", type);

			EditorGUILayout.Space ();

			DrawComponentRemover ();

			EditorGUILayout.Space ();

			DrawComponentAdder ();
		}

		private void DrawComponentRemover ()
		{
			_canRemoveComponents = EditorGUILayout.Toggle ("Can Remove Components", _canRemoveComponents);
		}

		private void DrawComponentAdder ()
		{
			EditorGUILayout.LabelField ("Component Adder");

			_filter = EditorGUILayout.TextField ("Filter", _filter);

			EditorGUILayout.BeginHorizontal ();

			List<string> choices = GetFilteredChoices ();

			//	Might be unnecessary
			if (_index_addChoice >= choices.Count)
				_index_addChoice = Mathf.Max (0, choices.Count-1);
			
			_index_addChoice = EditorGUILayout.Popup (_index_addChoice, choices.ToArray ());

			if (GUILayout.Button ("Add"))
			{
				Type theType = Type.GetType (choices[_index_addChoice]);
				AddComponent (theType);
			}

			EditorGUILayout.EndHorizontal ();
		}

		private List<string> GetFilteredChoices ()
		{
			List<string> choices = Editor_ListComponents_OnCompile.Components;
			if (string.IsNullOrEmpty (_filter))
				return choices;

			string lowerFilter = _filter.ToLower ();

			//	Remove any that don't contain _filter
			for (int i = choices.Count-1; i >= 0; i--)
			{
				if (choices[i].ToLower ().Contains (lowerFilter))
					continue;
				
				choices.RemoveAt (i);
			}

			return choices;
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
		}

		void Start ()
		{
			RegisterEvents ();
			RegisterTracking ();
		}

		void OnDestroy ()
		{
			UnregisterEvents ();
			UnregisterTracking ();
		}

		#endregion


		#region Base Methods

		public void AddComponent (Type theType)
		{
			AteComponent theComponent = gameObject.AddComponent (theType) as AteComponent;
			if (theComponent == null)
				return;

			components.Add (theComponent);
			theComponent.SetMyObject (this);
		}

		/// <summary>
		/// Called by components when they are removed.
		/// </summary>
		public void DestroyComponent (AteComponent theComponent)
		{
			components.Remove (theComponent);
			GameObject.DestroyImmediate (theComponent);
		}


		private void RegisterEvents ()
		{
			GameManager.Events.Register<EventType_Updates, EventData_Updates>
				((int)EventType_Updates.UpdateOne, ManagedUpdateOne);
			GameManager.Events.Register<EventType_Updates, EventData_Updates>
				((int)EventType_Updates.LateUpdateOne, ManagedLateUpdateOne);
		}

		private void UnregisterEvents ()
		{
			GameManager.Events.Unregister<EventType_Updates, EventData_Updates>
				((int)EventType_Updates.UpdateOne, ManagedUpdateOne);
			GameManager.Events.Unregister<EventType_Updates, EventData_Updates>
				((int)EventType_Updates.LateUpdateOne, ManagedLateUpdateOne);
		}


		/// <summary>
		/// Registers object with GameSystem_GameObjectTracker.
		/// </summary>
		private void RegisterTracking ()
		{
			GameManager.ObjectTracker.RegisterAteObject (this);
		}

		/// <summary>
		/// Unregisters object with GameSystem_GameObjectTracker.
		/// </summary>
		private void UnregisterTracking ()
		{
			GameManager.ObjectTracker.UnregisterAteObject (this);
		}

		#endregion


		/// <summary>
		/// Registered Update Event, controlled by the UpdatesBroadcaster.
		/// </summary>
		private void ManagedUpdateOne (EventData_Updates eventData)
		{
			for (int i = 0; i < components.Count; i++)
			{
				components[i].ManagedUpdateOne (eventData);
			}
		}

		/// <summary>
		/// Registered LateUpdate Event, controlled by the UpdatesBroadcaster.
		/// </summary>
		private void ManagedLateUpdateOne (EventData_Updates eventData)
		{
			for (int i = 0; i < components.Count; i++)
			{
				components[i].ManagedLateUpdateOne (eventData);
			}
		}

	}//End Class


}//End Namespace
