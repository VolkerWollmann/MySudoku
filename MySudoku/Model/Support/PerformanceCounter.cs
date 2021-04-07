using System;

namespace MySudoku.Model.Support
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

		private int Level;
        private readonly DateTime Start;

        readonly PerformanceCounterElement[] Stack;

		public PerformanceCounter()
		{
			Level = 0;
			Start = DateTime.Now;
			Stack = new PerformanceCounterElement[StackSize];
			for(int i = 0; i< StackSize; i++)
			{
				Stack[i] = new PerformanceCounterElement();
			}
		}

		public void Down()
		{
			Level = Level + 1;
			Stack[Level].Init();
		}

		public void Up()
		{
			Stack[Level].Complete();
			Level = Level - 1;
		}

		public void Update( int max, int current)
		{
            Stack[Level].Update(max, current);

			ulong seconds = (ulong)DateTime.Now.Subtract(Start).TotalSeconds;
			if (seconds >= 1)
			{
            }
		}
	}
}
