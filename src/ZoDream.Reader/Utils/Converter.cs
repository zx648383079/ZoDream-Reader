using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ZoDream.Reader.Utils
{
    public static class Converter
    {
        public static BitmapImage ToImg(string? value)
        {
            var imageUrl = value;
            if (string.IsNullOrEmpty(imageUrl))
            {
                imageUrl = RandomCover();
            }
            if (!imageUrl.StartsWith("http") && !imageUrl.StartsWith("pack:"))
            {
                imageUrl = string.Concat("pack://application:,,,/", imageUrl);
            }
            return new BitmapImage(new Uri(imageUrl, UriKind.Absolute));
        }

        public static string RandomCover()
        {
            var rd = new Random();
            return $"Assets/cover{rd.Next(1, 11)}.jpg";
        }
    }
}
