using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace MySudoku.Interfaces
{
	public interface ISudokuGridView
	{
		UIElement GetUIElement();

		void SetKeyEventHandler(EventHandler<Key> eventHandlerKey);

		void BindValue(int row, int column, Binding binding);

		void BindPossibleValues(int row, int column, Binding binding);

		void MarkCell(int row, int column);

		void GetCurrentCellCoordiantes(out int row, out int column);
	}
}