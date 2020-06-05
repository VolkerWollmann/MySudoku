using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MySudoku.Interfaces
{
	interface ISudokuCommands
	{
		UIElement GetUIElement();

		void SetClearCommandEventHandler(EventHandler eventHandler);

		void SetNewCommandEventHandler(EventHandler eventHandler);

		void SetBackCommandEventHandler(EventHandler eventHandler);

		void SetSolveCommandEventHandler(EventHandler eventHandler);

		bool SetButtonsEnabled(bool buttonsEnabled);

		int GetNumberOfCellsToFill();
	}
}
