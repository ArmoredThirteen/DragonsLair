

namespace Ate
{


	public enum EventType_Gameplay
	{
		None = 0,

		StatTracker_StatModified = 100,

		PoolObject   = 200,
		UnpoolObject = 210,

		//DestroyObject = 300,

		KillObject   = 400,
		ReviveObject = 410,

		ResetPlayerWalkSpeed = 500,
		SetPlayerWalkSpeed   = 510,

		SceneLoad   = 1000,
		SceneReload = 1010,
	}


}//End Namespace

