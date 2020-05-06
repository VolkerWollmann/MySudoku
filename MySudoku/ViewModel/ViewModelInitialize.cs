using MySudoku.Interfaces;
using MySudoku.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MySudoku.ViewModel
{
	public class ViewModelInitialize
	{
		private static GameGridViewModel gameGridViewModel;
		private static ISudokuGameModel sudokuGame;
		public static void InitializeViewModels(Grid grid)
		{
			sudokuGame = new SudokuGame();
			gameGridViewModel = new GameGridViewModel(grid, sudokuGame);
		}
	}
}
