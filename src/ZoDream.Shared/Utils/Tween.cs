using System;
using System.Collections.Generic;
using System.Text;

namespace ZoDream.Shared.Utils
{
	public class Tween<T>
	{

		public Tween(T data, T target, double duration, Func<double, T, T, double, T> func)
		{
			InitProperties = data;
			TargetProperties = target;
			EasingFunc = func;
			InitTime = DateTime.Now;
			Duration = duration;
		}

		public T InitProperties { get; private set; }

		public T TargetProperties { get; private set; }

		public DateTime InitTime { get; private set; }

		public Func<double, T, T, double, T> EasingFunc { get; private set; }
		/// <summary>
		/// /ms
		/// </summary>
        public double Duration { get; private set; }

        public T Get()
		{
			return Get(DateTime.Now);
		}

		public T Get(double time)
		{
			return EasingFunc(time, InitProperties, TargetProperties, Duration);
		}

		public T Get(DateTime time)
		{
			return Get(time - InitTime);
		}

        public T Get(TimeSpan timeSpan)
        {
			return Get(timeSpan.Ticks / 10000);
        }

        public static double Linear(double time, double nBegin, double nEnd, double nDuration)
		{
			if (time >= nDuration)
			{
				return nEnd;
			}
			return (nEnd - nBegin) * time / nDuration + nBegin;
		}

		public static double EaseIn(double time, double nBegin, double nEnd, double nDuration)
		{
			if (time >= nDuration)
			{
				return nEnd;
			}
			return (double)((nEnd - nBegin) * Math.Pow(time / nDuration, 2) + nBegin);
		}
	}
}
