using MySudoku.Interfaces;
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
	/// Interaction logic for SudokuCommandUserControl.xaml
	/// </summary>
	public partial class SudokuCommandUserControl : UserControl, ISudokuCommands
	{
		private EventHandler ClearCommandEventHandler;
		private EventHandler BackCommmandEventHandler;
		public SudokuCommandUserControl()
		{
			InitializeComponent();
		}

		public UIElement GetUIElement()
		{
			return this as UIElement;
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
			if (BackCommmandEventHandler != null)
				BackCommmandEventHandler(sender, e);
		}

		public void SetBackCommandEventHandler(EventHandler eventHandler)
		{
			BackCommmandEventHandler += eventHandler;
		}
	}
}
