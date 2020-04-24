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

namespace MySudoku
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public const int InvalidSudokuDigit=-1;
		public MainWindow()
		{
			InitializeComponent();


			for (int row = 0; row < 9; row++)
			{
				for (int column = 0; column < 9; column++)
				{
					var x = new SudokuCell(row, column, GameGrid);
				}
			}
		}

		/// <summary>
		/// Transforms the key for 1-9, otherwise -1
		/// </summary>
		/// <param name="key">pressed key</param>
		/// <returns>return -1, 1-9</returns>
		private int SudokuDigitFromKey( Key key )
		{
			if (key >= Key.D1 && key <= Key.D9 )
			{
				return (key - Key.D0);
			}

			if (key >=  Key.NumPad1  && key <= Key.NumPad9 )
			{
				return (key - Key.NumPad0);
			}

			return -1;
		}
		private void MainWindow_KeyUp(object sender, KeyEventArgs e)
		{
			// Key to sukdou digit
			int sudokuDigit = SudokuDigitFromKey(e.Key);
			if ( sudokuDigit != InvalidSudokuDigit )
			{
				SudokuCell.Set(sudokuDigit);
			}
			
		}

	}
}