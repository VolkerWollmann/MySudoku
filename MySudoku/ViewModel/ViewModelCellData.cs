using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace MySudoku.ViewModel
{
	public class ViewModelCellData : INotifyPropertyChanged 
	{
		private const string NotSet = "-";
		public string Value { private set; get; }
		public string PossibleValues { private set; get; }

		public void SetValue(int value)
		{
			if (value == 0)
				Value = NotSet;
			else
				Value = value.ToString();

			OnPropertyChanged("Value");
		}

		public void SetPossibleValues(List<int> possibleValues)
		{
			if ((possibleValues.Count() == 1) && (Value!= NotSet))
				PossibleValues = "";
			else
			{
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
			}
			OnPropertyChanged("PossibleValues");
		}

		public ViewModelCellData()
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