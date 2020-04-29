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
		int IntValue = 0;
		public const string SudokuCellValueName = "SudokuCellValue";
		public string SudokuCellValue
		{
			get
			{
				if (IntValue == 0)
					return "-";
				else
					return IntValue.ToString();
			}
		}

		public void SetValue(int value)
		{
			IntValue = value;
			OnPropertyChanged(SudokuCellValueName);
		}
		public SudokuCell()
		{
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
