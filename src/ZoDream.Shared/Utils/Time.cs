using System;
using System.Collections.Generic;
using System.Text;

namespace ZoDream.Shared.Utils
{
    public static class Time
    {
        /// <summary>
        /// 格式化时间
        /// </summary>
        /// <param name="date"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string Format(DateTime date, string format)
        {
            return date.ToString(format);
        }

        public static string Format(int timestamp, string format)
        {
            return Format(TimestampTo(timestamp), format);
        }

        /// <summary>
        /// 格式化时间
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string Format(DateTime date)
        {
            return Format(date, "yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// 格式化当前时间
        /// </summary>
        /// <returns></returns>
        public static string Format()
        {
            return Format(DateTime.Now);
        }

        /// <summary>
        /// 格式化时间戳
        /// </summary>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        public static string Format(int timestamp)
        {
            return Format(TimestampTo(timestamp));
        }

        /// <summary>
        /// 从时间戳转时间
        /// </summary>
        /// <param name="timestamp">秒</param>
        /// <returns></returns>
        public static DateTime TimestampTo(int timestamp)
        {
            var dateTimeStart = TimeZoneInfo.ConvertTimeToUtc(new DateTime(1970, 1, 1, 0, 0, 0, 0));
            var lTime = long.Parse(timestamp + "0000000");
            var toNow = new TimeSpan(lTime);
            return dateTimeStart.Add(toNow);
        }

        public static DateTime TimestampTo(string str)
        {
            if (DateTime.TryParse(str, out DateTime date))
            {
                return date;
            }
            return DateTime.MinValue;
        }

        /// <summary>
        /// 从时间转时间戳，精确到秒
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static int TimestampFrom(DateTime time)
        {
            var dateTimeStart = TimeZoneInfo.ConvertTimeToUtc(new DateTime(1970, 1, 1, 0, 0, 0, 0));
            return Convert.ToInt32((time.Ticks - dateTimeStart.Ticks) / 10000000);
        }

        public static int TimestampFrom(string str)
        {
            if (DateTime.TryParse(str, out DateTime date))
            {
                return TimestampFrom(date);
            }
            return 0;
        }

        public static int TimestampFrom(int time)
        {
            if (time.ToString().Length > 10)
            {
                return time / 1000;
            }
            return time;
        }

        /// <summary>
        /// 格式化多久之前
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string FormatAgo(DateTime date)
        {
            return FormatAgo(date, DateTime.Now);
        }

        /// <summary>
        /// 格式化多久之前
        /// </summary>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        public static string FormatAgo(int timestamp)
        {
            return FormatAgo(TimestampTo(timestamp));
        }

        public static string FormatAgo(int timestamp, int now)
        {
            return FormatAgo(TimestampTo(timestamp), TimestampTo(now));
        }

        public static string FormatAgo(DateTime date, DateTime now)
        {
            var diff = now - date;
            if (diff.TotalSeconds < 1)
            {
                return "刚刚";
            }
            if (diff.TotalSeconds < 60)
            {
                return $"{Convert.ToInt32(diff.TotalSeconds)}秒前";
            }
            if (diff.TotalMinutes < 60)
            {
                return $"{Convert.ToInt32(diff.TotalMinutes)}分钟前";
            }
            if (diff.TotalHours < 24)
            {
                return date.Day == now.Day ? $"{Convert.ToInt32(diff.TotalHours)}小时前" : date.ToString("MM-dd");
            }
            if (date.Year == now.Year)
            {
                return date.ToString("MM-dd");
            }
            return date.ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// 秒的差额
        /// </summary>
        /// <param name="time"></param>
        /// <param name="now"></param>
        /// <returns></returns>
        public static int SecondDiffer(int time, int now)
        {
            return Math.Abs(time - now);
        }

        public static int SecondDiffer(int time, DateTime now)
        {
            return SecondDiffer(TimestampTo(time), now);
        }

        public static int SecondDiffer(string time, DateTime now)
        {
            return SecondDiffer(TimestampTo(time), now);
        }

        public static int SecondDiffer(DateTime time, DateTime now)
        {
            var diff = now - time;
            return Convert.ToInt32(Math.Abs(diff.TotalSeconds));
        }
    }
}
