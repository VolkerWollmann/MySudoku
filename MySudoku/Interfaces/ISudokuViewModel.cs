using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace MySudoku.Interfaces
{
	public interface ISudokuViewModel
	{
		UIElement GetUIElement();

		void SetKeyEventHandler(EventHandler<Key> eventHandlerKey);

		void MarkCell(int row, int column);

		void SetCell(int row, int column);

		void SetValue(int row, int coulumn,string value);

		void SetPossibleValueSet(int row, int column, string possibleValuesSet); 

		void GetCurrentCellCoordiantes(out int row, out int column);
	}
}