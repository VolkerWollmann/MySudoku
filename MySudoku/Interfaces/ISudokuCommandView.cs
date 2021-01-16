using MySudoku.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MySudoku.Interfaces
{
	interface ISudokuCommandView
	{
		UIElement GetUIElement();

		void SetCommandEventHandler(SudokuCommand sudokuCommand, EventHandler eventHandler);

		bool SetButtonsEnabled(bool buttonsEnabled);

		int GetNumberOfCellsToFill();
	}
}
