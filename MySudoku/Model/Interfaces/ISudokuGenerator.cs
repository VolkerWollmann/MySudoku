using System;
using System.Collections.Generic;

namespace MySudoku.Model.Interfaces
{
	using IntegerTriple = Tuple<int, int, int>;
	public interface ISudokuGenerator
	{
		List<IntegerTriple> Generate();
	}
}
