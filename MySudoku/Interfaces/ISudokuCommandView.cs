using MySudoku.Constants;
using System;
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
