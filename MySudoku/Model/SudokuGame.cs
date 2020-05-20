using System;
using System.Collections.Generic;
using System.Linq;
using MySudoku.Interfaces;

namespace MySudoku.Model
{
	/// <summary>
	/// The sudoku game is a model of 9x9 cells. 
	/// The cells are empty or hold a digit 1-9
	/// and a set of possible digits
	/// </summary>
	public class SudokuGame : ISudokuGameModel
	{
		SudokuCell[,] grid;

		private const int InvalidSudokuDigit = -1;

		private List<Tuple<int,int,int>> History;

		public int GetInvalidSudokuDigit()
		{
			return InvalidSudokuDigit;
		}
		public int GetCellValue(int row, int column)
		{
			return grid[row, column].SudokuCellValue;
		}

		public List<int> GetSudokuCellPossibleValues(int row, int column)
		{
			return grid[row, column].SudokuCellPossibleValues;
		}

		/// <summary>
		/// Check, that for every empty field a least one possible values exists
		/// </summary>
		/// <returns></returns>
		public bool IsValid()
		{
			for(int row=0; row<9; row++)
			{
				for(int column=0; column<9; column++)
				{
					if (!grid[row, column].IsValid())
						return false;
				}
			}
			return true;
		}
		internal void Exclude(int row, int column, int value)
		{
			// Exclude from rows and columns
			for (int i = 0; i < 9; i++)
			{
				grid[row, i].Exclude(value);
				grid[i, column].Exclude(value);
			}

			// Exclude from square
			int rowBase = (row / 3) * 3;
			int columnBase = (column / 3) * 3;

			for (int i = rowBase; i < rowBase + 3;  i++)
			{
				for ( int j = columnBase; j < columnBase+3; j++  )
				{
					grid[i,j].Exclude(value);
				}
			}
		}

		public void SetValue(int row, int column, int value)
		{
			grid[row, column].SetValue(value);
			History.Add(new Tuple<int, int, int>(row, column, value));
		}

		private void ClearGrid()
		{
			for (int row = 0; row < 9; row++)
			{
				for (int column = 0; column < 9; column++)
				{
					grid[row, column].Clear();
				}
			}
		}

		public void Clear()
		{
			ClearGrid();
			History = new List<Tuple<int, int, int>>();
		}

		public void New()
		{

		}

		public void Back()
		{
			if (History.Count() == 0)
				return;

			List<Tuple<int, int, int>> Replay;
			Replay = History.Take(History.Count() - 1).ToList();
			Clear();
			Replay.ForEach(elem => { SetValue(elem.Item1, elem.Item2, elem.Item3); });
		}

		public SudokuGame()
		{
			// Initialize the the array of sudoku cells
			grid = new SudokuCell[9, 9];

			for(int row = 0; row<9; row++)
			{
				for(int column=0; column<9; column++)
				{
					grid[row, column] = new SudokuCell(this, row, column );
				}
			}

			// Initalize the history
			History = new List<Tuple<int, int, int>>();
		}
	}
}
