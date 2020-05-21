using System;
using System.Collections.Generic;
using System.Linq;
using MySudoku.Interfaces;

namespace MySudoku.Model
{

	using IntegerTriple = Tuple<int, int, int>;

	/// <summary>
	/// The sudoku game is a model of 9x9 cells. 
	/// The cells are empty or hold a digit 1-9
	/// and a set of possible digits
	/// </summary>
	public class SudokuGame : ISudokuGameModel
	{
		SudokuCell[,] grid;

		private const int InvalidSudokuDigit = -1;

		private List<IntegerTriple> History;

		#region Methods

		private SudokuCell this[int i, int j] => grid[i, j];

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
			return grid.Cast<SudokuCell>().All(c => c.IsValid());
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

		private SudokuGame Copy()
		{
			return new SudokuGame(this);
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

		#endregion

		#region Commands
		/// <summary>
		/// Clears the game
		/// </summary>
		public void Clear()
		{
			ClearGrid();
			History = new List<IntegerTriple>();
		}

		/// <summary>
		/// Populates the game with a new start situation
		/// </summary>
		public void New()
		{
			// Create a blank game

			// populate the 3x3 sub matrixs on the main diagonal

			// Fill the empty
		}

		/// <summary>
		/// Set the value in the field with the given row and column
		/// </summary>
		public void SetValue(int row, int column, int value)
		{
			grid[row, column].SetValue(value);
			History.Add(new IntegerTriple(row, column, value));
		}

		/// <summary>
		/// Goes one move back
		/// </summary>
		public void Back()
		{
			if (History.Count() == 0)
				return;

			List<Tuple<int, int, int>> Replay;
			Replay = History.Take(History.Count() - 1).ToList();
			Clear();
			Replay.ForEach(elem => { SetValue(elem.Item1, elem.Item2, elem.Item3); });
		}

		#endregion

		#region Constructors
		public SudokuGame()
		{
			// Initialize the the array of sudoku cells
			grid = new SudokuCell[9, 9];

			for (int row = 0; row<9; row++)
			{
				for(int column=0; column<9; column++)
				{
					grid[row, column] = new SudokuCell(this, row, column );
				}
			}

			// Initalize the history
			History = new List<IntegerTriple>();
		}

		private SudokuGame(SudokuGame original)
		{
			grid = new SudokuCell[9, 9];
			for (int row = 0; row < 9; row++)
			{
				for (int column = 0; column < 9; column++)
				{
					grid[row, column] = original[row, column].Copy(this);
				}
			}

			History = new List<IntegerTriple>(original.History);
		}
		#endregion
	}
}
