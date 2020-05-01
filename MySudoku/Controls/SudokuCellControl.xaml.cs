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
	/// Interaction logic for SudokuCellControl.xaml
	/// </summary>
	public partial class SudokuCellControl : UserControl
	{
		private GameGridView gameGridView;
		private SudokuCell sudokuCell;
		public string Value {
			set
			{
				TextBlockValue.Text = value;
			}

			get
			{
				return TextBlockValue.Text;
			}
		}

		public string PossibleValueSet
		{
			set
			{
				TextBlockPossibleValueSet.Text = value;
			}

			get
			{
				return TextBlockPossibleValueSet.Text;
			}
		}

		public SudokuCellControl(GameGridView _gameGridView, SudokuCell _sudokuCell) : this()
		{
			gameGridView = _gameGridView;
			sudokuCell = _sudokuCell;
		}
		public SudokuCellControl()
		{
			InitializeComponent();
			this.DataContext = this;
			Value = "0";
			PossibleValueSet = "{-}";
		}
	}
}
