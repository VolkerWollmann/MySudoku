using System;
using System.Collections.Generic;
using System.Linq;
using MySudoku.Model.Interfaces;
using MySudoku.Model.Support;

namespace MySudoku.Model.BruteForce 
{
	using IntegerTriple = Tuple<int, int, int>;

	internal class Field
	{ 
		internal int Value { get; set; }
        internal int Row { get; }
		public int Column { get; }

		internal List<Field> Neighbours { get; }

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
        readonly Field[,] _Game;

		public int GetValue(int row, int column)
		{
			return _Game[row, column].Value;
		}

		private List<Field> GetSubMatrixFieldList(int squareRow, int squareColumn)
		{
			List<Field> result = new List<Field>();

			for (int row = squareRow * 3; row <= squareRow * 3 + 2; row++)
			{
				for (int column = squareColumn * 3; column <= squareColumn * 3 + 2; column++)
				{
					result.Add(_Game[row, column]);
				}
			}

			return result; 
		}

		private void PopulateSubmatrix(int squareRow, int squareColumn )
		{
			List<int> shuffledValues = RandomListAccess.GetShuffledList(new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 });
			int i = 0;
			for (int row = squareRow*3; row <= squareRow*3+2; row++)
			{
				for (int column = squareColumn*3; column <= squareColumn*3+2; column++)
				{
					_Game[row, column].Value =shuffledValues[i++];
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
		public List<IntegerTriple> Generate()
		{
			for(int i =0; i<=2; i++)
				PopulateSubmatrix(i, i);

			List<Field> fieldsToFill = new List<Field>();

			_Game.Cast<Field>().Where(field => (field.Value == 0)).ToList().ForEach(field => { fieldsToFill.Add(field); });

			bool result = Search(fieldsToFill);
			if (!result)
				return null;
		
			List<IntegerTriple> list = new List<IntegerTriple>();
			_Game.Cast<Field>().ToList().ForEach(field => { list.Add(new IntegerTriple(field.Row, field.Column, field.Value)); });
			return list;
		}
		public SudokuBruteForceGenerator()
		{
			_Game = new Field[9, 9];
			for (int row = 0; row < 9; row++)
			{
				for(int column=0; column < 9; column++)
				{
					_Game[row, column] = new Field(row, column);
				}
			}

			_Game.Cast<Field>().ToList().ForEach( f =>
			{
				// Row neighbours
				IEnumerable<Field> rowNeighbours = _Game.Cast<Field>().Where(field => (field.Row == f.Row && field.Column != f.Column));
				f.Neighbours.AddRange(rowNeighbours.ToList());

				// Column neighbours
				IEnumerable<Field> columnNeighbours = _Game.Cast<Field>().Where(field => (field.Row != f.Row && field.Column == f.Column));
				f.Neighbours.AddRange(columnNeighbours.ToList());

				// Square neighbours
				int rowBase = (f.Row / 3) * 3;
				int columnBase = (f.Column / 3) * 3;

				IEnumerable<Field> l = _Game.Cast<Field>().Where(field => (field.Row >= rowBase) && (field.Row <= rowBase + 2) && (field.Column >= columnBase) && (field.Column <= columnBase + 2));
				l = l.Where(field => (field.Row != f.Row && field.Column != f.Column));
				l = l.Where(field => (!f.Neighbours.Contains(field)));
				f.Neighbours.AddRange(l);
			});
			
		}
	}
}
