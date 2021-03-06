﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using MySudoku.Controls;

namespace MySudoku.Commands
{
	public class NumberCommand : ICommand
	{
        readonly SudokuCellUserControl SudokuCellUserControl;
		public event EventHandler CanExecuteChanged;

		public bool CanExecute(object parameter)
		{
			return true;
		}

		public void Execute(object parameter)
		{
			Key key = Key.D0;
			if (parameter is string)
			{
				List<Tuple<string, Key>> mapping = new List<Tuple<string, Key>>()
				{
					new Tuple<string, Key>( "1", Key.D1 ),
					new Tuple<string, Key>( "2", Key.D2 ),
					new Tuple<string, Key>( "3", Key.D3 ),
					new Tuple<string, Key>( "4", Key.D4 ),
					new Tuple<string, Key>( "5", Key.D5 ),
					new Tuple<string, Key>( "6", Key.D6 ),
					new Tuple<string, Key>( "7", Key.D7 ),
					new Tuple<string, Key>( "8", Key.D8 ),
					new Tuple<string, Key>( "9", Key.D9 ),
				};
				key = mapping.First(e => (e.Item1 == (string)parameter)).Item2;
			}
			else if (parameter is Key key1)
				key = key1;

			SudokuCellUserControl.RaiseEventHandlerKey(this, key);
		}

		public NumberCommand(SudokuCellUserControl sudokuCellUserControl)
		{
			SudokuCellUserControl = sudokuCellUserControl;
		}
	}
}
