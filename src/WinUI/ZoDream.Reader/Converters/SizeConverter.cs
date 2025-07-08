using Microsoft.UI.Xaml.Data;
using System;
using System.Text.RegularExpressions;

namespace ZoDream.Reader.Converters
{
    public class SizeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
            {
                return "0B";
            }
            return FormatSize((long)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }

        public static string FormatSize(long size)
        {
            var len = size.ToString().Length;
            if (len < 4)
            {
                return $"{size}B";
            }
            if (len < 7)
            {
                return Math.Round(System.Convert.ToDouble(size / 1024d), 2) + "KB";
            }
            if (len < 10)
            {
                return Math.Round(System.Convert.ToDouble(size / 1024d / 1024), 2) + "MB";
            }
            if (len < 13)
            {
                return Math.Round(System.Convert.ToDouble(size / 1024d / 1024 / 1024), 2) + "GB";
            }
            if (len < 16)
            {
                return Math.Round(System.Convert.ToDouble(size / 1024d / 1024 / 1024 / 1024), 2) + "TB";
            }
            return Math.Round(System.Convert.ToDouble(size / 1024d / 1024 / 1024 / 1024 / 1024), 2) + "PB";
        }

        public static long Parse(string text, string defUnit = "B")
        {
            text = text.ToUpper();
            var match = Regex.Match(text, @"([\d\.]+)\s*([PTGMKB]?)");
            if (!match.Success)
            {
                return 0;
            }
            if (!float.TryParse(match.Groups[1].Value, out var v) || v <= 0)
            {
                return 0;
            }
            var unit = string.IsNullOrWhiteSpace(match.Groups[2].Value) ? defUnit : match.Groups[2].Value;
            return unit switch
            {
                "P" => (long)(v * Math.Pow(1024, 5)),
                "T" => (long)(v * Math.Pow(1024, 4)),
                "G" => (long)(v * Math.Pow(1024, 3)),
                "M" => (long)(v * Math.Pow(1024, 2)),
                "K" => (long)(v * 1024),
                _ => (long)v,
            };
        }
    }
}
