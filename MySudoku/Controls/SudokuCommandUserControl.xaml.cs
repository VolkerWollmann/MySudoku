using MySudoku.Constants;
using MySudoku.Interfaces;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace MySudoku.Controls
{
	/// <summary>
	/// Interaction logic for SudokuCommandUserControl.xaml
	/// </summary>
	public partial class SudokuCommandUserControl : UserControl, ISudokuCommandView, INotifyPropertyChanged
	{
		private readonly EventHandler[] EventHandler = new EventHandler[6];

		public SudokuCommandUserControl()
		{
			InitializeComponent();
			this.DataContext = this;

			this.ButtonCommandBack.Tag = SudokuCommand.Back;
			this.ButtonCommandClear.Tag = SudokuCommand.Clear;
			this.ButtonCommandNew.Tag = SudokuCommand.New;
			this.ButtonCommandSolve.Tag = SudokuCommand.Solve;
			this.ButtonCommandTogglePossibleValues.Tag = SudokuCommand.TogglePossibleValues;
		}

		public UIElement GetUIElement()
		{
			return this;
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void NotifyPropertyChanged(String info)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }

		bool _ButtonsEnabled = true;
		public bool ButtonsEnabled
		{
			get => _ButtonsEnabled;

            private set
			{
				_ButtonsEnabled = value;
				NotifyPropertyChanged("ButtonsEnabled");
			}
		}
		public bool SetButtonsEnabled(bool buttonsEnabled)
		{
			ButtonsEnabled  = buttonsEnabled;
			return ButtonsEnabled;
		}

		public int GetNumberOfCellsToFill()
		{
            if (!int.TryParse(Numbers.Text, out var numberOfCellsToFill))
			{
				numberOfCellsToFill = 54;
				Numbers.Text = "54";
			}
			else if ((numberOfCellsToFill <0) || (numberOfCellsToFill>81))
			{
				numberOfCellsToFill = 54;
				Numbers.Text = "54";
			}
			return numberOfCellsToFill;
		}

		public void SetCommandEventHandler(SudokuCommand sudokuCommand, EventHandler eventHandler)
		{
			EventHandler[(int)sudokuCommand] += eventHandler;
		}

		private void ButtonCommand_Click(object sender, RoutedEventArgs e)
		{
			if (sender is Button b)
            {
				EventHandler[(int)b.Tag](sender,e);
            }
		}
	}
}
