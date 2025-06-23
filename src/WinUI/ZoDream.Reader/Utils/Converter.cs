using Microsoft.UI.Xaml.Media.Imaging;
using System;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Storage.Streams;

namespace ZoDream.Reader.Utils
{
    public static class Converter
    {
        public static BitmapImage ToImg(string value)
        {
            var imageUrl = value;
            if (string.IsNullOrEmpty(imageUrl))
            {
                imageUrl = RandomCover();
            }
            if (!imageUrl.StartsWith("http") && !imageUrl.StartsWith("ms-appx:"))
            {
                if (imageUrl.EndsWith('='))
                {
                    var bi = new BitmapImage();
                    using var stream = new InMemoryRandomAccessStream();
                    stream.WriteAsync(Convert.FromBase64String(imageUrl).AsBuffer()).GetAwaiter().GetResult();
                    try
                    {
                        bi.SetSourceAsync(stream).GetAwaiter().GetResult();
                        return bi;
                    }
                    catch (Exception)
                    {
                        imageUrl = RandomCover();
                    }
                }
                imageUrl = string.Concat("ms-appx:///", imageUrl);
            }
            return new BitmapImage(new Uri(imageUrl, UriKind.Absolute));
        }

        public static BitmapImage ToImg(Stream value)
        {
            var bi = new BitmapImage();
            bi.SetSourceAsync(value.AsRandomAccessStream()).GetAwaiter().GetResult();
            return bi;
        }

        public static string RandomCover()
        {
            var rd = new Random();
            return $"Assets/cover{rd.Next(1, 11)}.jpg";
        }
    }
}
