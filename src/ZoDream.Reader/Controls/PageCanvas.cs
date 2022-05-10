using System;
using System.Collections.Generic;
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
using System.Windows.Threading;
using ZoDream.Shared.Controls;
using ZoDream.Shared.Events;
using ZoDream.Shared.Interfaces;
using ZoDream.Shared.Models;
using ZoDream.Shared.Utils;

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
    ///     <MyNamespace:PageCanvas/>
    ///
    /// </summary>
    [TemplatePart(Name = LayerPanelName, Type = typeof(Canvas))]
    public class PageCanvas : Control, ICanvasRender
    {
        public const string LayerPanelName = "PART_LayerPanel";
        static PageCanvas()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PageCanvas), new FrameworkPropertyMetadata(typeof(PageCanvas)));
        }

        private Canvas? LayerPanel;
        private Point LastMousePoint = new();
        private int CurrentPage = -1;
        private Action? OnSwapFinished;
        /// <summary>
        /// 翻页动画方向
        /// </summary>
        private bool? SwapDirect;
        private DispatcherTimer? SwapTimer;
        private Tween<float>? SwapTween;
        private List<PageLayer> LayerItems = new();
        public ICanvasSource? Source { get; set; }
        public bool IsReady { get; private set; } = false;

        public event PageChangedEventHandler? PageChanged;
        public event CanvasReadyEventHandler? OnReady;


        public AppOption Setting
        {
            get { return (AppOption)GetValue(SettingProperty); }
            set { SetValue(SettingProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Setting.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SettingProperty =
            DependencyProperty.Register("Setting", typeof(AppOption), typeof(PageCanvas), 
                new PropertyMetadata(null, (d, e) =>
                {
                    (d as PageCanvas)?.UpdateSetting();
                }));


        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            LayerPanel = GetTemplateChild(LayerPanelName) as Canvas;
        }


        /// <summary>
        /// 使用过渡动画切换到新的页面，下一页
        /// </summary>
        /// <param name="pages"></param>
        public void SwapTo(IList<PageItem> pages)
        {
            SwapTo(pages, 0);
        }


        public void SwapTo(IList<PageItem> pages, int page)
        {
            LayerItems[0] = LayerItems[1];
            LayerItems[1] = CreateLayer(pages, page);
            SwapAnimate(true, () =>
            {
                SwapDirect = null;
                CurrentPage = page;
                PageChanged?.Invoke(this, page, pages[0].Begin);
            });
        }

        public async Task SwapTo(int page)
        {
            if (Source == null || !Source.Canable(page))
            {
                return;
            }
            SwapTo(await Source.GetAsync(page), page);
        }

        /// <summary>
        /// 使用过渡动画切换回新的页面，上一页
        /// </summary>
        /// <param name="pages"></param>
        public void SwapFrom(IList<PageItem> pages)
        {
            SwapFrom(pages, 0);
        }

        public void SwapFrom(IList<PageItem> pages, int page)
        {
            LayerItems[2] = LayerItems[1];
            LayerItems[1] = CreateLayer(pages, page);
            SwapAnimate(false, () =>
            {
                SwapDirect = null;
                CurrentPage = page;
                PageChanged?.Invoke(this, page, pages[0].Begin);
            });
        }
        public async void SwapFrom(int page)
        {
            if (Source == null || !Source.Canable(page))
            {
                return;
            }
            SwapFrom(await Source.GetAsync(page), page);
        }

        private void SwapAnimate(bool toNext, Action finished)
        {
            SwapDirect = toNext;
            OnSwapFinished = finished;
            if (Setting.Animation < 1)
            {
                SwapTimer_Tick(null, null);
                return;
            }
            if (SwapTimer == null)
            {
                SwapTimer = new DispatcherTimer()
                {
                    Interval = TimeSpan.FromMilliseconds(15),
                };
                SwapTimer.Tick += SwapTimer_Tick;
            }
            SwapTimer.Start();
            SwapTween = new Tween<float>(0f, (float)ActualWidth, 1000, Tween<float>.EaseIn);
        }

        private void SwapTimer_Tick(object? sender, object? e)
        {
            if (IsSwapFinished())
            {
                OnSwapFinished?.Invoke();
                SwapDirect = null;
                SwapTimer?.Stop();
                foreach (var item in LayerItems)
                {
                    item?.EndSwap();
                }
            }
            if (swapDirect == true)
            {
                LayerItems[0].MoveSwap(-SwapTween.Get());
            }
            else if (SwapDirect == false)
            {
                LayerItems[1].MoveSwap(SwapTween.Get());
            }

        }

        private bool IsSwapFinished()
        {
            if (Setting.Animation < 1 || SwapDirect == null)
            {
                return true;
            }
            if (SwapDirect == true)
            {
                return LayerItems[0] == null || LayerItems[0].X <= 10 - LayerItems[0].Width;
            }
            return LayerItems[1] == null || LayerItems[1].X <= -10;
        }

        public async void SwapNext()
        {
            await SwapTo(CurrentPage + 1);
        }

        public async void SwapPrevious()
        {
            await SwapTo(CurrentPage - 1);
        }

        public void Flush()
        {

        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            LastMousePoint = e.GetPosition(this);
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);
            var p = e.GetPosition(this);
            var diff = p - LastMousePoint;
            if (diff.Length < 5)
            {
                // TODO 点击
                if (p.X < ActualWidth / 3)
                {
                    SwapPrevious();
                }
                else if (p.X > ActualWidth * .7)
                {
                    SwapNext();
                }
                return;
            }

            if (Math.Abs(diff.X) > Math.Abs(diff.Y))
            {
                if (diff.X > 0)
                {
                    SwapPrevious();
                }
                else if (diff.X < 0)
                {
                    SwapNext();
                }
            }
        }


        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.Key == Key.Right || e.Key == Key.PageDown)
            {
                SwapNext();
                return;
            }
            if (e.Key == Key.Left || e.Key == Key.PageUp)
            {
                SwapPrevious();
                return;
            }
        }

        private PageLayer CreateLayer(IList<PageItem> pages, int page)
        {
            var layer = new PageLayer()
            {
                Content = pages,
                Page = page,
                FontFamily = FontFamily,
                Background = Background,
                Foreground = Foreground,
                FontSize = FontSize,
            };
            return layer;
        }

        public void Draw(IList<PageItem> pages)
        {
            
        }

        private void UpdateSetting()
        {
            if (Setting == null)
            {
                return;
            }
            Background = string.IsNullOrWhiteSpace(Setting.BackgroundImage) ?
                ColorHelper.ColorToBrush(Setting.Background) : ColorHelper.ImageToBrush(Setting.BackgroundImage);
            if (!string.IsNullOrWhiteSpace(Setting.FontFamily))
            {
                FontFamily = new FontFamily(Setting.FontFamily);
            }
            FontSize = Setting.FontSize;
            Foreground = ColorHelper.ColorToBrush(Setting.Foreground);
            foreach (var item in LayerItems)
            {
                item.FontFamily = FontFamily;
                item.FontSize = FontSize;
                item.Background = item.Background;
                item.Foreground = item.Foreground;
            }
        }
    }
}
