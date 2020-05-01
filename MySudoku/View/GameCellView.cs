using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using MySudoku.Controls;

namespace MySudoku
{
	public class GameCellView
	{
		public const int InvalidSudokuDigit = -1;
		public static StackPanel GetStackPanel(GameGridView gameGridView, SudokuCell sudokuCell)
		{
			// set the value text block for the value
			TextBlock vtb = new TextBlock();
			vtb.Text = "-";
			vtb.Name = "V_" + sudokuCell.Row.ToString() + "_" + sudokuCell.Column.ToString();
			vtb.FontSize = 16;
			vtb.HorizontalAlignment = HorizontalAlignment.Center;
			vtb.VerticalAlignment = VerticalAlignment.Center;

			// set the set value text block for the possible values
			TextBlock pvtb = new TextBlock();
			pvtb.Text = "{1,2,3,4,5,6,7,8,9}";
			pvtb.Name = "S_" + sudokuCell.Row.ToString() + "_" + sudokuCell.Column.ToString();
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


			//SudokuCell sudokuCell = ;

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

		public static SudokuCellControl GetSudokuCellControl(GameGridView gameGridView, SudokuCell sudokuCell)
		{
			SudokuCellControl sudokuCellControl = new SudokuCellControl();
			return sudokuCellControl;
		}

		public static Border GetBorder(SudokuCell sudokuCell)
		{
			Border border = new Border()
			{
				BorderThickness = new Thickness
				{
					Left = 1,
					Right = (sudokuCell.Column == 2) || (sudokuCell.Column == 5) ? 3 : 1,
					Bottom = (sudokuCell.Row == 2) || (sudokuCell.Row == 5) ? 3 : 1,
					Top = 1
				},
				BorderBrush = new SolidColorBrush(Colors.Black),
			};

			return border;
		}

		#region Input
		private static void StackPanel_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			//if (CurrentCell != null)
			//	CurrentCell.Background = new SolidColorBrush(Colors.White);

			SudokuCellControl sudokuCellControl = (SudokuCellControl)sender;
			GameGridView gameGridViewModel = (GameGridView)sudokuCellControl.Tag;

			gameGridViewModel.MarkCell(sudokuCellControl);
		 
			//CurrentCell = stackPanel;
		}

		public static void Set(SudokuCellControl sudokuCellControl, int sudokuDigit)
		{
			//TextBlock tb = stackPanel.Children.OfType<TextBlock>().
			//	Where(e => e.Name.StartsWith("V_")).First();
			//SudokuCell sudokuCell = (SudokuCell)tb.Tag;
			//sudokuCell.SetValue(sudokuDigit);
		}
		#endregion
	}
}
