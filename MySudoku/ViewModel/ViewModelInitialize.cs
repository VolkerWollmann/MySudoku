using MySudoku.Interfaces;
using MySudoku.Model;
using System.Windows.Controls;

namespace MySudoku.ViewModel
{
	public class ViewModelInitialize
	{
		private static SudokuViewModel _sudokuViewModel;
		private static ISudokuGameModel _sudokuGame;
		public static void InitializeViewModels(Grid grid)
		{
			_sudokuGame = new SudokuGame("Game");
			_sudokuViewModel = new SudokuViewModel(grid, _sudokuGame);
		}
	}
}
