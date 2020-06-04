using System.ComponentModel;
using System.Threading;
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
		#region Constants
		private static SolidColorBrush SCBAntiAntiqueWhite = new SolidColorBrush(Colors.AntiqueWhite);
		private static SolidColorBrush SCBWhite = new SolidColorBrush(Colors.White);
		private static SolidColorBrush SCBLightGreen = new SolidColorBrush(Colors.LightGreen);
		#endregion

		private SudokuGridUserControl SudokuGridUserControl;

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
				AdjustBackGroundColor();
			}

			get
			{
				return _possibleValueSet;
			}
		}

		SolidColorBrush _backGroundColor;
		public SolidColorBrush BackGroundColor
		{
			set
			{
				_backGroundColor = value;
				NotifyPropertyChanged("BackGroundColor");
			}

			get
			{
				return _backGroundColor;
			}
		}

		private void AdjustBackGroundColor()
		{
			BackGroundColor = _possibleValueSet != "" ? SCBAntiAntiqueWhite : SCBWhite ;
		}

		public SudokuCellUserControl(SudokuGridUserControl _sudokuGridUserControl, int row, int column) : this()
		{
			SudokuGridUserControl = _sudokuGridUserControl;
			Row = row;
			Column = column;

			// Set border
			SudokuCellControlBorder.BorderThickness = new Thickness
			{
				Left = 1,
				Right = (Column == 2) || (Column == 5) ? 3 : 1,
				Bottom = (Row == 2) || (Row == 5) ? 3 : 1,
				Top = 1
			};

			SudokuCellControlBorder.BorderBrush = new SolidColorBrush(Colors.Black);
			BackGroundColor = SCBAntiAntiqueWhite;
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
			SudokuGridUserControl.EventHandlerKey(sender, e.Key);
		}

		public void Mark()
		{
			BackGroundColor = new SolidColorBrush(Colors.LightGreen);
		}

		public void UnMark()
		{
			AdjustBackGroundColor();
		}

		private void SudokuCellControlPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			SudokuGridUserControl.MarkCell(Row, Column);
			TextBoxFocus.Focus();
		}
	}
}
