using System;
using System.Collections.Generic;
using System.Linq;
using MySudoku.Interfaces;
using MySudoku.Perfomance;

namespace MySudoku.Model.BruteForce 
{
	using IntegerTriple = Tuple<int, int, int>;

	internal class Field
	{ 
		internal int Value { get; set; }
        internal int Row { get; private set; }
		public int Column { get; private set; }

		internal List<Field> Neighbors { get; private set; }

		internal bool Check( int value)
		{
			if (Value > 0)
				return (Value == value);
			
			return !Neighbors.Any( n => (n.Value == value));
		}
		

		internal Field( int row, int column)
		{
			Row = row;
			Column = column;
			Neighbors = new List<Field>();
		}

	}
	public class SudokuBruteForceGenerator : ISudokuGenerator 
	{
		Field[,] game = null;

		public int GetValue(int row, int column)
		{
			return game[row, column].Value;
		}

		public List<IntegerTriple> GetSolution()
		{
			List<IntegerTriple> list = new List<IntegerTriple>();
			for(int i=0; i<9; i++)
			{
				for( int j=0; j<9; j++)
				{
					list.Add(new IntegerTriple(i, j, game[i, j].Value));
				}
			}

			return list;
		}

		private List<Field> GetSubMartixFieldList(int sqaureRow, int squareColumn)
		{
			List<Field> result = new List<Field>();

			for (int row = sqaureRow * 3; row <= sqaureRow * 3 + 2; row++)
			{
				for (int column = squareColumn * 3; column <= squareColumn * 3 + 2; column++)
				{
					result.Add(game[row, column]);
				}
			}

			return result; 
		}

		private void PopulateSubmatrix(int sqaureRow, int squareColumn )
		{
			List<int> shuffledValues = RandomListAccess.GetShuffledList(new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 });
			int i = 0;
			for (int row = sqaureRow*3; row <= sqaureRow*3+2; row++)
			{
				for (int column = squareColumn*3; column <= squareColumn*3+2; column++)
				{
					game[row, column].Value =shuffledValues[i++];
				}
			}
		}

		private Field[] fieldsToFill;
		private PerformanceCounter performanceCounter;
		private bool Search(int index)
		{
			if (index >= fieldsToFill.Length)
				return true;

			Field currentField = fieldsToFill[index];
			for(int i = 1; i<=9; i++)
			{
				performanceCounter.Update(9, i);

				if (currentField.Check(i))
				{
					currentField.Value = i;

					performanceCounter.Down();
					bool result = Search(index + 1);
					performanceCounter.Up();
			        if (result)
						return true;

					currentField.Value = 0;
				}
			}

			return false;
		}
		public bool Generate()
		{
			for(int i =0; i<=2; i++)
				PopulateSubmatrix(i, i);

			performanceCounter = new PerformanceCounter();

			return Search(0);
		}
		public SudokuBruteForceGenerator()
		{
			game = new Field[9, 9];
			for(int row = 0; row < 9; row++)
			{
				for(int column=0; column < 9; column++)
				{
					game[row, column] = new Field(row, column);
				}
			}

			for (int row = 0; row < 9; row++)
			{
				for (int column = 0; column < 9; column++)
				{
					for(int i = 0; i<9; i++)
					{   
						if( i != row)
							game[row, column].Neighbors.Add(game[i, column]);
						if (i != column)
							game[row, column].Neighbors.Add(game[row, i]);
					}

					int rowBase = (row / 3) * 3;
					int columnBase = (column / 3) * 3;

					for (int i= rowBase; i<rowBase+2; i++ )
					{
						for(int j= columnBase; j<columnBase+2; j++)
						{
							if ((i != row) && (j != column))
								game[row, column].Neighbors.Add(game[i, j]);
						}
					}
				}
			}

			// allFields = RandomListAccess.GetShuffledList(game.Cast<Field>().Where(f => (f.Value == 0)).ToList()).ToArray();
			List<Field> sortedList = new List<Field>();
			for (int i = 0; i <= 2; i++)
			{
				for (int j = 0; j <= 2; j++)
				{
					if (i != j)
						sortedList = sortedList.Concat(GetSubMartixFieldList(i, j)).ToList();
				}
			}

			fieldsToFill = sortedList.ToArray();
		}
	}
}
