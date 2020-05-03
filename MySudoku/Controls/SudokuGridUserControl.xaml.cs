using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MySudoku.Controls
{
	/// <summary>
	/// Interaction logic for SudokuGridUserControl.xaml
	/// </summary>
	public partial class SudokuGridUserControl : UserControl
	{
		SudokuCellControl[,] SudokuCellControlGrid;
		public SudokuCellControl CurrentSudokuCellControl { get; private set; } = null;

		public SudokuGridUserControl()
		{
			InitializeComponent();

			SudokuCellControlGrid = new SudokuCellControl[9, 9];

			for (int row = 0; row < 9; row++)
			{
				for (int column = 0; column < 9; column++)
				{		
					SudokuCellControl sudokuCellControl = new SudokuCellControl(this, row, column);
					SudokuCellControlGrid[row, column] = sudokuCellControl;

					SudokuGrid.Children.Add(sudokuCellControl);
					Grid.SetRow(sudokuCellControl, row);
					Grid.SetColumn(sudokuCellControl, column);

				}
			}
		}

		public void BindValue(int row, int column, Binding binding)
		{
			SudokuCellControlGrid[row, column].BindValue(binding);
		}

		public void BindPossibleValues(int row, int column, Binding binding)
		{
			SudokuCellControlGrid[row, column].BindPossibleValues(binding);
		}

		public void MarkCell(int row, int column)
		{
			if (CurrentSudokuCellControl != null)
				CurrentSudokuCellControl.UnMark();

			CurrentSudokuCellControl = SudokuCellControlGrid[row, column];

			CurrentSudokuCellControl.Mark();

		}

		public void GetCurrentCoordiantes(out int row, out int column)
		{
			row = -1;
			column = -1;

			if (CurrentSudokuCellControl != null)
			{
				row = CurrentSudokuCellControl.Row;
				column = CurrentSudokuCellControl.Column;
			}
		}
	}
}
