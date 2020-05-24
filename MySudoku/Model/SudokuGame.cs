﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using MySudoku.Interfaces;
using MySudoku.Model.BruteForce;
using MySudoku.Perfomance;

namespace MySudoku.Model
{
	using IntegerTriple = Tuple<int, int, int>;

	/// <summary>
	/// The sudoku game is a model of 9x9 cells. 
	/// The cells are empty or hold a digit 1-9
	/// and a set of possible digits
	/// </summary>
	public class SudokuGame : ISudokuGameModel, ISudokuGenerator 
	{
		private string Name { get; set; }

		SudokuCell[,] grid;

		private const int InvalidSudokuDigit = -1;

		private List<IntegerTriple> History;

		#region Methods

		private SudokuCell this[int i, int j] => grid[i, j];


		private List<SudokuCell> cellList = null;
		private List<SudokuCell> GetCellList()
		{
			if (cellList == null)
				cellList = grid.Cast<SudokuCell>().ToList();

			return cellList;
		}

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

		public bool IsEqual(SudokuGame other)
		{
			for (int row = 0; row < 9; row++)
			{
				for (int column = 0; column < 9; column++)
				{
					if (!grid[row, column].IsEqual(other[row, column]))
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

		private SudokuGame Copy()
		{
			return new SudokuGame(this, this.Name +"_X");
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

		#region Generation

		private void PopulateSubmatrix(int startRow, int EndRow, int startColumn, int EndColumn)
		{
			List<int> shuffledValues = RandomListAccess.GetShuffledList(SudokuCell.GetInitalPossibleValueList());
			int i = 0;
			for( int row=startRow; row <= EndRow; row++)
			{
				for(int column=startColumn; column<=EndColumn; column++)
				{
					this[row, column].SetValue(shuffledValues[i++]);
				}
			}
		}

		private SudokuGame Search(SudokuGame sudokuGame, int level)
		{
			// check vor valid game
			if (!sudokuGame.IsValid())
				return null;

			List<SudokuCell> sudokuCells = sudokuGame.GetCellList();

			// First evaluate single possible values
			SudokuCell spvc = sudokuCells.FirstOrDefault(cell => (cell.SudokuCellValue == 0 && cell.SudokuCellPossibleValues.Count == 1));
			if (spvc != null)
			{
				while (spvc != null)
				{
					bool canSet = sudokuGame.SetValue(spvc.Row, spvc.Column, spvc.SudokuCellPossibleValues.First());
					if (!canSet)
						return null;

					spvc = sudokuCells.FirstOrDefault(cell => (cell.SudokuCellValue == 0 && cell.SudokuCellPossibleValues.Count == 1));
				}

				if (!sudokuGame.IsValid())
					return null;
			}

			// Get a shuffled list all fields whith more than one value must be filled
			List<SudokuCell> cellsToFill = sudokuCells.Where(cell => (cell.SudokuCellValue == 0)).ToList();
			cellsToFill = RandomListAccess.GetShuffledList(cellsToFill);

			// nothing left to fill : it is a valid solution
			if (cellsToFill.Count == 0)
				return sudokuGame;
		
			int i = 0;
			foreach ( SudokuCell cell in cellsToFill)
			{
				performanceCounter.Update(cellsToFill.Count, ++i);
				foreach( int possibleValue in cell.SudokuCellPossibleValues )
				{					
					//make copy of the game
					SudokuGame tryGame = sudokuGame.Copy();

					// try the value
					if (tryGame.SetValue(cell.Row, cell.Column, possibleValue))
					{
						// now try one recursion deeper, with one field more set
						performanceCounter.Down();
						tryGame = Search(tryGame, level+1);
						performanceCounter.Up();
						if (tryGame != null)
						{
							// the value contributes to a valid solution
							return tryGame;
						}
					}
				}
			}

			// no cell and no field generates a valid solution
			return null;
		}

		PerformanceCounter performanceCounter;
		private SudokuGame Search(SudokuGame sudokuGame )
		{
			performanceCounter = new PerformanceCounter();
			return Search(sudokuGame, 1);
		}

		private SudokuGame sudokuGameGenerator;
		bool Generate()
		{
			sudokuGameGenerator = new SudokuGame("L1");

			// populate the 3x3 sub matrixs on the main diagonal
			// that can be done without checks
			sudokuGameGenerator.PopulateSubmatrix(0, 2, 0, 2);
			sudokuGameGenerator.PopulateSubmatrix(3, 5, 3, 5);
			sudokuGameGenerator.PopulateSubmatrix(6, 8, 6, 8);

			// Fill the empty
			sudokuGameGenerator = Search(sudokuGameGenerator);

			return (sudokuGameGenerator != null);
		}

		List<IntegerTriple> GetSolution()
		{
			List<IntegerTriple> list = new List<IntegerTriple>();
			sudokuGameGenerator.GetCellList().ForEach(cell => list.Add(new IntegerTriple(cell.Row, cell.Column, cell.SudokuCellValue)));
			return list;
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
			Clear();

			// Generate solution
			ISudokuGenerator sudokuBruteForceGenerator = new SudokuBruteForceGenerator();
			bool result = sudokuBruteForceGenerator.Generate();
			if (result)
			{
				List<IntegerTriple> list = RandomListAccess.GetShuffledList<IntegerTriple>(sudokuBruteForceGenerator.GetSolution()).Take(54).ToList();
				list.ForEach(cell => { this.SetValue(cell.Item1, cell.Item2, cell.Item3); });
			}
			
		}

		/// <summary>
		/// Set the value in the field with the given row and column
		/// </summary>
		public bool SetValue(int row, int column, int value)
		{
			if (grid[row, column].SetValue(value))
			{
				History.Add(new IntegerTriple(row, column, value));
				return true;
			}

			return false;
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
		public SudokuGame(string name)
		{
			Name = name;

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

		private SudokuGame(SudokuGame original, string name)
		{
			Name = name;
			grid = new SudokuCell[9, 9];
			original.GetCellList().ForEach(cell => { grid[cell.Row, cell.Column] = cell.Copy(this);  });
			History = new List<IntegerTriple>(original.History);
		}
		#endregion
	}
}
