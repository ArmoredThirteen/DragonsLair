using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;

#if UNITY_EDITOR
using UnityEditor;
#endif


namespace Ate
{


	/// <summary>
	/// Creates a list of AteComponent classes after every Unity compile.
	/// Mainly for use with inspectors that want an AteComponent dropdown.
	/// </summary>
	[InitializeOnLoad]
	public static class Editor_ListComponents_OnCompile
	{

		static Editor_ListComponents_OnCompile ()
		{
			RebuildComponentsList ();
		}


		private static List<string> _ateComponents = new List<string> ();

		public static List<string> Components
		{
			get
			{
				return new List<string> (_ateComponents);
			}
		}


		[UnityEditor.Callbacks.DidReloadScripts]
		private static void RebuildComponentsList ()
		{
			_ateComponents.Clear ();

			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies ();
			for (int i = 0; i < assemblies.Length; i++)
			{
				Type[] types = assemblies[i].GetTypes ();
				for (int k = 0; k < types.Length; k++)
				{
					if (types[k].IsSubclassOf (typeof(AteComponent)))
					{
						_ateComponents.Add (types[k].ToString ());
					}
				}
			}
		}

	}//End Class


}//End Namespace
