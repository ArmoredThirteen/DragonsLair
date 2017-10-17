using UnityEngine;
using System.Collections;


namespace Ate
{


	/// <summary>
	/// For storing delegates that will be specified during runtime.
	/// </summary>
	public abstract class ClassDelegates
	{
		public abstract void AssignLogic (ClassLogic theLogic);
		public abstract void ClearDelegates ();
	}//End Class


}//End Namespace
