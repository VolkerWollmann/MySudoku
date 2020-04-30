using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace MySudoku
{
	public class GameGridView
	{
		public enum MoveDirection
		{
			None,
			Up,
			Down,
			Left,
			Right
		};

		public static StackPanel CurrentCell { get; set; } = null;

		private Grid GameGrid;

		private SudokuGrid sudokuGrid;

		StackPanel[,] StackPanelGrid;
		public GameGridView(Grid gameGrid)
		{
			sudokuGrid = new SudokuGrid();

			GameGrid = gameGrid;
			StackPanelGrid = new StackPanel[9, 9];

			for (int row = 0; row < 9; row++)
			{
				for (int column = 0; column < 9; column++)
				{
					StackPanel stackPanel = GameCellView.GetStackPanel(this, sudokuGrid.GetSudokuCell(row, column));
					StackPanelGrid[row, column] = stackPanel;

					GameGrid.Children.Add(stackPanel);
					Grid.SetRow(stackPanel, row);
					Grid.SetColumn(stackPanel, column);

					Border border = GameCellView.GetBorder();
					GameGrid.Children.Add(border);
					Grid.SetRow(border, row);
					Grid.SetColumn(border, column);
				}
			}
		}


		public void MarkCell(StackPanel stackPanel)
		{
			if (CurrentCell != null)
			{
				CurrentCell.Background = new SolidColorBrush(Colors.White);
				CurrentCell.Children.OfType<TextBlock>().ToList().ForEach(
					tb => { tb.Background = new SolidColorBrush(Colors.White); });
			}

			CurrentCell = stackPanel;

			CurrentCell.Background = new SolidColorBrush(Colors.LightGreen);
			CurrentCell.Children.OfType<TextBlock>().ToList().ForEach(
					tb => { tb.Background = new SolidColorBrush(Colors.LightGreen); });
		}

		private void Move(MoveDirection moveDirection)
		{
			StackPanel newCell = null;
			bool found = false;
			int currentRow=0, currentColumn=0;

			for (int row = 0; row < 9; row++)
			{
				for (int column = 0; column < 9; column++)
				{
					if (StackPanelGrid[row, column] == CurrentCell )
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
					newCell = StackPanelGrid[currentRow, currentColumn];
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

			return -1;
		}

		public void Set(Key key)
		{
			// Key to sukdou digit
			int sudokuDigit = SudokuDigitFromKey(key);
			if (sudokuDigit != GameCellView.InvalidSudokuDigit)
			{
				if ( CurrentCell != null )
					GameCellView.Set(CurrentCell,sudokuDigit);
				return;
			}
			MoveDirection moveDirection = MoveDirectionFromKey(key);
			if ( moveDirection != MoveDirection.None )
			{
				Move(moveDirection);
			}
		}
	}
}
