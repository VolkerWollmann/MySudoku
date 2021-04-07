using System.Windows.Controls;
using System.Windows.Input;
using MySudoku.Controls;
using MySudoku.Interfaces;
using System;
using System.ComponentModel;
using System.Windows;
using MySudoku.Constants;

namespace MySudoku.ViewModel
{
	using DataTriple = Tuple<int, int, bool>;
	/// <summary>
	/// Maps the game model to the view
	/// </summary>
	public class SudokuViewModel
	{
		public enum MoveDirection
		{
			None,
			Up,
			Down,
			Left,
			Right
		};

		// Grid from program
		private Grid SudokuGrid;

		// Model : game
		private readonly ISudokuGameModel SudokuGame;

		// View : board to display game
		private readonly ISudokuBoardView SudokuBoardView;

		// View : Command buttons 
		private readonly ISudokuCommandView SudokuCommands;

		// View model data: Formatting of single cells from the model ot the view model
		private readonly CellViewModel[,] GameCellToViewCell = new CellViewModel[9, 9];

		// View model data: Number of cells to fill
		private int NumberOfCellsToFill;

		private void UpdateValues()
		{
			int row, column;
			for (row = 0; row < 9; row++)
			{
				for (column = 0; column < 9; column++)
				{
					GameCellToViewCell[row, column].SetValue(SudokuGame.GetCellValue(row, column));
					GameCellToViewCell[row, column].SetPossibleValuesSet(SudokuGame.GetCellPossibleValues(row, column));

					SudokuBoardView.SetValue(row, column, GameCellToViewCell[row, column].Value);
					SudokuBoardView.SetPossibleValueSetString(row, column, GameCellToViewCell[row, column].PossibleValuesSetString);
					SudokuBoardView.SetPossibleValueContextMenu(row, column, GameCellToViewCell[row, column].PossibleValuesSet);
				}
			}

			SudokuBoardView.GetCurrentCellCoordinates(out row, out column);
			SudokuBoardView.MarkCell(row, column);
		}

		#region Constructor
		public SudokuViewModel(Grid sudokuGrid, ISudokuGameModel sudokuGame)
		{
			// grid form the program
			SudokuGrid = sudokuGrid;

			// prepare model
			SudokuGame = sudokuGame;

			// prepare game grid (view)
			SudokuBoardView = new SudokuBoardUserControl();

			// add game grid to program
			sudokuGrid.Children.Add(SudokuBoardView.GetUIElement());
			Grid.SetRow(SudokuBoardView.GetUIElement(), 0);
			Grid.SetColumn(SudokuBoardView.GetUIElement(), 0);

			// prepare the binding
			for (int row = 0; row < 9; row++)
			{
				for (int column = 0; column < 9; column++)
				{
					GameCellToViewCell[row, column] = new CellViewModel();
				}
			}

			// prepare Key operation
			SudokuBoardView.SetKeyEventHandler(KeyUp);

			// create the game button user control (view)
			SudokuCommands = new SudokuCommandUserControl();

			// add command panel to program
			sudokuGrid.Children.Add(SudokuCommands.GetUIElement());
			Grid.SetRow(SudokuCommands.GetUIElement(), 0);
			Grid.SetColumn(SudokuCommands.GetUIElement(), 1);

			// bind command to buttons
			SudokuCommands.SetCommandEventHandler(SudokuCommand.Clear, ClearCommand);
			SudokuCommands.SetCommandEventHandler(SudokuCommand.Back ,BackCommand);
			SudokuCommands.SetCommandEventHandler(SudokuCommand.New, NewCommand);
			SudokuCommands.SetCommandEventHandler(SudokuCommand.Solve, SolveCommand);
			SudokuCommands.SetCommandEventHandler(SudokuCommand.TogglePossibleValues, TogglePossibleValuesVisibilityCommand);

			SudokuBoardView.MarkCell(0, 0);
			UpdateValues();


		}

		#endregion

		#region Input

		private void Move(MoveDirection moveDirection)
		{
            SudokuBoardView.GetCurrentCellCoordinates(out var row, out var column);

			bool moved = false;
			if ((row >= 0) && (column >= 0))
			{
				if (moveDirection == MoveDirection.Up)
				{
					if (row > 0)
					{
						moved = true;
						row--;
					}
				}
				else if (moveDirection == MoveDirection.Down)
				{
					if (row < 8)
					{
						moved = true;
						row++;
					}
				}
				else if (moveDirection == MoveDirection.Left)
				{
					if (column > 0)
					{
						moved = true;
						column--;
					}
				}
				else if (moveDirection == MoveDirection.Right)
				{
					if (column < 8)
					{
						moved = true;
						column++;
					}
				}

				if (moved)
				{
					SudokuBoardView.MarkCell(row, column);
				}
			}
		}

		private MoveDirection MoveDirectionFromKey(Key key)
		{
			switch (key)
			{
				case Key.Up:
				case Key.W:
					return MoveDirection.Up;

				case Key.Down:
				case Key.S:
					return MoveDirection.Down;

				case Key.Left:
				case Key.A:
					return MoveDirection.Left;

				case Key.Right:
				case Key.D:
					return MoveDirection.Right;

				default:
					return MoveDirection.None;
			}
		}

		/// <summary>
		/// Transforms the key for 1-9, otherwise -1
		/// </summary>
		/// <param name="key">pressed key</param>
		/// <returns>return -1, 1-9</returns>
		private int SudokuDigitFromKey(Key key)
		{
			if (key >= Key.D1 && key <= Key.D9)
			{
				return (key - Key.D0);
			}

			if (key >= Key.NumPad1 && key <= Key.NumPad9)
			{
				return (key - Key.NumPad0);
			}

			return SudokuGame.GetInvalidDigit();
		}

		public void Set(Key key)
		{
			// Key space : Toggle PossibleValuesSetVisibility
			if ( key == Key.Space)
            {
				BackGroundTogglePossibleValuesVisibilityDoWork(null, null);
				return;
            }

			// Key to sudoku digit
			int sudokuDigit = SudokuDigitFromKey(key);
			if (sudokuDigit != SudokuGame.GetInvalidDigit())
			{
                SudokuBoardView.GetCurrentCellCoordinates(out var row, out var column);

				if ((row >= 0) && (column >= 0))
				{
					SudokuGame.SetValue(row, column, sudokuDigit);
					UpdateValues();
				}

				return;
			}
			MoveDirection moveDirection = MoveDirectionFromKey(key);
			if (moveDirection != MoveDirection.None)
			{
				Move(moveDirection);
			}
		}

		private void KeyUp(object sender, Key key)
		{
			Set(key);
		}

		#endregion

		#region commands

		private void ProcessBackGroundCommand(DoWorkEventHandler doWorkEventHandler, RunWorkerCompletedEventHandler workerCompletedEventHandler)
		{
			SudokuCommands.SetButtonsEnabled(false);
			NumberOfCellsToFill = SudokuCommands.GetNumberOfCellsToFill();

            BackgroundWorker backgroundWorker = new BackgroundWorker {WorkerReportsProgress = false};
            backgroundWorker.DoWork += doWorkEventHandler;
			backgroundWorker.RunWorkerCompleted += workerCompletedEventHandler;
			backgroundWorker.RunWorkerAsync(this);
		}

		#region Clear
		void BackGroundClearDoWork(object sender, DoWorkEventArgs e)
		{
			SudokuGame.Clear();
		}
		void BackGroundClearCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			UpdateValues();
			SudokuCommands.SetButtonsEnabled(true);
		}

		private void ClearCommand(object sender, EventArgs e)
		{
			ProcessBackGroundCommand(BackGroundClearDoWork, BackGroundClearCompleted);
		}
		#endregion

		#region New
		void BackGroundNewDoWork(object sender, DoWorkEventArgs e)
		{
			SudokuGame.New(NumberOfCellsToFill);
		}
		void BackGroundNewCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			UpdateValues();
			SudokuCommands.SetButtonsEnabled(true);
		}

		private void NewCommand(object sender, EventArgs e)
		{
			ProcessBackGroundCommand(BackGroundNewDoWork, BackGroundNewCompleted);
			SudokuBoardView.MarkCell(4, 4);
		}
		#endregion

		#region Back

		void BackGroundBackDoWork(object sender, DoWorkEventArgs e)
		{
            var valid = SudokuGame.GetLastOperation(out var row, out var column, out _);
			SudokuGame.Back();
			e.Result = new DataTriple(row, column, valid) ;
		}
		void BackGroundBackCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			DataTriple data = (DataTriple)e.Result;

			if (data.Item3)
				SudokuBoardView.MarkCell(data.Item1, data.Item2);

			UpdateValues();
			SudokuCommands.SetButtonsEnabled(true);
		}

		private void BackCommand(object sender, EventArgs e)
		{
			ProcessBackGroundCommand(BackGroundBackDoWork, BackGroundBackCompleted);
		}

		#endregion

		#region Solve
		private void BackGroundSolveWork(object sender, DoWorkEventArgs e)
		{
			e.Result = SudokuGame.Solve();
		}

		void BackGroundSolveCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			UpdateValues();
			SudokuCommands.SetButtonsEnabled(true);
			if (!(bool)e.Result)
			{
				MessageBox.Show("Cannot solve.");
			}
		}


		private void SolveCommand(object sender, EventArgs e)
		{
			ProcessBackGroundCommand(BackGroundSolveWork, BackGroundSolveCompleted);
		}
		#endregion

		#region TogglePossibleValues
		void BackGroundTogglePossibleValuesVisibilityDoWork(object sender, DoWorkEventArgs e)
		{
			SudokuBoardView.PossibleValueSetVisibility = !SudokuBoardView.PossibleValueSetVisibility;
		}

		void BackGroundTogglePossibleValuesVisibilityCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			SudokuCommands.SetButtonsEnabled(true);
		}

		private void TogglePossibleValuesVisibilityCommand(object sender, EventArgs e)
		{
			ProcessBackGroundCommand(BackGroundTogglePossibleValuesVisibilityDoWork, BackGroundTogglePossibleValuesVisibilityCompleted);
		}
		#endregion

		#endregion
	}
}
