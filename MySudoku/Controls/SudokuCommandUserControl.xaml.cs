using MySudoku.Constants;
using MySudoku.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
	/// Interaction logic for SudokuCommandUserControl.xaml
	/// </summary>
	public partial class SudokuCommandUserControl : UserControl, ISudokuCommandView, INotifyPropertyChanged
	{
		private EventHandler[] EventHandler = new EventHandler[6];

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
			return this as UIElement;
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void NotifyPropertyChanged(String info)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(info));
			}
		}

		bool _buttonsEnabled = true;
		public bool ButtonsEnabled
		{
			get
			{
				return _buttonsEnabled;
			}

			private set
			{
				_buttonsEnabled = value;
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
			int numberOfCellsToFill;
			if (!int.TryParse(Numbers.Text, out numberOfCellsToFill))
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
