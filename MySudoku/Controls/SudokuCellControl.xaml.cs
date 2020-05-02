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
		private GameGridViewModel gameGridView;
		public SudokuCell SudokuCell { get; private set; }
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

		public SudokuCellControl(GameGridViewModel _gameGridView, SudokuCell _sudokuCell) : this()
		{
			gameGridView = _gameGridView;
			SudokuCell = _sudokuCell;

			// Set border
			SudokuCellControlBorder.BorderThickness = new Thickness
			{
				Left = 1,
				Right = (SudokuCell.Column == 2) || (SudokuCell.Column == 5) ? 3 : 1,
				Bottom = (SudokuCell.Row == 2) || (SudokuCell.Row == 5) ? 3 : 1,
				Top = 1
			};

			SudokuCellControlBorder.BorderBrush = new SolidColorBrush(Colors.Black);

			// bind SudokuCellView to the SudokuCellModel for the value
			Binding valueBinding = new Binding(SudokuCell.SudokuCellValueName);
			valueBinding.Source = SudokuCell;
			TextBlockValue.SetBinding(TextBlock.TextProperty, valueBinding);

			// bind SudokuCellView to the SudokuCellModel for the possible values
			Binding possibleValuesBinding = new Binding(SudokuCell.SudokuCellPossibleValuesName);
			possibleValuesBinding.Source = SudokuCell;
			TextBlockPossibleValueSet.SetBinding(TextBlock.TextProperty, possibleValuesBinding);
		}
		public SudokuCellControl()
		{
			InitializeComponent();
			Value = "0";
			PossibleValueSet = "{-}";

			SudokuCellControlPanel.MouseLeftButtonDown += SudokuCellControlPanel_MouseLeftButtonDown;
		}

		public void Mark()
		{
			SudokuCellControlPanel.Background = new SolidColorBrush(Colors.LightGreen);
			TextBlockValue.Background = new SolidColorBrush(Colors.LightGreen);
			TextBlockPossibleValueSet.Background = new SolidColorBrush(Colors.LightGreen);
		}

		public void UnMark()
		{
			SudokuCellControlPanel.Background = new SolidColorBrush(Colors.White);
			TextBlockValue.Background = new SolidColorBrush(Colors.White);
			TextBlockPossibleValueSet.Background = new SolidColorBrush(Colors.White);
		}

		private void SudokuCellControlPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			gameGridView.MarkCell(this);
		}
	}
}
