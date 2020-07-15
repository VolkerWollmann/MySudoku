using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace MySudoku.Controls
{
	class TestCommand : ICommand
	{
		SudokuCellUserControl SudokuCellUserControl;
		public event EventHandler CanExecuteChanged;

		public bool CanExecute(object parameter)
		{
			return true;
		}

		public void Execute(object parameter)
		{
			List<Tuple<string, Key>> mapping = new List<Tuple<string, Key>>()
			{ 
				new Tuple<string, Key>( "1", Key.D1 ),
				new Tuple<string, Key>( "2", Key.D2 ),
				new Tuple<string, Key>( "3", Key.D3 ),
				new Tuple<string, Key>( "4", Key.D4 ),
				new Tuple<string, Key>( "5", Key.D5 ),
				new Tuple<string, Key>( "6", Key.D6 ),
				new Tuple<string, Key>( "7", Key.D7 ),
				new Tuple<string, Key>( "8", Key.D8 ),
				new Tuple<string, Key>( "9", Key.D9 ),
			};
			var key = mapping.Where(e => (e.Item1 == (string)parameter)).First().Item2;
			SudokuCellUserControl.RaiseEventHandlerKey(this, key);
		}

		public TestCommand(SudokuCellUserControl sudokuCellUserControl)
		{
			SudokuCellUserControl = sudokuCellUserControl;
		}
	}
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

		public ICommand NumberOneCommand
		{
			get
			{
				return new TestCommand(this);
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
			SudokuCellControlPanel.MouseRightButtonDown += SudokuCellControlPanel_MouseLeftButtonDown;

			this.KeyUp += SudokuCellUserControl_KeyUp;
		}

		internal void RaiseEventHandlerKey(object sender, Key key)
		{
			SudokuGridUserControl.EventHandlerKey(sender, key);
		}
		private void SudokuCellUserControl_KeyUp(object sender, KeyEventArgs e)
		{
			RaiseEventHandlerKey(sender, e.Key);
		}

		public void Mark()
		{
			BackGroundColor = new SolidColorBrush(Colors.LightGreen);
			TextBoxFocus.Focus();
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
