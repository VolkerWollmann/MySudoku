using System.Collections.Generic;
using System.Linq;

namespace MySudoku.Model
{
	/// <summary>
	/// A sudoku cell is empty or holds one number.
	/// In both cases it holds the possible values for this cell.
	/// </summary>
	internal class SudokuCell
	{ 
		// parent of the cell
        readonly SudokuGame Parent;

		public int Row { get; }
		public int Column { get; }

		public const string SudokuCellValueName = "CellValue";
		public const string SudokuCellPossibleValuesName = "CellPossibleValues";
		public int CellValue { private set; get; }

		public List<int> CellPossibleValues { private set; get; }


		/// <summary>
		/// The field is valid, if it is set or there is a least one possible value for the field
		/// </summary>
		public bool IsValid()
		{
			return ((CellValue > 0 ) || ( CellPossibleValues.Any()));
 		}

		public bool IsEqual(SudokuCell other)
		{
			if ((Row != other.Row) || (Column != other.Column) || (CellValue != other.CellValue) ||
				(CellPossibleValues.Count != other.CellPossibleValues.Count))
				return false;

			foreach( int value in CellPossibleValues )
			{
				if (!other.CellPossibleValues.Contains(value))
					return false;
			}

			return true;
		}

		public void Exclude(int valueToExclude)
		{
			if (CellValue > 0)
				return;

			CellPossibleValues.Remove(valueToExclude);
		}

		public bool SetValue(int newValue)
		{
			if (!CellPossibleValues.Contains(newValue))
				return false;

			CellValue = newValue;
			CellPossibleValues = new List<int> { newValue };
			Parent.Exclude(Row, Column, CellValue);

			return true;
		}

		public static List<int> GetInitialPossibleValueList()
		{
			return new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
		}
		public SudokuCell(SudokuGame parent, int row, int column)
		{
			Parent = parent;
			Row = row;
			Column = column;

			CellPossibleValues = GetInitialPossibleValueList();
		}

		private SudokuCell(SudokuGame parent, SudokuCell original)
		{
			Parent = parent;
			Row = original.Row;
			Column = original.Column;
			CellValue = original.CellValue;
			CellPossibleValues = new List<int>(original.CellPossibleValues);
		}
		public void Clear()
		{
			CellValue = 0;
			CellPossibleValues = GetInitialPossibleValueList();
		}

		internal SudokuCell Copy(SudokuGame parent)
		{
			return new SudokuCell(parent, this);
		}
		
	}
}
