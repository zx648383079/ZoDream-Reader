using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media.Imaging;
using System;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Storage.Streams;
using ZoDream.Reader.ViewModels;

namespace ZoDream.Reader.Converters
{
    public static class ConverterHelper
    {
        public static string Format(DateTime date)
        {
            if (date == DateTime.MinValue)
            {
                return "-";
            }
            return date.ToString("yyyy-MM-dd HH:mm");
        }

        public static string FormatSize(long size)
        {
            return SizeConverter.FormatSize(size);
        }


        public static string FormatHour(int value)
        {
            if (value <= 0)
            {
                return "00:00";
            }
            var m = value / 60;
            if (m >= 60)
            {
                return (m / 60).ToString("00") + ":"
                    + (m % 60).ToString("00") + ":" + (value % 60).ToString("00");
            }
            return m.ToString("00") + ":" + (value % 60).ToString("00");
        }

        public static Visibility VisibleIf(bool val)
        {
            return val ? Visibility.Visible : Visibility.Collapsed;
        }

        public static Visibility VisibleIf(string val)
        {
            return VisibleIf(!string.IsNullOrWhiteSpace(val));
        }
        public static Visibility CollapsedIf(bool val)
        {
            return VisibleIf(!val);
        }


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

        public static bool IsVolume(IEditableSection data)
        {
            if (data is VolumeItemViewModel)
            {
                return true;
            }
            if (data is ChapterItemViewModel o)
            {
                return o.Items.Count == 0;
            }
            return false;
        }
    }
}
