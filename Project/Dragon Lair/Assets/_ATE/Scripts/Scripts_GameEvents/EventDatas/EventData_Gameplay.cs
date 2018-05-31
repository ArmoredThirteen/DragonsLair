using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Ate.Pooling;


namespace Ate
{


	public class EventData_Gameplay : EventData
	{
		private int _ateGoInstanceID;

		private int _intArgOne;

		private float _floatArgOne;
		private float _floatArgTwo;

		private bool _boolArgOne;

		private TrackedStatType _trackedStatTypeOne;


		#region General

		/// <summary>
		/// For object ID for any object-specific event. Uses _ateGoInstanceID.
		/// </summary>
		public int AteGOInstanceID
		{
			get {return _ateGoInstanceID;}
			set {_ateGoInstanceID = value;}
		}

		#endregion


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


		#region Pooling

		/// <summary>
		/// For object pooling. Uses _intArgOne.
		/// </summary>
		public PoolID PoolType
		{
			get {return (PoolID)_intArgOne;}
			set {_intArgOne = (int)value;}
		}

		#endregion


		#region Pooling and Killing

		/// <summary>
		/// For object pooling or killing. If true, transition stages
		/// are entered and resolved the same frame as the calll.
		/// </summary>
		public bool IsImmediate
		{
			get {return _boolArgOne;}
			set {_boolArgOne = value;}
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

