

namespace Ate
{
	

	/// <summary>
	/// Available TriggeredBehaviour types.
	/// </summary>
	public enum TriggeredBehaviourType
	{
		None = 0,

		DebugLog = 100,

		AddHudText    = 1000,
		RemoveHudText = 1010,
		ClearHudText  = 1020,

		ModifyActionActivity = 1500,

		ResetActionBundle = 1600,

		SceneLoad   = 5000,
		SceneReload = 5010,
	}


}//End Namespace
