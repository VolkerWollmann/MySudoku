using System;
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
			// set the value text block
			TextBlock vtb = new TextBlock();
			vtb.Text = "-";
			vtb.Name = "V_" + row.ToString() + "_" + column.ToString();
			vtb.FontSize = 16;
			vtb.HorizontalAlignment = HorizontalAlignment.Center;
			vtb.VerticalAlignment = VerticalAlignment.Center;

			// set the set value text box
			TextBox stb = new TextBox();
			stb.Text = "{1,2,3,4,5,6,7,8,9}";
			stb.Name = "S_" + row.ToString() + "_" + column.ToString();
			stb.TextWrapping = TextWrapping.Wrap;
			stb.FontSize = 10;
			stb.HorizontalAlignment = HorizontalAlignment.Center;
			stb.VerticalAlignment = VerticalAlignment.Center;

			// set the panel
			StackPanel stackPanel = new StackPanel();
			stackPanel.VerticalAlignment = VerticalAlignment.Stretch;
			stackPanel.HorizontalAlignment = HorizontalAlignment.Stretch;
			stackPanel.Children.Add(vtb);
			stackPanel.Children.Add(stb);
			stackPanel.MouseLeftButtonDown += StackPanel_MouseLeftButtonDown;
			stackPanel.Background = new SolidColorBrush(Colors.White);
			stackPanel.Tag = gameGridView;


			// bind SudokuCellView to the SudokuCellModel
			SudokuCell sudokuCell = new SudokuCell();
			Binding myBinding = new Binding(SudokuCell.SudokuCellValueName);
			myBinding.Source = sudokuCell;
			vtb.SetBinding(TextBlock.TextProperty, myBinding);
			vtb.Tag = sudokuCell;

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
