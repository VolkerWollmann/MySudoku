using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace MySudoku.Controls
{
	/// <summary>
	/// Interaction logic for SudokuCellUserControl.xaml
	/// </summary>
	public partial class SudokuCellUserControl : UserControl, INotifyPropertyChanged
	{
		private SudokuGridUserControl sudokuGridUserControl;

		public event PropertyChangedEventHandler PropertyChanged;

		private void NotifyPropertyChanged(string info)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(info));
			}
		}

		public int Row { get; private set; }
		public int Column { get; private set; }

		string _value;
		public string Value {
			set
			{
				_value = value;
				NotifyPropertyChanged("Value");
			}

			get
			{
				return _value;
			}
		}

		string _possibleValueSet;
		public string PossibleValueSet
		{
			set
			{
				_possibleValueSet = value;
				NotifyPropertyChanged("PossibleValueSet");
			}

			get
			{
				return _possibleValueSet;
			}
		}

		public SudokuCellUserControl(SudokuGridUserControl _sudokuGridUserControl, int _row, int _column) : this()
		{
			sudokuGridUserControl = _sudokuGridUserControl;
			Row = _row;
			Column = _column;

			// Set border
			SudokuCellControlBorder.BorderThickness = new Thickness
			{
				Left = 1,
				Right = (Column == 2) || (Column == 5) ? 3 : 1,
				Bottom = (Row == 2) || (Row == 5) ? 3 : 1,
				Top = 1
			};

			SudokuCellControlBorder.BorderBrush = new SolidColorBrush(Colors.Black);
		}

		public SudokuCellUserControl()
		{
			InitializeComponent();
			this.DataContext = this;

			Value = "-";
			PossibleValueSet = "---";

			SudokuCellControlPanel.MouseLeftButtonDown += SudokuCellControlPanel_MouseLeftButtonDown;

			this.KeyUp += SudokuCellUserControl_KeyUp;
		}

		private void SudokuCellUserControl_KeyUp(object sender, KeyEventArgs e)
		{
			sudokuGridUserControl.EventHandlerKey(sender, e.Key);
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
			sudokuGridUserControl.MarkCell(Row, Column);
			TextBoxFocus.Focus();
		}
	}
}
