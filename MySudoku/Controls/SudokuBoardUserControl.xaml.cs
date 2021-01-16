﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MySudoku.Interfaces;
namespace MySudoku.Controls
{
	/// <summary>
	/// Interaction logic for SudokuBoardUserControl.xaml
	/// </summary>
	public partial class SudokuBoardUserControl : UserControl, ISudokuBoardView
	{
		SudokuCellUserControl[,] SudokuCellUserControlGrid;
		public SudokuCellUserControl CurrentSudokuCellUserControl { get; private set; } = null;

		private int[,] iValues= new int[9,9];

		public EventHandler<Key> EventHandlerKey;

		public SudokuBoardUserControl()
		{
			InitializeComponent();

			SudokuCellUserControlGrid = new SudokuCellUserControl[9, 9];

			for (int row = 0; row < 9; row++)
			{
				for (int column = 0; column < 9; column++)
				{		
					SudokuCellUserControl sudokuCellUserControl = new SudokuCellUserControl(this, row, column);
					SudokuCellUserControlGrid[row, column] = sudokuCellUserControl;

					SudokuBoard.Children.Add(sudokuCellUserControl);
					Grid.SetRow(sudokuCellUserControl, row);
					Grid.SetColumn(sudokuCellUserControl, column);

				}
			}
		}

		public UIElement GetUIElement()
		{
			return this as UIElement;
		}

		public void GetCurrentCellCoordiantes(out int row, out int column)
		{
			row = -1;
			column = -1;

			if (CurrentSudokuCellUserControl != null)
			{
				row = CurrentSudokuCellUserControl.Row;
				column = CurrentSudokuCellUserControl.Column;
			}
		}

		void ISudokuBoardView.SetKeyEventHandler(EventHandler<Key> eventHandlerKey)
		{
			EventHandlerKey += eventHandlerKey;
		}

		public void SetValue(int row, int column,string value)
		{
			SudokuCellUserControlGrid[row, column].Value = value;
		}

		public void SetPossibleValueSetString(int row, int column, string possibleValueSet)
		{
			// #decouple threads : changing the user interface must be done by the GUI-Thread 
			this.Dispatcher.Invoke(() =>
			{
				SudokuCellUserControlGrid[row, column].PossibleValueSet = possibleValueSet;
			});
		}

		public void SetPossibleValueContextMenu(int row, int column, List<int> possibleValueSet)
		{
			// #decouple threads : changing the user interface must be done by the GUI-Thread 
			this.Dispatcher.Invoke(() =>
			{
				SudokuCellUserControlGrid[row, column].SetContextMenu(possibleValueSet);
			});
		}

		public void MarkCell(int row, int column)
		{
			if (CurrentSudokuCellUserControl != null)
				CurrentSudokuCellUserControl.UnMark();

			CurrentSudokuCellUserControl = SudokuCellUserControlGrid[row, column];

			CurrentSudokuCellUserControl.Mark();

		}

		private bool _possibleValueSetVisibilty = false; //start invisible

		public bool PossibleValueSetVisibilty
        {
			set
            {
				_possibleValueSetVisibilty = value;
				SudokuCellUserControlGrid.Cast<SudokuCellUserControl>().ToList().ForEach( cell => { cell.PossibleValuesVisibilty = value ? Visibility.Visible : Visibility.Hidden; });
			}

			get
            {
				return _possibleValueSetVisibilty;

			}
        }
    }
}
