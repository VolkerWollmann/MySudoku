using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using MySudoku.Controls;
using MySudoku.Interfaces;
using System;
using System.Threading;

namespace MySudoku.ViewModel
{
	/// <summary>
	/// Maps the game model to the view model
	/// </summary>
	public class GameModelToViewModel
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

		// The game model
		private ISudokuGameModel SudokuGame;
		private int NumberOfCellsToFill;

		// The view model
		private ISudokuViewModel SudokuGridView;

		// Formatting of the game data for single cells of the view model
		private GameCellToViewCell[,] GameCellToViewCell = new GameCellToViewCell[9, 9];

		// Command Control
		private ISudokuCommands SudokuCommands;

		private void UpdateValues()
		{
			for (int row = 0; row < 9; row++)
			{
				for (int column = 0; column < 9; column++)
				{
					GameCellToViewCell[row, column].SetValue(SudokuGame.GetCellValue(row, column));
					GameCellToViewCell[row, column].SetPossibleValueSet(SudokuGame.GetCellPossibleValues(row, column));

					SudokuGridView.SetValue(row, column, GameCellToViewCell[row, column].Value);
					SudokuGridView.SetPossibleValueSet(row, column, GameCellToViewCell[row, column].PossibleValueSet);
				}
			}
		}

		public GameModelToViewModel(Grid sudokuGrid, ISudokuGameModel sudokuGame)
		{
			// grid form the program
			SudokuGrid = sudokuGrid;

			// prepare model
			SudokuGame = sudokuGame;

			// prepare game grid (view)
			SudokuGridView = (ISudokuViewModel)new SudokuGridUserControl();

			// add game grid to progam
			sudokuGrid.Children.Add(SudokuGridView.GetUIElement());
			Grid.SetRow(SudokuGridView.GetUIElement(), 0);
			Grid.SetColumn(SudokuGridView.GetUIElement(), 0);

			// prepare the binding
			for (int row = 0; row < 9; row++)
			{
				for (int column = 0; column < 9; column++)
				{
					GameCellToViewCell[row, column] = new GameCellToViewCell();
				}
			}

			// prepare Key operation
			SudokuGridView.SetKeyEventHandler(KeyUp);

			// create the game button user control (view)
			SudokuCommands = new SudokuCommandUserControl();

			// add command panel to program
			sudokuGrid.Children.Add(SudokuCommands.GetUIElement());
			Grid.SetRow(SudokuCommands.GetUIElement(), 0);
			Grid.SetColumn(SudokuCommands.GetUIElement(), 1);

			// bind command to buttons
			SudokuCommands.SetClearCommandEventHandler(ClearCommand);
			SudokuCommands.SetBackCommandEventHandler(BackCommand);
			SudokuCommands.SetNewCommandEventHandler(NewCommand);

			UpdateValues();

		}

		#region Input

		private void Move(MoveDirection moveDirection)
		{
			int row, column;
			SudokuGridView.GetCurrentCellCoordiantes(out row, out column);

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
					SudokuGridView.MarkCell(row, column);
				}
			}
		}

		private MoveDirection MoveDirectionFromKey(Key key)
		{
			switch (key)
			{
				case Key.Up:
					return MoveDirection.Up;

				case Key.Down:
					return MoveDirection.Down;

				case Key.Left:
					return MoveDirection.Left;

				case Key.Right:
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
			// Key to sukdou digit
			int sudokuDigit = SudokuDigitFromKey(key);
			if (sudokuDigit != SudokuGame.GetInvalidDigit())
			{
				int row, column;
				SudokuGridView.GetCurrentCellCoordiantes(out row, out column);

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
		#endregion

		private void KeyUp(object sender, Key key)
		{
			Set(key);
		}

		#region commands
		private void ClearCommand(object sender, EventArgs e)
		{
			SudokuGame.Clear();
			UpdateValues();
		}


		private static void BackGroundNew(object data)
		{
			GameModelToViewModel gameGridViewModel = (GameModelToViewModel)data;
			gameGridViewModel.SudokuGame.New(gameGridViewModel.NumberOfCellsToFill);
			gameGridViewModel.UpdateValues();
			gameGridViewModel.SudokuCommands.SetButtonsEnabled(true) ;
		}

		private void NewCommand(object sender, EventArgs e)
		{
			SudokuCommands.SetButtonsEnabled(false);
			NumberOfCellsToFill = SudokuCommands.GetNumberOfCellsToFill();
			ParameterizedThreadStart ps = new ParameterizedThreadStart(BackGroundNew);
			Thread thread = new Thread(ps);
			thread.Start(this);
		}

		private void BackCommand(object sender, EventArgs e)
		{
			SudokuGame.Back();
			UpdateValues();
		}

		#endregion
	}
}
