using UnityEngine;
using System.Collections;
using Ate.GameSystems;

#if UNITY_EDITOR
using UnityEditor;
#endif


namespace Ate.Pooling
{
	
	
	/// <summary>
	/// Description
	/// </summary>
	public abstract class PoolableObject : AteComponent
	{

		[System.Serializable]
		public abstract class PO_Data
		{
			#if UNITY_EDITOR
			public abstract void DrawInspector ();
			#endif
		}


		#region Public Variables

		public PoolID theID = PoolID.None;

		/*public bool startPooled = true;
		public PoolID theCategory = PoolID.None;*/

		#endregion


		#region Private Variables

		/*private PoolManager _poolManager;
		private bool _isPooled;*/

		#endregion


		#if UNITY_EDITOR

		public override void DrawInspector ()
		{
			base.DrawInspector ();

			/*startPooled = EditorGUILayout.Toggle ("Start Pooled", startPooled);
			theCategory = (PoolID)EditorGUILayout.EnumPopup ("Pool Category", theCategory);*/

			theID = (PoolID)EditorGUILayout.EnumPopup ("Pool ID", theID);
		}

		#endif


		#region AteComponent

		protected override void AteAwake ()
		{
			//_poolManager = GameManager.GetGameSystem<PoolManager> ();
		}

		protected override void AteStart ()
		{
			/*if (startPooled)
			{
				PoolRequested (true);
			}
			else
			{
				UnpoolRequested (true);
			}*/
		}


		protected override void AteUpdate ()
		{
			
		}

		#endregion


		#region PoolableObject

		/*
		/// <summary>
		/// Called whenever a system (generally through events) requests this object be pooled.
		/// </summary>
		private void PoolRequested (bool ignoreIsPooled = false)
		{
			//	Already pooled
			if (!ignoreIsPooled && _isPooled)
				return;

			_isPooled = true;
			Pool ();
		}

		/// <summary>
		/// Called whenever a system (generally through events) requests this object be unpooled.
		/// </summary>
		private void UnpoolRequested (bool ignoreIsPooled = false)
		{
			//	Already unpooled
			if (!ignoreIsPooled && !_isPooled)
				return;

			_isPooled = false;
			Unpool ();
		}
		*/


		/*
		#region PoolableObject

		public override void Pool ()
		{
			base.Pool ();
		}

		public override void Unpool ()
		{
			base.Unpool ();
		}

		#endregion
		*/
		public virtual void Pool ()
		{
			gameObject.SetActive (false);
		}

		public virtual void Unpool ()
		{
			gameObject.SetActive (true);
		}

		#endregion


		#region Public Methods

		#endregion


		#region Private Methods

		/*private void RegisterEvents ()
		{
			base.RegisterEvents ();

			GameManager.Events.Register<EventType_Gameplay, EventData_Gameplay> ((int)EventType_Gameplay.PoolObject,   OnPoolObject);
			GameManager.Events.Register<EventType_Gameplay, EventData_Gameplay> ((int)EventType_Gameplay.UnpoolObject, OnUnpoolObject);
		}

		private void UnregisterEvents ()
		{
			base.UnregisterEvents ();

			GameManager.Events.Unregister<EventType_Gameplay, EventData_Gameplay> ((int)EventType_Gameplay.PoolObject,   OnPoolObject);
			GameManager.Events.Unregister<EventType_Gameplay, EventData_Gameplay> ((int)EventType_Gameplay.UnpoolObject, OnUnpoolObject);
		}


		private void OnPoolObject (EventData_Gameplay eventData)
		{
			//	Event target is a different object
			if (eventData.AteObjectID != GOInstanceID)
				return;

			PoolRequested ();
		}

		private void OnUnpoolObject (EventData_Gameplay eventData)
		{
			//	Event target is a different object
			if (eventData.AteObjectID != GOInstanceID)
				return;

			UnpoolRequested ();
		}*/

		#endregion

	}//End Class
	
	
}//End Namespace
