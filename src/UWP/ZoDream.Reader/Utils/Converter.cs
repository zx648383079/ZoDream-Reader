using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;
using ZoDream.Reader.Models;

namespace ZoDream.Reader.Utils
{
    public static class Converter
    {
        public static BitmapImage ToImg(string value)
        {
            var imageUrl = value;
            if (string.IsNullOrEmpty(imageUrl))
            {
                imageUrl = BookItem.RandomCover();
            }
            if (!imageUrl.StartsWith("http") && !imageUrl.StartsWith("ms-appx:"))
            {
                imageUrl = string.Concat("ms-appx:///", imageUrl);
            }
            return new BitmapImage(new Uri(imageUrl, UriKind.Absolute));
        }
    }
}
