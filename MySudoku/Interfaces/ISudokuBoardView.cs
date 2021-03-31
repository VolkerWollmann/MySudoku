using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace MySudoku.Interfaces
{
	public interface ISudokuBoardView
	{
		UIElement GetUIElement();

		// EventHandler for responding on arrow and number keys pressed on the board
		void SetKeyEventHandler(EventHandler<Key> eventHandlerKey);

		void MarkCell(int row, int column);

		void SetValue(int row, int column,string value);

		void SetPossibleValueSetString(int row, int column, string possibleValuesSet);

		bool PossibleValueSetVisibility { get; set; }

		void SetPossibleValueContextMenu(int row, int column, List<int> possibleValueSet);

		void GetCurrentCellCoordinates(out int row, out int column);
	}
}