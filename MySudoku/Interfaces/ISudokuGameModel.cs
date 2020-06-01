using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

		void Back();
	}
}
