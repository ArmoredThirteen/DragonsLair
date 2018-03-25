using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif


namespace Ate.Enemies
{
	
	
	/// <summary>
	/// Controls movement of enemy ships.
	/// </summary>
	public class EnemyShipMovement : AteComponent_fpsControlled
	{

		protected enum MoveDirection
		{
			PositiveX,
			NegativeX,

			PositiveY,
			NegativeY,

			PositiveZ,
			NegativeZ,
		}

		#region Public Variables

		public float moveDistance = 0.5f;
		public int   maxMoveSteps = 1;

		public bool  useRandomSeed = true;
		public int   seed          = 0;

		#endregion


		#region Private Variables

		private Vector3 _startLocation = new Vector3 ();


		private System.Random _random = new System.Random ();


		private int _curSteps_x = 0;
		private int _curSteps_y = 0;
		private int _curSteps_z = 0;

		private int _prevSteps_x = 0;
		private int _prevSteps_y = 0;
		private int _prevSteps_z = 0;


		/// <summary>
		/// Used while checking which directions you can move in.
		/// Since it is built/rebuilt often, it is declared here to
		/// reduce creation/destruction of lists.
		/// Should only ever be used while finding a new location
		/// and is not for reading or writing outside of that.
		/// </summary>
		private List<MoveDirection> _possibleMoves = new List<MoveDirection> ();

		#endregion


		#if UNITY_EDITOR

		public override void DrawInspector ()
		{
			base.DrawInspector ();

			moveDistance = EditorGUILayout.FloatField ("Move Distance",  moveDistance);
			maxMoveSteps = EditorGUILayout.IntField   ("Max Move Steps", maxMoveSteps);

			useRandomSeed = EditorGUILayout.Toggle ("Use Random Seed", useRandomSeed);
			if (!useRandomSeed)
				seed = EditorGUILayout.IntField ("Seed", seed);
		}

		#endif


		#region AteComponent

		protected override void AteAwake ()
		{
			base.AteAwake ();

			BuildRandomFromSeed ();
		}

		protected override void AteStart ()
		{
			base.AteStart ();

			_startLocation = Position;
		}


		// Updates every game frame
		protected override void AteUpdate ()
		{
			
		}

		// Updates 24 times per second
		protected override void FpsUpdate24 ()
		{

		}

		// Updates once per framelength, which is one or more FpsUpdate## calls
		protected override void UpdateFrameLength ()
		{
			MoveDirection newDirection = FindNewMoveDirection ();
			Vector3 newLocation = FindNewLocation (newDirection);
			MoveToLocation (newLocation);
		}

		#endregion


		#region Public Methods

		/// <summary>
		/// Creates a new random system using the given seed.
		/// Sorta hacky way for enemy spawners to ensure the
		/// random system is using updated seeds when respawning
		/// enemies from out of the pool.
		/// </summary>
		public void BuildRandomFromSeed ()
		{
			_random = new System.Random (seed);
		}

		#endregion


		#region Private Methods

		private MoveDirection FindNewMoveDirection ()
		{
			_possibleMoves.Clear ();

			int minMoveSteps = maxMoveSteps * -1;

			if (_curSteps_x < maxMoveSteps && (_curSteps_x+1 != _prevSteps_x))
				_possibleMoves.Add (MoveDirection.PositiveX);
			if (_curSteps_x > minMoveSteps && (_curSteps_x-1 != _prevSteps_x))
				_possibleMoves.Add (MoveDirection.NegativeX);

			if (_curSteps_y < maxMoveSteps && (_curSteps_y+1 != _prevSteps_y))
				_possibleMoves.Add (MoveDirection.PositiveY);
			if (_curSteps_y > minMoveSteps && (_curSteps_y-1 != _prevSteps_y))
				_possibleMoves.Add (MoveDirection.NegativeY);

			//TODO: Set it up to have 'up axis' settings to selectively ignore x, y, or z.
			/*if (_curSteps_z < maxMoveSteps && (_curSteps_z+1 != _prevSteps_z))
				_possibleMoves.Add (MoveDirection.PositiveZ);
			if (_curSteps_z > minMoveSteps && (_curSteps_z-1 != _prevSteps_z))
				_possibleMoves.Add (MoveDirection.NegativeZ);*/

			int randomDirection = -1;
			if (useRandomSeed)
				randomDirection = Random.Range (0, _possibleMoves.Count);
			else
			{
				randomDirection = _random.Next (0, _possibleMoves.Count);
			}
			
			return _possibleMoves[randomDirection];
		}

		private Vector3 FindNewLocation (MoveDirection newDirection)
		{
			int newStepsX = _curSteps_x;
			int newStepsY = _curSteps_y;
			int newStepsZ = _curSteps_z;

			switch (newDirection)
			{
				case MoveDirection.PositiveX:
					newStepsX = newStepsX + 1;
					break;
				
				case MoveDirection.NegativeX:
					newStepsX = newStepsX - 1;
					break;
				
				case MoveDirection.PositiveY:
					newStepsY = newStepsY + 1;
					break;
				
				case MoveDirection.NegativeY:
					newStepsY = newStepsY - 1;
					break;
				
				case MoveDirection.PositiveZ:
					newStepsZ = newStepsZ + 1;
					break;
				
				case MoveDirection.NegativeZ:
					newStepsZ = newStepsZ - 1;
					break;
			}

			_prevSteps_x = _curSteps_x;
			_prevSteps_y = _curSteps_y;
			_prevSteps_z = _curSteps_z;

			_curSteps_x = newStepsX;
			_curSteps_y = newStepsY;
			_curSteps_z = newStepsZ;

			float newX = _startLocation.x + (newStepsX * moveDistance);
			float newY = _startLocation.y + (newStepsY * moveDistance);
			float newZ = _startLocation.z + (newStepsZ * moveDistance);

			return new Vector3 (newX, newY, newZ);
		}

		private void MoveToLocation (Vector3 newLoc)
		{
			Position = newLoc;
		}

		#endregion

	}//End Class
	
	
}//End Namespace
