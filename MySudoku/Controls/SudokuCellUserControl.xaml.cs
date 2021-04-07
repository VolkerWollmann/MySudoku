using System.Collections.Generic;
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
		private static readonly SolidColorBrush ScbAntiAntiqueWhite = new SolidColorBrush(Colors.AntiqueWhite);
		private static readonly SolidColorBrush ScbWhite = new SolidColorBrush(Colors.White);

        #endregion

		private readonly SudokuBoardUserControl SudokuBoardUserControl;

		public event PropertyChangedEventHandler PropertyChanged;

		private void NotifyPropertyChanged(string info)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }

		public ICommand NumberCommand => new NumberCommand(this);

        public ICommand TogglePossibleValueSetVisibilityCommand => new TogglePossibleValueSetVisibilityMenuCommand(this);

        public int Row { get; }
		public int Column { get; }

        private string _Value;
		public string Value {
			set
			{
				_Value = value;
				NotifyPropertyChanged("Value");
			}

			get => _Value;
        }

		private string _PossibleValueSet;
		public string PossibleValueSet
		{
			set
			{
				_PossibleValueSet = value;
				NotifyPropertyChanged("PossibleValueSet");
				AdjustBackGroundColor();
			}

			get => _PossibleValueSet;
        }

		private SolidColorBrush _BackGroundColor;
		public SolidColorBrush BackGroundColor
		{
			set
			{
				_BackGroundColor = value;
				NotifyPropertyChanged("BackGroundColor");
			}

			get => _BackGroundColor;
        }

		private void AdjustBackGroundColor()
		{
			switch(_PossibleValueSet)
            {
				case SudokuConstants.EmptySet:
					Color red = new Color();
					red.A = 255;
					red.B = 150;
					red.R = 255;
					red.G = 150;
					BackGroundColor = new SolidColorBrush(red);
					break;

				case SudokuConstants.OneNumberSet:
					BackGroundColor = ScbAntiAntiqueWhite;
					break;

				default:
					BackGroundColor = ScbWhite;
					break;
			}

		}

		public void SetContextMenu(List<int> possibleValueSet)
		{
			ContextMenu contextMenu = new ContextMenu();

            MenuItem menuItemTogglePossibleValueSetVisibility = new MenuItem
            {
                Header = "Toggle possible values", Command = TogglePossibleValueSetVisibilityCommand
            };
            contextMenu.Items.Add(menuItemTogglePossibleValueSetVisibility);

			possibleValueSet.ForEach(i =>
		   {
               MenuItem menuItem = new MenuItem
               {
                   Header = i.ToString(), Command = NumberCommand, CommandParameter = i.ToString()
               };
               contextMenu.Items.Add(menuItem);
		   });

			SudokuCellControlPanel.ContextMenu = contextMenu;
		}
		public SudokuCellUserControl(SudokuBoardUserControl sudokuBoardUserControl, int row, int column) : this()
		{
			SudokuBoardUserControl = sudokuBoardUserControl;
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
			BackGroundColor = ScbAntiAntiqueWhite;
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


		private Visibility _PossibleValuesVisibility = Visibility.Hidden;
		public Visibility PossibleValuesVisibility
		{
			set
			{
				_PossibleValuesVisibility = value;
				

				NotifyPropertyChanged("PossibleValuesVisibility");
			}

			get => _PossibleValuesVisibility;
        }
      
		private void SudokuCellControlPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			SudokuBoardUserControl.MarkCell(Row, Column);
			TextBoxFocus.Focus();
		}
	}
}
