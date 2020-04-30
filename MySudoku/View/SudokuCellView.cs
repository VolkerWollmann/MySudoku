﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace MySudoku
{
    
	public class SudokuCellView
	{
		public const int InvalidSudokuDigit = -1;
		public static StackPanel GetSudokuCell(int row, int column, GameGridView gameGridView)
		{
			// set the value text block for the value
			TextBlock vtb = new TextBlock();
			vtb.Text = "-";
			vtb.Name = "V_" + row.ToString() + "_" + column.ToString();
			vtb.FontSize = 16;
			vtb.HorizontalAlignment = HorizontalAlignment.Center;
			vtb.VerticalAlignment = VerticalAlignment.Center;

			// set the set value text block for the possible values
			TextBlock pvtb = new TextBlock();
			pvtb.Text = "{1,2,3,4,5,6,7,8,9}";
			pvtb.Name = "S_" + row.ToString() + "_" + column.ToString();
			pvtb.TextWrapping = TextWrapping.Wrap;
			pvtb.FontSize = 10;
			pvtb.HorizontalAlignment = HorizontalAlignment.Center;
			pvtb.VerticalAlignment = VerticalAlignment.Center;

			// set the panel
			StackPanel stackPanel = new StackPanel();
			stackPanel.VerticalAlignment = VerticalAlignment.Stretch;
			stackPanel.HorizontalAlignment = HorizontalAlignment.Stretch;
			stackPanel.Children.Add(vtb);
			stackPanel.Children.Add(pvtb);
			stackPanel.MouseLeftButtonDown += StackPanel_MouseLeftButtonDown;
			stackPanel.Background = new SolidColorBrush(Colors.White);
			stackPanel.Tag = gameGridView;


			SudokuCell sudokuCell = new SudokuCell();

			// bind SudokuCellView to the SudokuCellModel for the value
			Binding valueBinding = new Binding(SudokuCell.SudokuCellValueName);
			valueBinding.Source = sudokuCell;
			vtb.SetBinding(TextBlock.TextProperty, valueBinding);
			vtb.Tag = sudokuCell;

			// bind SudokuCellView to the SudokuCellModel for the possible values
			Binding possibleValuesBinding = new Binding(SudokuCell.SudokuCellPossibleValuesName);
			possibleValuesBinding.Source = sudokuCell;
			pvtb.SetBinding(TextBlock.TextProperty, possibleValuesBinding);
			pvtb.Tag = sudokuCell;

			return stackPanel;
		}

		public static Border GetBorder()
		{
			Border border = new Border()
			{
				BorderThickness = new Thickness
				{
					Left = 1,
					Right = 1,
					Bottom = 1,
					Top = 1
				},
				BorderBrush = new SolidColorBrush(Colors.Black),
			};

			return border;
		}
			

		private static void StackPanel_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			//if (CurrentCell != null)
			//	CurrentCell.Background = new SolidColorBrush(Colors.White);

			StackPanel stackPanel = (StackPanel)sender;
			GameGridView gameGridViewModel = (GameGridView)stackPanel.Tag;

			gameGridViewModel.MarkCell(stackPanel);
		 
			//CurrentCell = stackPanel;
		}

		public static void Set(StackPanel stackPanel, int sudokuDigit)
		{
			TextBlock tb = stackPanel.Children.OfType<TextBlock>().
				Where(e => e.Name.StartsWith("V_")).First();
			SudokuCell sudokuCell = (SudokuCell)tb.Tag;
			sudokuCell.SetValue(sudokuDigit);
		}
	}
}
