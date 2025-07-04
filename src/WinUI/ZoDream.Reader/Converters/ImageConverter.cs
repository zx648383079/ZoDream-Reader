﻿using Microsoft.UI.Xaml.Data;
using System;

namespace ZoDream.Reader.Converters
{
    public class ImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return Utils.Converter.ToImg((string)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
