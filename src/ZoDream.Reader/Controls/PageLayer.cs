using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ZoDream.Shared.Interfaces;
using ZoDream.Shared.Models;

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

        public double Left => Canvas.GetLeft(this);
        public double Top => Canvas.GetTop(this);


        public int Page { get; set; }

        public IList<PageItem> Content
        {
            get { return (IList<PageItem>)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Content.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register("Content", typeof(IList<PageItem>), 
                typeof(PageLayer), new PropertyMetadata(null, (d, e) =>
                {
                    (d as PageLayer)?.InvalidateVisual();
                }));

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            drawingContext.DrawRectangle(Background, new Pen(BorderBrush, BorderThickness.Top), new Rect(
                0, 0, ActualWidth, ActualHeight));
            if (Content is null)
            {
                return;
            }
            var font = new Typeface(FontFamily, FontStyle, FontWeight, FontStretch);
            foreach (var page in Content)
            {
                foreach (var item in page)
                {
                    var format = new FormattedText(item.Code.ToString(), CultureInfo.CurrentCulture,
                        FlowDirection.LeftToRight, font, FontSize, Foreground, 1.25);
                    drawingContext.DrawText(format, new Point(item.X, item.Y));
                }
            }
        }

        public void Add(IEnumerable<CharItem> items)
        {
            Add(new List<PageItem>
            {
                new PageItem() { Data = items.ToList() }
            });
        }

        public void Add(IEnumerable<PageItem> items)
        {
            Content = items.ToList();
        }

        public void Clear()
        {
            Content = null;
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

        public void Dispose()
        {
            
        }
    }
}
