using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif


/// <summary>
/// Description
/// </summary>
public class AteComponent_Test : AteComponent
{
	
	#region Public Variables

	#endregion


	#region Private Variables

	#endregion


	#if UNITY_EDITOR

	public override void DrawInspector ()
	{
		base.DrawInspector ();
	}

	#endif


	#region AteGameObject

	protected override void AteUpdate ()
	{
		
	}

	#endregion


	#region Public Methods

	#endregion


	#region Private Methods

	#endregion

}
