using UnityEngine;
using System.Collections;


namespace Ate
{


	public class Example_EventUser : AteComponent
	{
		
		protected override void RegisterEvents ()
		{
			GameManager.Events.Register<EventType_Updates, EventData_Updates> ((int)EventType_Updates.UpdateTwo, SecondUpdate);
		}

		protected override void UnregisterEvents ()
		{
			Debug.LogError ("Destroyed After 3 Seconds");
			GameManager.Events.Unregister<EventType_Updates, EventData_Updates> ((int)EventType_Updates.UpdateTwo, SecondUpdate);
		}

		protected override void AteUpdate ()
		{
			Debug.Log ("AteUpdate");
			if (Time.time > 3)
				Destroy (this.gameObject);
		}

		private void SecondUpdate (EventData_Updates eventData)
		{
			Debug.Log ("Second Update");
		}

	}//End Class


}//End Namespace
