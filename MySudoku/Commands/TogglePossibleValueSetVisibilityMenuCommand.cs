using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MySudoku.Controls;

namespace MySudoku.Commands
{
    public class TogglePossibleValueSetVisibilityMenuCommand : ICommand
    {
        SudokuCellUserControl SudokuCellUserControl;
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            SudokuCellUserControl.RaiseEventHandlerKey(this, Key.Space);
        }

        public TogglePossibleValueSetVisibilityMenuCommand(SudokuCellUserControl sudokuCellUserControl)
        {
            SudokuCellUserControl = sudokuCellUserControl;
        }
    }
}
