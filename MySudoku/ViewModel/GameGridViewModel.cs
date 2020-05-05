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
		private Grid mySudokuGrid;

		// The game grid from the model
		private ISudokuModel sudokuGrid;

		// The view control
		ISudokuGridControl sudokuGridUserControl;

		// The binding information
		SudokuData[,] sudokuDatas = new SudokuData[9, 9];

		private void UpdateValues()
		{
			for (int row = 0; row < 9; row++)
			{
				for (int column = 0; column < 9; column++)
				{
					sudokuDatas[row, column].SetValue(sudokuGrid.GetCellValue(row,column));
					sudokuDatas[row, column].SetPossibleValues(sudokuGrid.GetSudokuCellPossibleValues(row,column));
				}
			}
		}

  		public GameGridViewModel(Grid _mySudokuGrid)
		{ 
			mySudokuGrid = _mySudokuGrid;

			// prepare model
			sudokuGrid = new SudokuGrid();

			// prepare view
			sudokuGridUserControl = (ISudokuGridControl) new SudokuGridUserControl();
			
			mySudokuGrid.Children.Add(sudokuGridUserControl.GetUIElement());
			Grid.SetRow(sudokuGridUserControl.GetUIElement(), 0);
			Grid.SetColumn(sudokuGridUserControl.GetUIElement(), 0);

		    // prepare the binding
			for (int row = 0; row < 9; row++)
			{
				for (int column = 0; column < 9; column++)
				{
					sudokuDatas[row, column] = new SudokuData();

					Binding valueBinding = new Binding("Value");
					valueBinding.Source = sudokuDatas[row, column];
					sudokuGridUserControl.BindValue(row, column, valueBinding);

					Binding possibleValuesBinding = new Binding("PossibleValues");
					possibleValuesBinding.Source = sudokuDatas[row, column];
					sudokuGridUserControl.BindPossibleValues(row, column, possibleValuesBinding);
				}
			}

			UpdateValues();

			// prepare Key operation
			sudokuGridUserControl.SetKeyEventHandler(KeyUp);
		}

		public void Clear()
		{
			sudokuGrid.Clear();
		}

		#region Input

		private void Move(MoveDirection moveDirection)
		{
			int row, column;
			sudokuGridUserControl.GetCurrentCellCoordiantes(out row, out column);

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
					sudokuGridUserControl.MarkCell(row, column);
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

			return sudokuGrid.GetInvalidSudokuDigit();
		}

		public void Set(Key key)
		{
			// Key to sukdou digit
			int sudokuDigit = SudokuDigitFromKey(key);
			if (sudokuDigit != sudokuGrid.GetInvalidSudokuDigit())
			{
				int row, column;
				sudokuGridUserControl.GetCurrentCellCoordiantes(out row, out column);

				if ((row >= 0) && (column >= 0))
				{
					sudokuGrid.SetValue(row, column, sudokuDigit);
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
	}
}
