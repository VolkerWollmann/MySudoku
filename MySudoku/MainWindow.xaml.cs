using System.Windows;
using MySudoku.ViewModel;

namespace MySudoku
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			ViewModelInitialize.InitializeViewModels(MySudokuGame);
		}
	}
}