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
using MySudoku;

namespace MySudoku
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		GameGridView gameGridView;
		public MainWindow()
		{
			InitializeComponent();
			gameGridView = new GameGridView(GameGrid);
		}



		private void MainWindow_KeyUp(object sender, KeyEventArgs e)
		{
			gameGridView.Set(e.Key);
		}

	}
}