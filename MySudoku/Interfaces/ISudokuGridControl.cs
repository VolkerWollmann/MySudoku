using System.Windows;
using System.Windows.Data;

namespace MySudoku.Interfaces
{
	public interface ISudokuGridControl
	{
		UIElement GetUIElement();

		void BindValue(int row, int column, Binding binding);

		void BindPossibleValues(int row, int column, Binding binding);

		void MarkCell(int row, int column);

		void GetCurrentCellCoordiantes(out int row, out int column);
	}
}