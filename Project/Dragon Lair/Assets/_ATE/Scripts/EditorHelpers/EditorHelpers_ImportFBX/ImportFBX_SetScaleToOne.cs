
#if UNITY_EDITOR


using UnityEngine;
using UnityEditor;
using System.Collections;


namespace Ate
{
	

	public class ImportFBX_SetScaleToOne : AssetPostprocessor
	{
		
		//	http://answers.unity3d.com/questions/30408/how-can-i-change-the-scale-size-of-the-fbx-importe.html
		void OnPreprocessModel ()
		{
			ModelImporter modelImporter = assetImporter as ModelImporter;

			if (modelImporter.globalScale != 1)
			{
				Debug.Log ("<color=green>" + modelImporter.name + ":</color> Setting model scale to One");
				modelImporter.globalScale = 1;
			}
		}

	}//End Class


}//End Namespace


#endif
