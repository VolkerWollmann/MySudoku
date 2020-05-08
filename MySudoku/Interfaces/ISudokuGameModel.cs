using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySudoku.Interfaces
{
	public interface ISudokuGameModel
	{
		int GetInvalidSudokuDigit();

		int GetCellValue(int row, int column);

		List<int> GetSudokuCellPossibleValues(int row, int column);

		void SetValue(int row, int column, int value);

		void Clear();

		void Back();
	}
}
