using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySudoku.Constants
{
    public class SudokuConstants
    {
        public const string EMPTY_SET = "{}";
        public const string ONE_NUMBER_SET = "";
    }

    public enum SudokuCommand
    {
        Clear = 1,
        Back = 2,
        New = 3,
        Solve = 4,
        TogglePossibleValues =5,
    }

}
