using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySudoku
{
	public class SudokuCell : INotifyPropertyChanged
	{
		SudokuGrid grid;

		public int Row { get; private set; }
		public int Column { get; private set; }

		int value = 0;
		List<int> possibleValues;


		public const string SudokuCellValueName = "SudokuCellValue";
		public const string SudokuCellPossibleValuesName = "SudokuCellPossibleValues";
		public string SudokuCellValue
		{
			get
			{
				if (value == 0)
					return "-";
				else
					return value.ToString();
			}
		}

		public string SudokuCellPossibleValues
		{
			get
			{
				if (value > 0)
					return "";

				string result = "{";
				for( int i=0; i< possibleValues.Count -1; i++ )
				{
					result = result + " " + possibleValues[i] + ",";
				}
				if ( possibleValues.Count > 0 )
				{
					result = result + " " + possibleValues.Last();
				}

				result = result + "}";

				return result;
			}
		}

		public void Exclude(int valueToExclude)
		{
			if (value == valueToExclude)
				return;

			possibleValues.Remove(valueToExclude);
			OnPropertyChanged(SudokuCellPossibleValuesName);
		}

		public void SetValue(int newValue)
		{
			if (!possibleValues.Contains(newValue))
				return;

			value = newValue;
			possibleValues = new List<int> { newValue };
			OnPropertyChanged(SudokuCellValueName);
			OnPropertyChanged(SudokuCellPossibleValuesName);
			grid.Exclude(Row, Column, value);
		}

		public SudokuCell(SudokuGrid parent, int myRow, int myColumn)
		{
			grid = parent;
			Row = myRow;
			Column = myColumn;

			possibleValues = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChanged(string info)
		{
			PropertyChangedEventHandler handler = PropertyChanged;
			if (handler != null)
			{
				handler(this, new PropertyChangedEventArgs(info));
			}
		}


	}
}
