using System;
using System.Collections.Generic;

namespace MySudoku.Model
{
	internal class RandomListAccess
	{
		private static Random random = new Random();
		internal static T GetRandomElement<T>(List<T> list)
		{
			int index = random.Next(0, list.Count);
			return list[index];
		}

		internal static List<T> GetShuffledList<T>(List<T> list)
		{
			List<T> result = new List<T>();
			List<T> work = new List<T>();
			list.ForEach(e => work.Add(e));
			while (work.Count > 0)
			{
				int index = random.Next(0, work.Count);
				result.Add(work[index]);
				work.RemoveAt(index);
			}

			return result;
		}
	}
}
