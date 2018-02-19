using UnityEngine;
using System.Collections;
using Ate.Pooling;

#if UNITY_EDITOR
using UnityEditor;
#endif


namespace Ate.Spawning
{
	
	
	/// <summary>
	/// Description
	/// </summary>
	public abstract class Spawner : AteComponent
	{
		
		#region Public Variables

		public PoolID theID = PoolID.None;

		#endregion


		#region Private Variables

		#endregion


		#if UNITY_EDITOR

		public override void DrawInspector ()
		{
			base.DrawInspector ();

			theID = (PoolID)EditorGUILayout.EnumPopup ("Pool ID", theID);
		}

		#endif


		#region AteComponent

		#endregion


		#region Public Methods

		public abstract void Spawn ();

		#endregion


		#region Private Methods

		#endregion

	}//End Class
	
	
}//End Namespace
