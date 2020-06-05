using System;

namespace MySudoku.Perfomance
{

	internal class PerformanceCounterElement
	{
		public int Max { get; private set; }
		public int Current { get; private set; }
		public ulong Operation { get; private set; }

		public ulong Count { get; private set; }
		public double AverageSeconds { get; private set; }
		public DateTime Start { get; private set; }

		public void Update( int max, int current)
		{
			Max = max;
			Current = current;
			Operation++;
		}

		public override string ToString()
		{
			return "Max:" + Max.ToString() + " Current:" + Current + " Operation:" + Operation + " Count: " + Count + " AverageSeconds:" + AverageSeconds;
		}

		internal void Init()
		{
			Max = 0;
			Current = 0;
			Start = DateTime.Now;
		}

		internal void Complete()
		{
			if (Count > 0)
			{
				double nas = (AverageSeconds * Count + DateTime.Now.Subtract(Start).TotalSeconds) / (Count+1) ;
				AverageSeconds = nas;
			}
			Count++;
		}

		public PerformanceCounterElement()
		{
			Max = 0;
			Current = 0;
			Operation = 0;
			AverageSeconds = 0;
			Start = DateTime.Now;
		}
	}

	public class PerformanceCounter
	{
		private const int StackSize = 82;

		private int level = 0;
		private ulong total = 0;
		private ulong opsPersecond;
		private DateTime start;

		PerformanceCounterElement[] stack;

		public PerformanceCounter()
		{
			level = 0;
			start = DateTime.Now;
			stack = new PerformanceCounterElement[StackSize];
			for(int i = 0; i< StackSize; i++)
			{
				stack[i] = new PerformanceCounterElement();
			}
		}

		public void Down()
		{
			level = level + 1;
			stack[level].Init();
		}

		public void Up()
		{
			stack[level].Complete();
			level = level - 1;
		}

		public void Update( int max, int current)
		{
			total++;
			stack[level].Update(max, current);

			ulong seconds = (ulong)DateTime.Now.Subtract(start).TotalSeconds;
			if (seconds >= 1)
			{
				opsPersecond = total / seconds;
			}
		}
	}
}
