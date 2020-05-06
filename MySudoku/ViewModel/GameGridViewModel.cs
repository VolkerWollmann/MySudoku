using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using MySudoku.Controls;
using MySudoku.Model;
using MySudoku.Interfaces;
using System.Windows;
using System;

namespace MySudoku.ViewModel
{
	public class GameGridViewModel
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
					sudokuDatas[row, column].SetValue(sudokuGame.GetCellValue(row,column));
					sudokuDatas[row, column].SetPossibleValues(sudokuGame.GetSudokuCellPossibleValues(row,column));
				}
			}
		}

  		public GameGridViewModel(Grid _sudokuGrid, ISudokuGameModel _sudokuGame)
		{ 
			sudokuGrid = _sudokuGrid;

			// prepare model
			sudokuGame = _sudokuGame;

			// prepare view
			sudokuGridView = (ISudokuGridView) new SudokuGridUserControl();
			
			sudokuGrid.Children.Add(sudokuGridView.GetUIElement());
			Grid.SetRow(sudokuGridView.GetUIElement(), 0);
			Grid.SetColumn(sudokuGridView.GetUIElement(), 0);

		    // prepare the binding
			for (int row = 0; row < 9; row++)
			{
				for (int column = 0; column < 9; column++)
				{
					sudokuDatas[row, column] = new ViewModelCellData();

					Binding valueBinding = new Binding("Value");
					valueBinding.Source = sudokuDatas[row, column];
					sudokuGridView.BindValue(row, column, valueBinding);

					Binding possibleValuesBinding = new Binding("PossibleValues");
					possibleValuesBinding.Source = sudokuDatas[row, column];
					sudokuGridView.BindPossibleValues(row, column, possibleValuesBinding);
				}
			}

			// prepare Key operation
			sudokuGridView.SetKeyEventHandler(KeyUp);

			sudokuCommand = new SudokuCommandUserControl();

			sudokuGrid.Children.Add(sudokuCommand.GetUIElement());
			Grid.SetRow(sudokuCommand.GetUIElement(), 0);
			Grid.SetColumn(sudokuCommand.GetUIElement(), 1);

			sudokuCommand.SetClearCommandEventHandler(ClearCommand);

			UpdateValues();

		}

		public void Clear()
		{
			sudokuGame.Clear();
		}

		#region Input

		private void Move(MoveDirection moveDirection)
		{
			int row, column;
			sudokuGridView.GetCurrentCellCoordiantes(out row, out column);

			bool moved = false;
			if ((row >= 0) && (column>=0))
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
			switch(key)
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

			return sudokuGame.GetInvalidSudokuDigit();
		}

		public void Set(Key key)
		{
			// Key to sukdou digit
			int sudokuDigit = SudokuDigitFromKey(key);
			if (sudokuDigit != sudokuGame.GetInvalidSudokuDigit())
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
			if ( moveDirection != MoveDirection.None )
			{
				Move(moveDirection);
			}
		}
		#endregion

		private void KeyUp(object sender, Key key)
		{
			Set(key);
		}

		private void ClearCommand(object sender, EventArgs e)
		{
			sudokuGame.Clear();
			UpdateValues();
		}
	}
}
