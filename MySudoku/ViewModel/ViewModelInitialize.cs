using MySudoku.Interfaces;
using MySudoku.Model;
using System.Windows.Controls;

namespace MySudoku.ViewModel
{
	public class ViewModelInitialize
	{
		private static SudokuViewModel sudokuViewModel;
		private static ISudokuGameModel sudokuGame;
		public static void InitializeViewModels(Grid grid)
		{
			sudokuGame = new SudokuGame("Game");
			sudokuViewModel = new SudokuViewModel(grid, sudokuGame);
		}
	}
}
