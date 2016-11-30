using UnityEngine;
using System.Collections;


/// <summary>
/// For storing delegates that will be specified during runtime.
/// </summary>
public abstract class ClassDelegates
{
	public abstract void AssignLogic (ClassLogic theLogic);
	public abstract void ClearDelegates ();
}
