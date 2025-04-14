using System;
using System.Globalization;
using System.Numerics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ZoDream.Shared.Interfaces;
using ZoDream.Shared.Interfaces.Tokenizers;

namespace ZoDream.Reader.Controls
{
    /// <summary>
    /// 按照步骤 1a 或 1b 操作，然后执行步骤 2 以在 XAML 文件中使用此自定义控件。
    ///
    /// 步骤 1a) 在当前项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根
    /// 元素中:
    ///
    ///     xmlns:MyNamespace="clr-namespace:ZoDream.Reader.Controls"
    ///
    ///
    /// 步骤 1b) 在其他项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根
    /// 元素中:
    ///
    ///     xmlns:MyNamespace="clr-namespace:ZoDream.Reader.Controls;assembly=ZoDream.Reader.Controls"
    ///
    /// 您还需要添加一个从 XAML 文件所在的项目到此项目的项目引用，
    /// 并重新生成以避免编译错误:
    ///
    ///     在解决方案资源管理器中右击目标项目，然后依次单击
    ///     “添加引用”->“项目”->[浏览查找并选择此项目]
    ///
    ///
    /// 步骤 2)
    /// 继续操作并在 XAML 文件中使用控件。
    ///
    ///     <MyNamespace:PageLayer/>
    ///
    /// </summary>
    public class PageLayer : Control, ICanvasLayer
    {
        static PageLayer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PageLayer), new FrameworkPropertyMetadata(typeof(PageLayer)));
        }

        public int Page { get; set; }

        public Vector2 Position => new((float)Canvas.GetLeft(this), (float)Canvas.GetTop(this));

        public Vector2 Size => new((float)ActualWidth, (float)ActualHeight);

        private INovelPage? _data;

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            drawingContext.DrawRectangle(Background, new Pen(BorderBrush, BorderThickness.Top), new Rect(
                0, 0, ActualWidth, ActualHeight));
            if (_data is null)
            {
                return;
            }
            var baseFont = new Typeface(FontFamily, FontStyle, FontWeight, FontStretch);
            FormattedText formatted;
            foreach (var page in _data)
            {
                var font = baseFont;
                if (!string.IsNullOrWhiteSpace(page.FontFamily))
                {
                    font = new Typeface(FontFamily, 
                        page.FontItalic ? FontStyles.Italic : FontStyle, 
                        page.FontWeight <= 0 ? FontWeight : FontWeight.FromOpenTypeWeight(page.FontWeight),
                        FontStretch);
                }
                var fontSize = page.FontSize > 0 ? page.FontSize : FontSize;
                foreach (var item in page)
                {
                    if (item is INovelPageChar c)
                    {
                        formatted = new FormattedText(c.Text, CultureInfo.CurrentCulture,
                        FlowDirection.LeftToRight, font, fontSize, Foreground, 1.25);
                        drawingContext.DrawText(formatted, new Point(page.Position.X + item.Position.X, 
                            page.Position.Y + item.Position.Y));
                    }
                    if (item is INovelPageImage i)
                    {
                        drawingContext.DrawImage(
                            new BitmapImage(new Uri(i.Source, UriKind.Absolute))
                            , new Rect(page.Position.X + item.Position.X, 
                            page.Position.Y + item.Position.Y, item.Size.X, item.Size.Y));
                    }
                }
            }
        }


        public void Clear()
        {
            // Content = null;
        }

        public void Move(double x, double y)
        {
            Canvas.SetLeft(this, x);
            Canvas.SetTop(this, y);
        }

        public void Resize(double x, double y, double width, double height)
        {
            Height = height;
            Width = width;
            Move(x, y);
        }

        public void Toggle(bool visible)
        {
            if (visible == IsVisible)
            {
                return;
            }
            Visibility = visible ? Visibility.Visible : Visibility.Collapsed;
        }

        public void DrawText(INovelPage data)
        {
            Canvas.SetLeft(this, data.Position.X);
            Canvas.SetTop(this, data.Position.Y);
            _data = data;
            InvalidateVisual();
        }

        public void Dispose()
        {
            
        }

        public void Move(Vector2 point)
        {
            throw new NotImplementedException();
        }

        public void Resize(Vector4 bound)
        {
            throw new NotImplementedException();
        }
    }
}
