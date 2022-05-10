using System;
using System.Collections.Generic;
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
    public class PageLayer : Control
    {
        static PageLayer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PageLayer), new FrameworkPropertyMetadata(typeof(PageLayer)));
        }

        public int Page { get; set; }

        public IEnumerable<PageItem> Content
        {
            get { return (IEnumerable<PageItem>)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Content.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register("Content", typeof(IEnumerable<PageItem>), 
                typeof(PageLayer), new PropertyMetadata(null, (d, e) =>
                {
                    (d as PageLayer)?.InvalidateVisual();
                }));

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            if (Content == null)
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
    }
}
