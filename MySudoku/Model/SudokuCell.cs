﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySudoku
{
	public class SudokuCell : INotifyPropertyChanged
	{
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


		public void SetValue(int newValue)
		{
			value = newValue;
			possibleValues = new List<int> { newValue };
			OnPropertyChanged(SudokuCellValueName);
			OnPropertyChanged(SudokuCellPossibleValuesName);
		}

		public SudokuCell()
		{
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
