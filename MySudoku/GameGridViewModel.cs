﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace MySudoku
{
	public class GameGridViewModel
	{
		public enum MyEnum
		{
			Up,
			Down,
			Left,
			Right
		};

		public static StackPanel CurrentCell { get; set; } = null;

		private Grid GameGrid;

		StackPanel[,] StackPanelGrid;
		public GameGridViewModel(Grid gameGrid)
		{
			GameGrid = gameGrid;
			StackPanelGrid = new StackPanel[9, 9];

			for (int row = 0; row < 9; row++)
			{
				for (int column = 0; column < 9; column++)
				{
					StackPanel stackPanel = SudokuCellViewModel.GetSudokuCell(row, column, this);
					StackPanelGrid[row, column] = stackPanel;

					gameGrid.Children.Add(stackPanel);
					Grid.SetRow(stackPanel, row);
					Grid.SetColumn(stackPanel, column);

					Border border = SudokuCellViewModel.GetBorder();
					gameGrid.Children.Add(border);
					Grid.SetRow(border, row);
					Grid.SetColumn(border, column);
				}
			}
		}


		public void MarkCell(StackPanel stackPanel)
		{
			if (CurrentCell != null)
				CurrentCell.Background = new SolidColorBrush(Colors.White);

			CurrentCell = stackPanel;

			CurrentCell.Background = new SolidColorBrush(Colors.LightGreen);
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
			if (sudokuDigit != SudokuCellViewModel.InvalidSudokuDigit)
			{
				if ( CurrentCell != null )
					SudokuCellViewModel.Set(CurrentCell,sudokuDigit);
			}
		}
	}
}
