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
    
	public class SudokuCell
	{
		public static StackPanel previousStackPanel = null;

		public SudokuCell(int row, int column, Grid gameGrid )
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
			gameGrid.Children.Add(stackPanel);
			Grid.SetRow(stackPanel, row);
			Grid.SetColumn(stackPanel, column);

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

			gameGrid.Children.Add(border);
			Grid.SetRow(border, row);
			Grid.SetColumn(border, column);
		}

		private void StackPanel_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			if (previousStackPanel != null)
				previousStackPanel.Background = new SolidColorBrush(Colors.White);

			StackPanel stackPanel = (StackPanel)sender;
			stackPanel.Background = new SolidColorBrush(Colors.LightGreen);

			previousStackPanel = stackPanel;
		}
	}
}
