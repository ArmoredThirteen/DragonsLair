

namespace Ate
{


	public enum EventType_Collision
	{
		None = 0,

		RegisterCollider   = 100,
		UnregisterCollider = 150,
		RequestColliderRegistration = 500,

		CollisionBegan     = 1000,
		CollisionContinued = 1100,
		CollisionEnded     = 1200,

		AreaCollisionBegan     = 3000,
		//AreaCollisionContinued = 3100,
		AreaCollisionEnded     = 3200,
	}


}//End Namespace
