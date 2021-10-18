﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ZoDream.Shared.Utils
{
	public class Tween<T>
	{

		public Tween(T data, T target, float duration, Func<float, T, T, float, T> func)
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

		public Func<float, T, T, float, T> EasingFunc { get; private set; }

        public float Duration { get; private set; }

        public T Get()
		{
			return Get(DateTime.Now);
		}

		public T Get(float time)
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

        public static float Linear(float time, float nBegin, float nEnd, float nDuration)
		{
			if (time >= nDuration)
			{
				return nEnd;
			}
			return (nEnd - nBegin) * time / nDuration + nBegin;
		}

		public static float EaseIn(float time, float nBegin, float nEnd, float nDuration)
		{
			if (time >= nDuration)
			{
				return nEnd;
			}
			return (float)((nEnd - nBegin) * Math.Pow(time / nDuration, 2) + nBegin);
		}
	}
}
