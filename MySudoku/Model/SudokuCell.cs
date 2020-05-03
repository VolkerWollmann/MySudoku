using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySudoku
{
	public class SudokuCell
	{ 
		public const int InvalidSudokuDigit = -1;

		SudokuGrid grid;

		public int Row { get; private set; }
		public int Column { get; private set; }

		int value = 0;
		List<int> possibleValues;


		public const string SudokuCellValueName = "SudokuCellValue";
		public const string SudokuCellPossibleValuesName = "SudokuCellPossibleValues";
		public int SudokuCellValue
		{
			get
			{
				return value;
			}
		}

		public List<int> SudokuCellPossibleValues
		{
			get
			{
				return possibleValues;
			}
		}

		public void Exclude(int valueToExclude)
		{
			if (value == valueToExclude)
				return;

			possibleValues.Remove(valueToExclude);
		}

		public void SetValue(int newValue)
		{
			if (!possibleValues.Contains(newValue))
				return;

			value = newValue;
			possibleValues = new List<int> { newValue };
			grid.Exclude(Row, Column, value);
		}

		public SudokuCell(SudokuGrid parent, int myRow, int myColumn)
		{
			grid = parent;
			Row = myRow;
			Column = myColumn;


			possibleValues = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
		}

		public void Clear()
		{
			value = 0;
			possibleValues = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
		}
		
	}
}
