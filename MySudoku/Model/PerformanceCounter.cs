using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Schema;

namespace MySudoku.Perfomance
{

	internal class PerformanceCounterElement
	{
		public int Max { get; private set; }
		public int Current { get; private set; }
		public ulong Total { get; private set; }
		public double AverageSeconds { get; private set; }
		public DateTime Start { get; private set; }

		public void Update( int max, int current)
		{
			Max = max;
			Current = current;
			Total++;
		}

		public override string ToString()
		{
			return "Max:" + Max.ToString() + " Current:" + Current + " Total:" + Total + " AverageSeconds:" + AverageSeconds;
		}

		public void Init()
		{
			Max = 0;
			Current = 0;
			Start = DateTime.Now;
		}

		public void CalculateAverage()
		{
			if ((Current > 0) && ( Total >= (ulong)Current))
			{
				double nas = (AverageSeconds / (Total - (ulong)Current)) + DateTime.Now.Subtract(Start).TotalSeconds / Current;
				AverageSeconds = nas;
			}
		}

		public PerformanceCounterElement()
		{
			Max = 0;
			Current = 0;
			Total = 0;
			AverageSeconds = 0;
			Start = DateTime.Now;
		}
	}

	public class PerformanceCounter
	{
		private int level = 0;
		private ulong total = 0;
		private ulong opsPersecond;
		private DateTime start;

		PerformanceCounterElement[] stack;

		public PerformanceCounter()
		{
			level = 0;
			start = DateTime.Now;
			stack = new PerformanceCounterElement[81];
			for(int i = 0; i<81; i++)
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
			stack[level].CalculateAverage();
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
