using System.Collections.Generic;

namespace MySudoku.Interfaces
{
	public interface ISudokuGameModel
	{
		int GetInvalidDigit();

		int GetCellValue(int row, int column);

		List<int> GetCellPossibleValues(int row, int column);

		bool SetValue(int row, int column, int value);

		void Clear();

		void New(int numberOfCellsToFill);

		bool Solve();

		void Back();

		bool GetLastOperation(out int x, out int y, out int value);
	}
}
