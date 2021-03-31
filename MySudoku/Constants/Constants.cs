namespace MySudoku.Constants
{
    public class SudokuConstants
    {
        public const string EmptySet = "{}";
        public const string OneNumberSet = "";
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
