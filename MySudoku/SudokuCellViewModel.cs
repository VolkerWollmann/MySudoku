using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MySudoku
{
    
	public class SudokuCellViewModel
	{
		public const int InvalidSudokuDigit = -1;
		public static StackPanel GetSudokuCell(int row, int column, GameGridViewModel gameGridViewModel)
		{
			// set the text box
			TextBlock tb = new TextBlock();
			tb.Text = row.ToString() + column.ToString();
			tb.Name = "N" + tb.Text;
			tb.FontSize = 16;
			//tb.MouseLeftButtonDown += Tb_MouseLeftButtonDown;
			tb.HorizontalAlignment = HorizontalAlignment.Center;
			tb.VerticalAlignment = VerticalAlignment.Center;

			// set the panel
			StackPanel stackPanel = new StackPanel();
			stackPanel.VerticalAlignment = VerticalAlignment.Stretch;
			stackPanel.HorizontalAlignment = HorizontalAlignment.Stretch;
			stackPanel.Children.Add(tb);
			stackPanel.MouseLeftButtonDown += StackPanel_MouseLeftButtonDown;
			stackPanel.Background = new SolidColorBrush(Colors.White);

			stackPanel.Tag = gameGridViewModel;

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
			GameGridViewModel gameGridViewModel = (GameGridViewModel)stackPanel.Tag;

			gameGridViewModel.MarkCell(stackPanel);
		 
			//CurrentCell = stackPanel;
		}

		public static void Set(StackPanel stackPanel, int sudokuDigit)
		{
			TextBlock tb = stackPanel.Children.OfType<TextBlock>().First();
			tb.Text = sudokuDigit.ToString();
		}
	}
}
