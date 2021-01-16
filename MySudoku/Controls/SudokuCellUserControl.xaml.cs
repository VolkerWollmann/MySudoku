﻿using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using MySudoku.Constants;
using MySudoku.Commands;

namespace MySudoku.Controls
{
	/// <summary>
	/// Interaction logic for SudokuCellUserControl.xaml
	/// </summary>
	public partial class SudokuCellUserControl : UserControl, INotifyPropertyChanged
	{
		#region Constants
		private static SolidColorBrush SCBAntiAntiqueWhite = new SolidColorBrush(Colors.AntiqueWhite);
		private static SolidColorBrush SCBWhite = new SolidColorBrush(Colors.White);
		private static SolidColorBrush SCBLightGreen = new SolidColorBrush(Colors.LightGreen);

		#endregion

		private SudokuBoardUserControl SudokuBoardUserControl;

		public event PropertyChangedEventHandler PropertyChanged;

		private void NotifyPropertyChanged(string info)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(info));
			}
		}

		public ICommand NumberCommand
		{
			get
			{
				return new NumberCommand(this);
			}
		}

		public ICommand TogglePossibleValueSetVisibiltyCommand
        {
			get
            {
				return new TogglePossibleValueSetVisibilityMenuCommand(this);
			}
        }

		public int Row { get; private set; }
		public int Column { get; private set; }

		string _value;
		public string Value {
			set
			{
				_value = value;
				NotifyPropertyChanged("Value");
			}

			get
			{
				return _value;
			}
		}

		string _possibleValueSet;
		public string PossibleValueSet
		{
			set
			{
				_possibleValueSet = value;
				NotifyPropertyChanged("PossibleValueSet");
				AdjustBackGroundColor();
			}

			get
			{
				return _possibleValueSet;
			}
		}

		SolidColorBrush _backGroundColor;
		public SolidColorBrush BackGroundColor
		{
			set
			{
				_backGroundColor = value;
				NotifyPropertyChanged("BackGroundColor");
			}

			get
			{
				return _backGroundColor;
			}
		}

		private void AdjustBackGroundColor()
		{
			switch(_possibleValueSet)
            {
				case SudokuConstants.EMPTY_SET:
					Color red = new Color();
					red.A = 255;
					red.B = 150;
					red.R = 255;
					red.G = 150;
					BackGroundColor = new SolidColorBrush(red);
					break;

				case SudokuConstants.ONE_NUMBER_SET:
					BackGroundColor = SCBAntiAntiqueWhite;
					break;

				default:
					BackGroundColor = SCBWhite;
					break;
			}

		}

		public void SetContextMenu(List<int> possibleValueSet)
		{
			ContextMenu contextMenu = new ContextMenu();

			MenuItem menuItemToggelPossibleValueSetVisibilty = new MenuItem();
			menuItemToggelPossibleValueSetVisibilty.Header = "Toggle possible values";
			menuItemToggelPossibleValueSetVisibilty.Command = TogglePossibleValueSetVisibiltyCommand;
			contextMenu.Items.Add(menuItemToggelPossibleValueSetVisibilty);

			possibleValueSet.ForEach(i =>
		   {
			   MenuItem menuItem = new MenuItem();
			   menuItem.Header = i.ToString();
			   menuItem.Command = NumberCommand;
			   menuItem.CommandParameter = i.ToString();
			   contextMenu.Items.Add(menuItem);
		   });

			SudokuCellControlPanel.ContextMenu = contextMenu;
		}
		public SudokuCellUserControl(SudokuBoardUserControl _sudokuBoardUserControl, int row, int column) : this()
		{
			SudokuBoardUserControl = _sudokuBoardUserControl;
			Row = row;
			Column = column;

			// Set border
			SudokuCellControlBorder.BorderThickness = new Thickness
			{
				Left = 1,
				Right = (Column == 2) || (Column == 5) ? 3 : 1,
				Bottom = (Row == 2) || (Row == 5) ? 3 : 1,
				Top = 1
			};

			SudokuCellControlBorder.BorderBrush = new SolidColorBrush(Colors.Black);
			BackGroundColor = SCBAntiAntiqueWhite;
		}

		public SudokuCellUserControl()
		{
			InitializeComponent();
			this.DataContext = this;

			Value = "-";
			PossibleValueSet = "---";

			SudokuCellControlPanel.MouseLeftButtonDown += SudokuCellControlPanel_MouseLeftButtonDown;
			SudokuCellControlPanel.MouseRightButtonDown += SudokuCellControlPanel_MouseLeftButtonDown;

			this.KeyUp += SudokuCellUserControl_KeyUp;
		}

		internal void RaiseEventHandlerKey(object sender, Key key)
		{
			SudokuBoardUserControl.EventHandlerKey(sender, key);
		}
		private void SudokuCellUserControl_KeyUp(object sender, KeyEventArgs e)
		{
			// Binding to number key does not work
			NumberCommand.Execute(e.Key);
		}

		public void Mark()
		{
			BackGroundColor = new SolidColorBrush(Colors.LightGreen);
			TextBoxFocus.Focus();
		}

		public void UnMark()
		{
			AdjustBackGroundColor();
		}


		private Visibility _possibleValuesVisibilty = Visibility.Hidden;
		public Visibility PossibleValuesVisibilty
		{
			set
			{
				_possibleValuesVisibilty = value;
				

				NotifyPropertyChanged("PossibleValuesVisibilty");
			}

			get
			{
				return _possibleValuesVisibilty;
			}
		}
      
		private void SudokuCellControlPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			SudokuBoardUserControl.MarkCell(Row, Column);
			TextBoxFocus.Focus();
		}
	}
}
