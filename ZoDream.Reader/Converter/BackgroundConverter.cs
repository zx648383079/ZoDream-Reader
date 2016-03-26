using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ZoDream.Reader.Converter
{
    public class BackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var val = (string) value;
            if (string.IsNullOrEmpty(val) || val[0] == '#' || val.IndexOf(',') > 0)
            {
                return value;
            }
            return new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri(val))
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
