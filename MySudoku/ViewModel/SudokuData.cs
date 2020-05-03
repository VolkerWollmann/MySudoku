using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using MySudoku.Controls;

namespace MySudoku
{
	public class SudokuData : INotifyPropertyChanged 
	{
		public string Value { private set; get; }
		public string PossibleValues { private set; get; }

		public void SetValue(int value)
		{
			if (value == 0)
				Value = "-";
			else
				Value = value.ToString();

			OnPropertyChanged("Value");
		}

		public void SetPossibleValues(List<int> possibleValues)
		{
			if (Value != "-")
				PossibleValues = "";

			string result = "{";
			for (int i = 0; i < possibleValues.Count - 1; i++)
			{
				result = result + " " + possibleValues[i] + ",";
			}
			if (possibleValues.Count > 0)
			{
				result = result + " " + possibleValues.Last();
			}

			result = result + "}";

			PossibleValues = result;
			OnPropertyChanged("PossibleValues");
		}

		public SudokuData()
		{
			Value = "";
			PossibleValues = "";
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