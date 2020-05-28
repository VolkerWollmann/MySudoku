using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using MySudoku.Controls;
using MySudoku.Interfaces;
using System;
using System.Threading;

namespace MySudoku.ViewModel
{
	public class ViewModelGameGrid
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
		private Grid sudokuGrid;

		// The game grid from the model
		private ISudokuGameModel sudokuGame;

		// The view control
		ISudokuGridView sudokuGridView;

		// The binding information
		ViewModelCellData[,] sudokuDatas = new ViewModelCellData[9, 9];

		// Command Control
		ISudokuCommands sudokuCommand;

		private void UpdateValues()
		{
			for (int row = 0; row < 9; row++)
			{
				for (int column = 0; column < 9; column++)
				{
					sudokuDatas[row, column].SetValue(sudokuGame.GetCellValue(row, column));
					sudokuDatas[row, column].SetPossibleValueSet(sudokuGame.GetCellPossibleValues(row, column));

					sudokuGridView.SetValue(row, column, sudokuDatas[row, column].Value);
					sudokuGridView.SetPossibleValueSet(row, column, sudokuDatas[row, column].PossibleValueSet);
				}
			}
		}

		public ViewModelGameGrid(Grid _sudokuGrid, ISudokuGameModel _sudokuGame)
		{
			// grid form the program
			sudokuGrid = _sudokuGrid;

			// prepare model
			sudokuGame = _sudokuGame;

			// prepare game grid (view)
			sudokuGridView = (ISudokuGridView)new SudokuGridUserControl();

			// add game grid to progam
			sudokuGrid.Children.Add(sudokuGridView.GetUIElement());
			Grid.SetRow(sudokuGridView.GetUIElement(), 0);
			Grid.SetColumn(sudokuGridView.GetUIElement(), 0);

			// prepare the binding
			for (int row = 0; row < 9; row++)
			{
				for (int column = 0; column < 9; column++)
				{
					sudokuDatas[row, column] = new ViewModelCellData();
				}
			}

			// prepare Key operation
			sudokuGridView.SetKeyEventHandler(KeyUp);

			// create the game button user control (view)
			sudokuCommand = new SudokuCommandUserControl();

			// add command panel to program
			sudokuGrid.Children.Add(sudokuCommand.GetUIElement());
			Grid.SetRow(sudokuCommand.GetUIElement(), 0);
			Grid.SetColumn(sudokuCommand.GetUIElement(), 1);

			// bind command to buttons
			sudokuCommand.SetClearCommandEventHandler(ClearCommand);
			sudokuCommand.SetBackCommandEventHandler(BackCommand);
			sudokuCommand.SetNewCommandEventHandler(NewCommand);

			UpdateValues();

		}

		#region Input

		private void Move(MoveDirection moveDirection)
		{
			int row, column;
			sudokuGridView.GetCurrentCellCoordiantes(out row, out column);

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
					sudokuGridView.MarkCell(row, column);
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

			return sudokuGame.GetInvalidDigit();
		}

		public void Set(Key key)
		{
			// Key to sukdou digit
			int sudokuDigit = SudokuDigitFromKey(key);
			if (sudokuDigit != sudokuGame.GetInvalidDigit())
			{
				int row, column;
				sudokuGridView.GetCurrentCellCoordiantes(out row, out column);

				if ((row >= 0) && (column >= 0))
				{
					sudokuGame.SetValue(row, column, sudokuDigit);
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
			sudokuGame.Clear();
			UpdateValues();
		}


		private static void BackGroundNew(object data)
		{
			ViewModelGameGrid gameGridViewModel = (ViewModelGameGrid)data;
			gameGridViewModel.sudokuGame.New();
			gameGridViewModel.UpdateValues();
			gameGridViewModel.sudokuCommand.SetButtonsEnabled(true) ;
		}

		private void NewCommand(object sender, EventArgs e)
		{
			sudokuCommand.SetButtonsEnabled(false);

			ParameterizedThreadStart ps = new ParameterizedThreadStart(BackGroundNew);
			Thread thread = new Thread(ps);
			thread.Start(this);
		}

		private void BackCommand(object sender, EventArgs e)
		{
			sudokuGame.Back();
			UpdateValues();
		}

		#endregion
	}
}
