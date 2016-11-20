using System.Collections;
using System.Collections.Generic;
using System;


public class MazeGenerator
{
	public class Cell
	{
		public bool rowWall = true;
		public bool colWall = true;
	}


	public int rows = 10;
	public int cols = 10;

	private Random rand = new Random ();
	private List<Cell> cells = new List<Cell> ();


	[UnityEngine.ContextMenu ("GenerateMaze")]
	public void GenerateMaze ()
	{
		int curRow = 0;
		int curCol = 0;

		while (true)
		{
			
			break;
		}
	}


	private Cell NextCell ()
	{
		//	50/50
		bool isRow = rand.Next (2) == 0;
		return null;
	}

	private bool CanMakeCellAt (int row, int col)
	{
		return (row >= 0 && col >= 0 && row < rows && col < cols);
	}


	private void PrintMaze ()
	{
		for (int i = 0; i < rows; i++)
		{
			for (int k = 0; k < cols; k++)
			{
				
			}
		}
	}
}

