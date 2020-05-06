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
	}
}
