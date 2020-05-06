using MySudoku.Controls;
using MySudoku.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MySudoku.ViewModel
{
	public class GameCommandViewModel
	{
		// Grid from program
		private Grid sudokuGrid;

		// The game grid from the model
		private ISudokuGameModel sudokuGame;

		// Command Control
		ISudokuCommands sudokuCommand;

		public GameCommandViewModel(Grid _sudokuGrid, ISudokuGameModel _sudokuGame )
		{
			sudokuGrid = _sudokuGrid;
			sudokuGame = _sudokuGame;
			sudokuCommand = new SudokuCommandUserControl();

			sudokuGrid.Children.Add(sudokuCommand.GetUIElement());
			Grid.SetRow(sudokuCommand.GetUIElement(), 0);
			Grid.SetColumn(sudokuCommand.GetUIElement(), 1);

			sudokuCommand.SetClearCommandEventHandler(ClearCommand);
		}

		private void ClearCommand(object sender, EventArgs e)
		{
			sudokuGame.Clear();
		}
	}
}
