using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
using Ate.EditorHelpers;
#endif


namespace Ate.Players
{
	
	
	/// <summary>
	/// Description
	/// </summary>
	public class PlayerShipMovement : AteComponent
	{
		
		#region Public Variables

		FollowMethod.FollowType followType = FollowMethod.FollowType.None;

		FollowMethodData followData   = new FollowMethodData   ();
		FollowMethod     followMethod = null;

		#endregion


		#region Private Variables

		#endregion


		#if UNITY_EDITOR

		public override void DrawInspector ()
		{
			base.DrawInspector ();

			FollowMethod.FollowType newType = (FollowMethod.FollowType)EditorGUILayout.EnumPopup ("Follow Type", followType);
			ChangeFollowType (newType);

			EditorGUILayout.Space ();
			DrawFollowMethod ();
		}


		private void DrawFollowMethod ()
		{
			switch (followType)
			{
				case FollowMethod.FollowType.None:
					DrawFollowMethod_None ();
					break;

				case FollowMethod.FollowType.Mouse:
					DrawFollowMethod_Mouse ();
					break;

				case FollowMethod.FollowType.Keyboard:
					DrawFollowMethod_Keyboard ();
					break;

				case FollowMethod.FollowType.Object:
					DrawFollowMethod_Object ();
					break;
			}
		}

		private void DrawFollowMethod_None ()
		{
			EditorGUILayout.LabelField ("Follow Method is null, object will remain stationary.");
		}

		private void DrawFollowMethod_Mouse ()
		{
			followData.moveDistancePerFrame = EditorGUILayout.FloatField ("Move Dist / Frame", followData.moveDistancePerFrame);
		}

		private void DrawFollowMethod_Keyboard ()
		{
			EditorGUILayout.LabelField ("Unimplemented");
		}

		private void DrawFollowMethod_Object ()
		{
			EditorGUILayout.LabelField ("Unimplemented");
		}

		#endif


		#region AteComponent

		protected override void AteUpdate ()
		{
			
		}

		#endregion


		#region Public Methods

		public void ChangeFollowType (FollowMethod.FollowType newType)
		{
			followType = newType;

			switch (newType)
			{
				case FollowMethod.FollowType.None:
					ChangeFollowType_None ();
					break;

				case FollowMethod.FollowType.Mouse:
					ChangeFollowType_Mouse ();
					break;

				case FollowMethod.FollowType.Keyboard:
					ChangeFollowType_Keyboard ();
					break;

				case FollowMethod.FollowType.Object:
					ChangeFollowType_Object ();
					break;
			}
		}

		#endregion


		#region Private Methods

		private void ChangeFollowType_None ()
		{
			followMethod = null;
		}

		private void ChangeFollowType_Mouse ()
		{
			followMethod = new FollowMethod_Mouse ();
		}

		private void ChangeFollowType_Keyboard ()
		{
			//followMethod = new FollowMethod_Keyboard ();
		}

		private void ChangeFollowType_Object ()
		{
			//followMethod = new FollowMethod_Object ();
		}

		#endregion

	}//End Class
	
	
}//End Namespace
