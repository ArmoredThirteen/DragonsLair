using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace Ate
{


	public class EventData_Gameplay : EventData
	{
		private int _intArgOne;

		private float _floatArgOne;
		private float _floatArgTwo;

		private TrackedStatType _trackedStatTypeOne;


		#region Scene Loading

		/// <summary>
		/// For scene-loading events. Uses _intArgOne.
		/// </summary>
		public int SceneIndex
		{
			get {return _intArgOne;}
			set {_intArgOne = value;}
		}

		#endregion


		#region Movement Speed

		/// <summary>
		/// For movement speed changing events.
		/// </summary>
		public float WalkSpeed
		{
			get {return _floatArgOne;}
			set {_floatArgOne = value;}
		}

		#endregion


		#region StatTracker

		/// <summary>
		/// The stat that was modified during a stat change event.
		/// </summary>
		public TrackedStatType TheStatType
		{
			get {return _trackedStatTypeOne;}
			set {_trackedStatTypeOne = value;}
		}

		/// <summary>
		/// For StatTracker value changes.
		/// Value before the change happened.
		/// </summary>
		public float OldStatValue
		{
			get {return _floatArgOne;}
			set {_floatArgOne = value;}
		}

		/// <summary>
		/// For StatTracker value changes.
		/// Value after the change happened.
		/// </summary>
		public float NewStatValue
		{
			get {return _floatArgTwo;}
			set {_floatArgTwo = value;}
		}

		#endregion


	}//End Class


}//End Namespace

