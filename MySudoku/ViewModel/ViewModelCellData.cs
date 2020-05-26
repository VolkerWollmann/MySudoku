using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace MySudoku.ViewModel
{
	public class ViewModelCellData
	{
		private const string NotSet = "-";
		public string Value { private set; get; }
		public string PossibleValueSet { private set; get; }

		public void SetValue(int value)
		{
			if (value == 0)
				Value = NotSet;
			else
				Value = value.ToString();

		}

		public void SetPossibleValueSet(List<int> possibleValues)
		{

			if ((possibleValues.Count() == 1))
				PossibleValueSet = "";
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

				PossibleValueSet = result;
			}
		}



			public ViewModelCellData()
		{
			Value = NotSet;
			PossibleValueSet = "---";
		}
	}
}