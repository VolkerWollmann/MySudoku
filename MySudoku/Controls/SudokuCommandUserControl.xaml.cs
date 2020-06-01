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
	public partial class SudokuCommandUserControl : UserControl, ISudokuCommands, INotifyPropertyChanged
	{
		private EventHandler ClearCommandEventHandler;
		private EventHandler BackCommandEventHandler;
		private EventHandler NewCommandEventHandler;

		public SudokuCommandUserControl()
		{
			InitializeComponent();
			this.DataContext = this;
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

		public void SetClearCommandEventHandler(EventHandler eventHandler)
		{
			ClearCommandEventHandler += eventHandler;
		}

		private void ButtonCommandClear_Click(object sender, RoutedEventArgs e)
		{
			if(ClearCommandEventHandler!=null)
				ClearCommandEventHandler(sender, e);
		}

		private void ButtonCommandBack_Click(object sender, RoutedEventArgs e)
		{
			if (BackCommandEventHandler != null)
				BackCommandEventHandler(sender, e);
		}

		public void SetBackCommandEventHandler(EventHandler eventHandler)
		{
			BackCommandEventHandler += eventHandler;
		}

		private void ButtonCommandNew_Click(object sender, RoutedEventArgs e)
		{
			if (NewCommandEventHandler != null)
				NewCommandEventHandler(sender, e);
		}

		public void SetNewCommandEventHandler(EventHandler eventHandler)
		{
			NewCommandEventHandler += eventHandler;
		}
	}
}
