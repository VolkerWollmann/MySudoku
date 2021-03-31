using System;
using System.Collections.Generic;
using System.Linq;
using MySudoku.Interfaces;
using MySudoku.Model.Interfaces;
using MySudoku.Model.Support;

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
		private string Name { get; }

        readonly SudokuCell[,] grid;

		private const int InvalidDigit = -1;

		internal List<IntegerTriple> History;

		#region Methods

		private SudokuCell this[int i, int j] => grid[i, j];


		private List<SudokuCell> cellList;
		private List<SudokuCell> GetCellList()
		{
			if (cellList == null)
				cellList = grid.Cast<SudokuCell>().ToList();

			return cellList;
		}

		public int GetInvalidDigit()
		{
			return InvalidDigit;
		}
		public int GetCellValue(int row, int column)
		{
			return grid[row, column].CellValue;
		}

		public List<int> GetCellPossibleValues(int row, int column)
		{
			return grid[row, column].CellPossibleValues;
		}

		/// <summary>
		/// Check, that for every empty field a least one possible values exists
		/// </summary>
		/// <returns></returns>
		public bool IsValid()
		{
			return GetCellList().All(c => c.IsValid());
		}

		public void GetFirstInvalid()
        {
            var firstInValidCell = GetCellList().First(c => !c.IsValid());
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
			return new SudokuGame(this, Name +"_X");
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

		private void PopulateSubmatrix(int startRow, int endRow, int startColumn, int endColumn)
		{
			List<int> shuffledValues = RandomListAccess.GetShuffledList(SudokuCell.GetInitialPossibleValueList());
			int i = 0;
			for( int row=startRow; row <= endRow; row++)
			{
				for(int column=startColumn; column<=endColumn; column++)
				{
					this[row, column].SetValue(shuffledValues[i++]);
				}
			}
		}

		private SudokuGame Search(SudokuGame sudokuGame, int level)
		{
            // check vor valid game
			if (!sudokuGame.IsValid())
			{
				sudokuGame.GetFirstInvalid();
				return null;
			}

			List<SudokuCell> cellsToFill = sudokuGame.GetCellList().Where(ce => (ce.CellValue == 0)).ToList();

			// nothing left to fill : it is a valid solution
			if (cellsToFill.Count == 0)
				return sudokuGame;

			int c = 0;
			SudokuCell cell = cellsToFill.First();

			foreach (int possibleValue in cell.CellPossibleValues)
			{
				performanceCounter.Update(cell.CellPossibleValues.Count, ++c);
				//make copy of the game
				SudokuGame tryGame = sudokuGame.Copy();

				// try the value
				if (tryGame.SetValue(cell.Row, cell.Column, possibleValue))
				{ 
                    // now try one recursion deeper, with one field more set
					performanceCounter.Down();
					tryGame = Search(tryGame, level + 1);
					performanceCounter.Up();
					if (tryGame != null)
					{
						// the value contributes to a valid solution
						return tryGame;
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

		public List<IntegerTriple> Generate()
		{
			SudokuGame sudokuGameGenerator = new SudokuGame("L1");

			// populate the 3x3 sub matrixs on the main diagonal
			// that can be done without checks
			sudokuGameGenerator.PopulateSubmatrix(0, 2, 0, 2);
			sudokuGameGenerator.PopulateSubmatrix(3, 5, 3, 5);
			sudokuGameGenerator.PopulateSubmatrix(6, 8, 6, 8);

			// Fill the empty
			sudokuGameGenerator = Search(sudokuGameGenerator);

			if (sudokuGameGenerator == null)
				return null;

			List<IntegerTriple> list = new List<IntegerTriple>();
			sudokuGameGenerator.GetCellList().ForEach(cell => list.Add(new IntegerTriple(cell.Row, cell.Column, cell.CellValue)));
			return list;
		}

		#endregion

		#region Commands

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
		public void New( int numberOfCellsToFill)
		{
			// Create a blank game
			Clear();

			// Generate solution
			//ISudokuGenerator iSudokuGenerator = new SudokuBruteForceGenerator();
			ISudokuGenerator iSudokuGenerator = this;
			List<IntegerTriple> result = iSudokuGenerator.Generate();
			if (result != null)
			{
				RandomListAccess.GetShuffledList(result).Take(numberOfCellsToFill).ToList().ForEach(cell => { SetValue(cell.Item1, cell.Item2, cell.Item3); });
			}			
		}
		/// <summary>
		/// Goes one move back
		/// </summary>
		public void Back()
		{
			if (!History.Any())
				return;

            var Replay = History.Take(History.Count - 1).ToList();
			Clear();
			Replay.ForEach(elem => { SetValue(elem.Item1, elem.Item2, elem.Item3); });
		}

		public bool GetLastOperation(out int x, out int y, out int value)
		{
			if (History.Count == 0)
			{
				x = 0;
				y = 0;
				value = 0;
				return false;
			}	

			var e = History.Last();

			x = e.Item1;
			y = e.Item2;
			value = e.Item3;
			return true;
		}


		public bool Solve()
		{

			SudokuGame solver = Copy();
			solver = Search(solver);

			if (solver == null)
				return false;

			var sublist = solver.History.GetRange(History.Count, (solver.History.Count - History.Count));
			sublist.ForEach(cell => { SetValue(cell.Item1, cell.Item2, cell.Item3); });

			return true;			
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

			// Initialize the history
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
