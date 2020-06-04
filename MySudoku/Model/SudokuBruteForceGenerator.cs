using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
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

		internal List<Field> Neighbours { get; private set; }

		internal bool Check( int value)
		{
			if (Value > 0)
				return (Value == value);
			
			return !Neighbours.Any( n => (n.Value == value));
		}
		

		internal Field( int row, int column)
		{
			Row = row;
			Column = column;
			Neighbours = new List<Field>();
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
			
			game.Cast<Field>().ToList().ForEach(field => { list.Add(new IntegerTriple(field.Row, field.Column, field.Value));  });

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

		private bool Search(List<Field> fieldsToFill)
		{
			if (!fieldsToFill.Any())
				return true;

			Field currentField = fieldsToFill.First();

			for(int i = 1; i<=9; i++)
			{
				if (currentField.Check(i))
				{
					currentField.Value = i;

					bool result = Search((new List<Field>(fieldsToFill).GetRange(1, fieldsToFill.Count - 1)));
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

			List<Field> unpolutatedFields = new List<Field>();

			game.Cast<Field>().Where(field => (field.Value == 0)).ToList().ForEach(field => { unpolutatedFields.Add(field); });

			return Search(unpolutatedFields);
		}
		public SudokuBruteForceGenerator()
		{
			game = new Field[9, 9];
			for (int row = 0; row < 9; row++)
			{
				for(int column=0; column < 9; column++)
				{
					game[row, column] = new Field(row, column);
				}
			}

			game.Cast<Field>().ToList().ForEach( f =>
			{
				// Row neighbours
				IEnumerable<Field> rowNeighbours = game.Cast<Field>().Where(field => (field.Row == f.Row && field.Column != f.Column));
				f.Neighbours.AddRange(rowNeighbours.ToList());

				// Column neighbours
				IEnumerable<Field> columnNeighbours = game.Cast<Field>().Where(field => (field.Row != f.Row && field.Column == f.Column));
				f.Neighbours.AddRange(columnNeighbours.ToList());

				// Square neighbours
				int rowBase = (f.Row / 3) * 3;
				int columnBase = (f.Column / 3) * 3;

				IEnumerable<Field> l = game.Cast<Field>().Where(field => (field.Row >= rowBase) && (field.Row <= rowBase + 2) && (field.Column >= columnBase) && (field.Column <= columnBase + 2));
				l = l.Where(field => (field.Row != f.Row && field.Column != f.Column));
				l = l.Where(field => (!f.Neighbours.Contains(field)));
				f.Neighbours.AddRange(l);
			});
			
		}
	}
}
