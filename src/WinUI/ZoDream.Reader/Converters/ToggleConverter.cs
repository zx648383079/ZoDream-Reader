﻿using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using System;
using System.Linq;

namespace ZoDream.Reader.Converters
{
    public class ToggleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return IsVisible(value, parameter) ? Visibility.Visible : Visibility.Collapsed;
        }

        private static bool IsVisible(object value, object parameter)
        {
            if (value is null)
            {
                return false;
            }
            if (parameter is null)
            {
                if (value is bool o)
                {
                    return o;
                }
                if (value is int i)
                {
                    return i > 0;
                }
                return string.IsNullOrWhiteSpace(value.ToString());
            }
            if (parameter is bool b)
            {
                return (bool)value == b;
            }
            var pStr = parameter.ToString();
            var vStr = value.ToString();
            if (pStr == vStr)
            {
                return true;
            }
            if (vStr is null || pStr is null)
            {
                return false;
            }
            var isRevert = false;
            if (pStr.StartsWith('^'))
            {
                isRevert = true;
                pStr = pStr[1..];
            }
            return pStr.Split('|').Contains(vStr) == !isRevert;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
