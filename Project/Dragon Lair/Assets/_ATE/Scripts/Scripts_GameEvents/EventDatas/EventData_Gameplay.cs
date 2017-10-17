using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace Ate
{


	public class EventData_Gameplay : EventData
	{
		private int _intArgOne;

		private float _floatArgOne;


		/// <summary>
		/// For scene-loading events. Uses _intArgOne.
		/// </summary>
		public int SceneIndex
		{
			get {return _intArgOne;}
			set {_intArgOne = value;}
		}

		/// <summary>
		/// For movement speed changing events.
		/// </summary>
		public float WalkSpeed
		{
			get {return _floatArgOne;}
			set {_floatArgOne = value;}
		}
	}//End Class


}//End Namespace

