using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace Ate
{


	public class EventData_UI : EventData
	{
		#region Private Fields

		private KeyCode val_keyCode_one;

		private Vector3 val_vector3_one = new Vector3 ();
		private Vector3 val_vector3_two = new Vector3 ();

		private Object  val_object_one = null;

		#endregion


		#region Properties

		public KeyCode TheKey
		{
			get
			{
				return val_keyCode_one;
			}

			set
			{
				val_keyCode_one = value;
			}
		}

		public Vector3 MousePosition
		{
			get
			{
				return val_vector3_one;
			}

			set
			{
				val_vector3_one = value;
			}
		}

		public Vector3 LastMousePosition
		{
			get
			{
				return val_vector3_one;
			}

			set
			{
				val_vector3_one = value;
			}
		}

		public Vector3 NewMousePosition
		{
			get
			{
				return val_vector3_two;
			}

			set
			{
				val_vector3_two = value;
			}
		}

		public Object ClickedObject
		{
			get
			{
				return val_object_one;
			}

			set
			{
				val_object_one = value;
			}
		}

		#endregion


		public EventData_UI ()
		{
			
		}

		public EventData_UI (Vector3 screenCoords)
		{
			this.val_vector3_one = screenCoords;
		}

		public EventData_UI (KeyCode theKey, Vector3 screenCoords)
		{
			this.TheKey          = theKey;
			this.val_vector3_one = screenCoords;
		}

		public EventData_UI (KeyCode theKey, Vector3 screenCoords, Object clickedObject)
		{
			this.TheKey          = theKey;
			this.val_vector3_one = screenCoords;
			this.val_object_one  = clickedObject;
		}

	}//End Class


}//End Namespace

