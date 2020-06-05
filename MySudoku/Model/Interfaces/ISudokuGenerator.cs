using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySudoku.Interfaces
{
	using IntegerTriple = Tuple<int, int, int>;
	public interface ISudokuGenerator
	{
		List<IntegerTriple> Generate();
	}
}
