using UnityEngine;
using System.Collections;
//using System;


namespace Ate
{


	/// <summary>
	/// Basically a Tuple.
	/// </summary>
	public class Pair<T1, T2>
	{
		public T1 v1;
		public T2 v2;

		public Pair ()
		{
			this.v1 = default (T1);
			this.v2 = default (T2);
		}

		public Pair (T1 v1, T2 v2)
		{
			this.v1 = v1;
			this.v2 = v2;
		}

	}


	/// <summary>
	/// Shorthand Pair where both types are the same.
	/// </summary>
	public class PairPod<T> : Pair<T, T>
	{
		public PairPod () : base () {}
		public PairPod (T v1, T v2) : base (v1, v2) {}
	}


	/// <summary>
	/// PairPod<float> with some range functionality.
	/// Had Range() method for quickly accessing Random.Range().
	/// ExtFloat has an IsInRange() with a FloatRange parameter.
	/// </summary>
	[System.Serializable]
	public class FloatRange : PairPod<float>
	{
		public FloatRange () : base () {}
		public FloatRange (float min, float max) : base (min, max) {}
		
		public float Range ()
		{
			return Random.Range (v1, v2);
		}

		/// <summary>
		/// Returns true if val is greater than min
		/// and less than max, both inclusive.
		/// </summary>
		public bool WithinRange (float val)
		{
			return (val >= v1) && (val <= v2);
		}
	}


	/*public class KeyPair<T1, T2> : Pair<T1, T2>//, System.IComparable
		//where T1 : System.IComparable<T1>
	{
		public int CompareTo (T1 compareVal)
		{
			//return v1.CompareTo (compareVal);
		}

		public bool IsKey (T1 key)
		{
			//return CompareTo (key) == 0;
			//return v1.Equals ()
		}
	}*/


	/*public class EnumerablePair<T1, T2> : Pair<T1, T2>, System.IEquatable<EnumerablePair<T1, T2>>
	{
		public EnumerablePair () : base () {}
		public EnumerablePair (T1 v1, T2 v2) : base (v1, v2) {}
		
		public override int GetHashCode ()
		{
			return v1.GetHashCode () ^ v2.GetHashCode ();
		}
		
		public override bool Equals (object obj)
		{
			if (obj == null || obj.GetType () != this.GetType ())
			{
				return false;
			}
			
			return Equals ((EnumerablePair<T1, T2>)obj);
		}
		
		public bool Equals (EnumerablePair<T1, T2> other)
		{
			return (this.v1.Equals (other.v1)) && (this.v2.Equals (other.v2));
		}
	}*/

	/*public class EnumerablePairPod<T> : EnumerablePair<T, T>
	{
		public EnumerablePairPod () : base () {}
		public EnumerablePairPod (T v1, T v2) : base (v1, v2) {}
	}*/


	//End Classes


}//End Namespace
