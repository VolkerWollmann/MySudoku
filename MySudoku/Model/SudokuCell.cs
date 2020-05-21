﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySudoku.Model
{
	/// <summary>
	/// A sudoku cell is empty or holds one number.
	/// In both cases it holds the possible values for this cell.
	/// </summary>
	internal class SudokuCell
	{ 
		// parent of the cell
		SudokuGame Parent;

		public int Row { get; private set; }
		public int Column { get; private set; }

		public const string SudokuCellValueName = "SudokuCellValue";
		public const string SudokuCellPossibleValuesName = "SudokuCellPossibleValues";
		public int SudokuCellValue { private set; get; }

		public List<int> SudokuCellPossibleValues { private set; get; }


		/// <summary>
		/// The field is valid, if it is set or there is a least one possible value for the field
		/// </summary>
		public bool IsValid()
		{
			return (SudokuCellValue > 0) || (SudokuCellPossibleValues.Count() >= 1);
 		}

		public void Exclude(int valueToExclude)
		{
			if (SudokuCellValue == valueToExclude)
				return;

			SudokuCellPossibleValues.Remove(valueToExclude);
		}

		public void SetValue(int newValue)
		{
			if (!SudokuCellPossibleValues.Contains(newValue))
				return;

			SudokuCellValue = newValue;
			SudokuCellPossibleValues = new List<int> { newValue };
			Parent.Exclude(Row, Column, SudokuCellValue);
		}

		public static List<int> GetInitalPossibleValueList()
		{
			return new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
		}
		public SudokuCell(SudokuGame parent, int row, int column)
		{
			this.Parent = parent;
			Row = row;
			Column = column;

			SudokuCellPossibleValues = GetInitalPossibleValueList();
		}

		private SudokuCell(SudokuGame parent, SudokuCell original)
		{
			Parent = parent;
			Row = original.Row;
			Column = original.Column;
			SudokuCellValue = original.SudokuCellValue;
			SudokuCellPossibleValues = original.SudokuCellPossibleValues;
		}
		public void Clear()
		{
			SudokuCellValue = 0;
			SudokuCellPossibleValues = GetInitalPossibleValueList();
		}

		internal SudokuCell Copy(SudokuGame parent)
		{
			return new SudokuCell(parent, this);
		}
		
	}
}
