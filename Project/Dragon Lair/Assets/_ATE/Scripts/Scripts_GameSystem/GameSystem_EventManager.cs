using UnityEngine;
using UnityEngine.Networking;
using System.Collections;


//TODO:	Needs reworked with EventSystem to better seperate control
public class GameSystem_EventManager : GameSystem
{
	#region Event Systems

	private EventSystem<EventType_Updates, EventData_Updates>   _updateEventSystem    = new EventSystem<EventType_Updates,  EventData_Updates> ();
	private EventSystem<EventType_UI, EventData_UI>             _uiEventSystem        = new EventSystem<EventType_UI,       EventData_UI> ();
	private EventSystem<EventType_Gameplay, EventData_Gameplay> _gameplayEventSystem  = new EventSystem<EventType_Gameplay, EventData_Gameplay> ();

	private EventSystem<EventType_Trigger, EventData_Trigger>     _triggerEventSystem   = new EventSystem<EventType_Trigger,   EventData_Trigger> ();
	private EventSystem<EventType_Collision, EventData_Collision> _collisionEventSystem = new EventSystem<EventType_Collision, EventData_Collision> ();

	#endregion


	#region GameSystem

	public override void Initialize ()
	{
		_updateEventSystem    = new EventSystem<EventType_Updates,  EventData_Updates> ();
		_uiEventSystem        = new EventSystem<EventType_UI,       EventData_UI> ();
		_gameplayEventSystem  = new EventSystem<EventType_Gameplay, EventData_Gameplay> ();

		_triggerEventSystem   = new EventSystem<EventType_Trigger,   EventData_Trigger> ();
		_collisionEventSystem = new EventSystem<EventType_Collision, EventData_Collision> ();
	}

	public override void SceneInitialize (){}
	public override void SystemUpdate (){}
	public override void SystemLateUpdate (){}

	#endregion


	//TODO: GREATLY NEEDS REVAMPED
	//TODO: This is a (hopefully) temporary, and awkward, solution
	//TODO: for having one method in this class that can register
	//TODO: with any of the event systems.
	//TODO: GREATLY NEEDS REVAMPED
	/// <summary>
	/// Register an event callback for specified eventType.
	/// EType is the enum to cast eventType into.
	/// </summary>
	public void Register<EType, DType> (int eventType, Callback<DType> callback) where DType : EventData
	{
		if (typeof(EType) == typeof(EventType_Updates))
			_updateEventSystem.Register ((EventType_Updates)eventType, (callback as Callback<EventData_Updates>));
		
		else if (typeof(EType) == typeof(EventType_UI))
			_uiEventSystem.Register ((EventType_UI)eventType, (callback as Callback<EventData_UI>));
		
		else if (typeof(EType) == typeof(EventType_Gameplay))
			_gameplayEventSystem.Register ((EventType_Gameplay)eventType, (callback as Callback<EventData_Gameplay>));

		else if (typeof(EType) == typeof(EventType_Trigger))
			_triggerEventSystem.Register ((EventType_Trigger)eventType, (callback as Callback<EventData_Trigger>));

		else if (typeof(EType) == typeof(EventType_Collision))
			_collisionEventSystem.Register ((EventType_Collision)eventType, (callback as Callback<EventData_Collision>));
	}


	/// <summary>
	/// Unregister an event callback for specified eventType.
	/// EType is the enum to cast eventType into.
	/// </summary>
	public void Unregister<EType, DType> (int eventType, Callback<DType> callback) where DType : EventData
	{
		if (typeof(EType) == typeof(EventType_Updates))
			_updateEventSystem.Unregister ((EventType_Updates)eventType, (callback as Callback<EventData_Updates>));

		else if (typeof(EType) == typeof(EventType_UI))
			_uiEventSystem.Unregister ((EventType_UI)eventType, (callback as Callback<EventData_UI>));

		else if (typeof(EType) == typeof(EventType_Gameplay))
			_gameplayEventSystem.Unregister ((EventType_Gameplay)eventType, (callback as Callback<EventData_Gameplay>));

		else if (typeof(EType) == typeof(EventType_Trigger))
			_triggerEventSystem.Unregister ((EventType_Trigger)eventType, (callback as Callback<EventData_Trigger>));

		else if (typeof(EType) == typeof(EventType_Collision))
			_collisionEventSystem.Unregister ((EventType_Collision)eventType, (callback as Callback<EventData_Collision>));
	}


	/// <summary>
	/// Broadcasts an event with data for specified eventType.
	/// EType is the enum to cast eventType into.
	/// </summary>
	public void Broadcast<EType> (int eventType, EventData eventData, GameObject sender = null)
	{
		if (typeof(EType) == typeof(EventType_Updates))
			_updateEventSystem.Broadcast ((EventType_Updates)eventType, (eventData as EventData_Updates), sender);

		else if (typeof(EType) == typeof(EventType_UI))
			_uiEventSystem.Broadcast ((EventType_UI)eventType, (eventData as EventData_UI), sender);

		else if (typeof(EType) == typeof(EventType_Gameplay))
			_gameplayEventSystem.Broadcast ((EventType_Gameplay)eventType, (eventData as EventData_Gameplay), sender);

		else if (typeof(EType) == typeof(EventType_Trigger))
			_triggerEventSystem.Broadcast ((EventType_Trigger)eventType, (eventData as EventData_Trigger), sender);

		else if (typeof(EType) == typeof(EventType_Collision))
			_collisionEventSystem.Broadcast ((EventType_Collision)eventType, (eventData as EventData_Collision), sender);
	}
	

	private void Cleanup ()
	{
		_updateEventSystem   = new EventSystem<EventType_Updates,  EventData_Updates> ();
		_uiEventSystem       = new EventSystem<EventType_UI,       EventData_UI> ();
		_gameplayEventSystem = new EventSystem<EventType_Gameplay, EventData_Gameplay> ();

		_triggerEventSystem   = new EventSystem<EventType_Trigger,   EventData_Trigger> ();
		_collisionEventSystem = new EventSystem<EventType_Collision, EventData_Collision> ();
	}

}

