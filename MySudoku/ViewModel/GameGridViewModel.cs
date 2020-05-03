using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using MySudoku.Controls;

namespace MySudoku
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

		public static SudokuCellControl  CurrentSudokuCellControl { get; set; } = null;

		// Grid from program
		private Grid mySudokuGrid;

		// The game grid from the model
		private SudokuGrid sudokuGrid;

		// The view controls
		SudokuGridUserControl sudokuGridUserControl; 
		SudokuCellControl [,] SudokuCellControlGrid;
		public GameGridViewModel(Grid _mySudokuGrid)
		{
			mySudokuGrid = _mySudokuGrid;

			// prepare model
			sudokuGrid = new SudokuGrid();

			// prepare view
			sudokuGridUserControl = new SudokuGridUserControl();
			mySudokuGrid.Children.Add(sudokuGridUserControl);
			Grid.SetRow(sudokuGridUserControl, 0);
			Grid.SetColumn(sudokuGridUserControl, 0);

			SudokuCellControlGrid = new SudokuCellControl[9, 9];

			for (int row = 0; row < 9; row++)
			{
				for (int column = 0; column < 9; column++)
				{
					SudokuCell sudokuCell = sudokuGrid.GetSudokuCell(row, column);
					SudokuCellControl sudokuCellControl = new SudokuCellControl(this, sudokuCell);
					SudokuCellControlGrid[row, column] = sudokuCellControl;

					sudokuGridUserControl.SudokuGrid.Children.Add(sudokuCellControl);
					Grid.SetRow(sudokuCellControl, row);
					Grid.SetColumn(sudokuCellControl, column);

				}
			}
		}

		public void Clear()
		{
			sudokuGrid.Clear();
		}

		#region Input
		public void MarkCell(SudokuCellControl sudokuCellControl)
		{
			if (CurrentSudokuCellControl != null)
				CurrentSudokuCellControl.UnMark();

			CurrentSudokuCellControl = sudokuCellControl;

			CurrentSudokuCellControl.Mark();
		}

		private void Move(MoveDirection moveDirection)
		{
			SudokuCellControl newCell = null;
			bool found = false;
			int currentRow=0, currentColumn=0;

			for (int row = 0; row < 9; row++)
			{
				for (int column = 0; column < 9; column++)
				{
					if (SudokuCellControlGrid[row, column] == CurrentSudokuCellControl)
					{
						currentRow = row;
						currentColumn = column;
						found = true;
						break;
					}
				}

				if (found)
					break;
			}

			bool moved = false;
			if (found)
			{
				if (moveDirection == MoveDirection.Up)
				{
					if (currentRow > 0)
					{
						moved = true;
						currentRow--;
					}
				}
				else if (moveDirection == MoveDirection.Down)
				{
					if (currentRow < 8)
					{
						moved = true;
						currentRow++;
					}
				}
				else if (moveDirection == MoveDirection.Left)
				{
					if (currentColumn > 0)
					{
						moved = true;
						currentColumn--;
					}
				}
				else if (moveDirection == MoveDirection.Right)
				{
					if (currentColumn < 8)
					{
						moved = true;
						currentColumn++;
					}
				}

				if (moved)
				{
					newCell = SudokuCellControlGrid[currentRow, currentColumn];
					MarkCell(newCell);
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

			return SudokuCell.InvalidSudokuDigit;
		}

		public void Set(Key key)
		{
			// Key to sukdou digit
			int sudokuDigit = SudokuDigitFromKey(key);
			if (sudokuDigit != SudokuCell.InvalidSudokuDigit)
			{
				if (CurrentSudokuCellControl != null)
					CurrentSudokuCellControl.SudokuCell.SetValue(sudokuDigit);
				return;
			}
			MoveDirection moveDirection = MoveDirectionFromKey(key);
			if ( moveDirection != MoveDirection.None )
			{
				Move(moveDirection);
			}
		}
		#endregion

	}
}
