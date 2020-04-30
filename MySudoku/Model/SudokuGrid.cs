using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySudoku
{
	public class SudokuGrid
	{
		SudokuCell[,] grid;

		public SudokuCell GetSudokuCell(int row, int column)
		{
			return grid[row, column];
		}

		public void Exclude(int row, int column, int value)
		{
			for (int i = 0; i < 9; i++)
			{
				grid[row, i].Exclude(value);
				grid[i, column].Exclude(value);
			}
		}
		public SudokuGrid()
		{
			grid = new SudokuCell[9, 9];
			for(int row = 0; row<9; row++)
			{
				for(int column=0; column<9; column++)
				{
					grid[row, column] = new SudokuCell(this, row, column );
				}
			}
		}
	}
}
