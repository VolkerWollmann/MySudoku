using System.Collections.Generic;
using System.Linq;
using MySudoku.Constants;

namespace MySudoku.ViewModel
{
	public class CellViewModel
	{
		private const string NotSet = "-";
		public string Value { private set; get; }
		public string PossibleValuesSetString { private set; get; }

		private List<int> possibleValuesSet;
		public List<int> PossibleValuesSet
		{
			private set => possibleValuesSet = value;
            get
			{
				if (Value == NotSet)
					return possibleValuesSet;
				else
					return new List<int>();
			}
		}

		public int _value;
		public void SetValue(int value)
		{
			_value = value;
			if (value == 0)
				Value = NotSet;
			else
				Value = value.ToString();
		}

		public void SetPossibleValuesSet(List<int> possibleValues)
		{
			PossibleValuesSet = possibleValues;

			if (_value != 0)
				PossibleValuesSetString = SudokuConstants.OneNumberSet;
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

				PossibleValuesSetString = result;
			}
		}

		public CellViewModel()
		{
			Value = NotSet;
			_value = 0;
			PossibleValuesSetString = "---";
			PossibleValuesSet = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
		}
	}
}