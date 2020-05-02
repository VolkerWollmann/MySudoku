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
	
		public static SudokuCellControl GetSudokuCellControl(GameGridView gameGridView, SudokuCell sudokuCell)
		{
			SudokuCellControl sudokuCellControl = new SudokuCellControl(gameGridView, sudokuCell);
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
<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
=======
		private static void StackPanel_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			StackPanel stackPanel = (StackPanel)sender;
			GameGridView gameGridViewModel = (GameGridView)stackPanel.Tag;

			gameGridViewModel.MarkCell(stackPanel);
		 
		}
>>>>>>> Eleminate Comment
=======
>>>>>>> SudokuCellUserControl
=======
>>>>>>> SudokuCellUserControl

		#endregion
	}
}
