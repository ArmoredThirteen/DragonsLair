using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif


namespace Ate.Pooling
{
	
	public enum PoolID
	{
		/// <summary>
		/// Default value. If not changed, pool systems will ignore this one.
		/// </summary>
		None = 0,

		Player = 100,

		Enemy_01 = 200,

		Projectile_Player  = 300,
		Projectile_Enemy01 = 310,
	}
	
}//End Namespace
