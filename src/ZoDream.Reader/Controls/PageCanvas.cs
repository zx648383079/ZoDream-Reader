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
using ZoDream.Shared.Animations;
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

        public PageCanvas()
        {
            Loaded += PageCanvas_Loaded;
            Unloaded += PageCanvas_Unloaded;
        }

        private Canvas? LayerPanel;
        private Point LastMousePoint = new();
        private int CurrentPage = -1;
        private Action? OnSwapFinished;
        /// <summary>
        /// 翻页动画方向
        /// </summary>
        private ICanvasAnimate Animate = new NoneAnimate();
        private Tween<double>? SwapTween;
        private bool IsTouchMove = false;
        private readonly List<ICanvasLayer> LayerItems = new();
        public ICanvasSource? Source { get; set; }

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
            InitLayer();
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            foreach (var item in LayerItems)
            {
                item.Resize(0, 0, ActualWidth, ActualHeight);
            }
        }

        private void PageCanvas_Unloaded(object sender, RoutedEventArgs e)
        {
            CompositionTarget.Rendering -= CompositionTarget_Rendering;
        }

        private void PageCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            // Timer.Start();
            CompositionTarget.Rendering += CompositionTarget_Rendering;
            OnReady?.Invoke(this);
        }

        private void CompositionTarget_Rendering(object? sender, EventArgs e)
        {
            if (SwapTween == null)
            {
                return;
            }
            var isEnd = Animate.Animate(LayerItems, SwapTween.Get(), false);
            if (!isEnd)
            {
                return;
            }
            SwapFinished();
        }

        private void SwapFinished()
        {
            SwapTween = null;
            OnSwapFinished?.Invoke();
            //var layer = LayerItems[1] as PageLayer;
            //layer.Background = ColorHelper.ColorToBrush("red");
            //layer.Foreground = ColorHelper.ColorToBrush("#fff");
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
            SetPage(0, 1);
            SetPage(1, page, pages);
            SwapAnimate(true, () =>
            {
                CurrentPage = page;
                PageChanged?.Invoke(this, page, pages[0].Begin);
            });
        }

        public async Task SwapToAsync(int page)
        {
            if (Source == null || !Source.Enable(page))
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

        #region 设置layer的数据
        private void SetPage(ICanvasLayer dist, ICanvasLayer source)
        {
            SetPage(dist, source.Page, source.Content);
        }

        private void SetPage(int dist, int page, IList<PageItem> items)
        {
            if (dist < 0 || dist >= LayerItems.Count)
            {
                return;
            }
            SetPage(LayerItems[dist], page, items);
        }

        private void SetPage(ICanvasLayer dist, int page, IList<PageItem> items)
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                dist.Page = page;
                if (items != null)
                {
                    dist.Add(items);
                } else
                {
                    dist.Clear();
                }
            });
        }

        private void SetPage(int dist, int source)
        {
            if (dist < 0 || dist >= LayerItems.Count || source < 0 || source >= LayerItems.Count)
            {
                return;
            }
            SetPage(LayerItems[dist], LayerItems[source]);
        }
        #endregion

        public void SwapFrom(IList<PageItem> pages, int page)
        {
            SetPage(2, 1);
            SetPage(1, page, pages);
            SwapAnimate(false, () =>
            {
                CurrentPage = page;
                PageChanged?.Invoke(this, page, pages[0].Begin);
            });
        }
        public async Task SwapFromAsync(int page)
        {
            if (Source == null || !Source.Enable(page))
            {
                return;
            }
            SwapFrom(await Source.GetAsync(page), page);
        }

        private void SwapAnimate(bool toNext, Action finished)
        {
            OnSwapFinished = finished;
            SwapTween = new Tween<double>(.0, toNext ? 100 : -100, 500, Tween<double>.EaseIn);
        }


        public async Task SwapNextAsync()
        {
            await SwapToAsync(CurrentPage + 1);
        }

        public async Task SwapPreviousAsync()
        {
            await SwapFromAsync(CurrentPage - 1);
        }

        public void Flush()
        {

        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            IsTouchMove = false;
            LastMousePoint = e.GetPosition(this);
            Animate.MoveStart(LayerItems[1], LastMousePoint.X, LastMousePoint.Y);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (e.LeftButton != MouseButtonState.Pressed)
            {
                return;
            }
            IsTouchMove = true;
            var p = e.GetPosition(this);
            var offset = Animate.Move(LayerItems[1], p.X, p.Y);
            Animate.Animate(LayerItems, offset, true);
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);
            var p = e.GetPosition(this);
            if (!IsTouchMove)
            {
                // TODO 点击
                if (p.X < ActualWidth / 3)
                {
                    _ = SwapPreviousAsync();
                }
                else if (p.X > ActualWidth * .7)
                {
                    _ = SwapNextAsync();
                }
                return;
            }
            var offset = Animate.Move(LayerItems[1], p.X, p.Y);
            if (Math.Abs(offset) < 30)
            {
                SwapTween = new Tween<double>(offset, 0, 300, Tween<double>.EaseIn);
                return;
            }
            if (offset > 0)
            {
                _ = SwapNextAsync();
            } else
            {
                _ = SwapPreviousAsync();
            }
        }


        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.Key == Key.Right || e.Key == Key.PageDown)
            {
                _ = SwapNextAsync();
                return;
            }
            if (e.Key == Key.Left || e.Key == Key.PageUp)
            {
                _ = SwapPreviousAsync();
                return;
            }
        }


        private void InitLayer()
        {
            if (LayerPanel == null)
            {
                return;
            }
            LayerItems.Clear();
            LayerPanel.Children.Clear();
            for (int i = 0; i < 3; i++)
            {
                var layer = new PageLayer
                {
                    FontFamily = FontFamily,
                    FontSize = FontSize,
                    Background = Background,
                    Foreground = Foreground
                };
                LayerItems.Add(layer);
                LayerPanel.Children.Add(layer);
                layer.Resize(0, 0, ActualWidth, ActualHeight);
                Panel.SetZIndex(layer, i + 1);
            }
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
            foreach (PageLayer item in LayerItems)
            {
                item.FontFamily = FontFamily;
                item.FontSize = FontSize;
                item.Background = Background;
                item.Foreground = Foreground;
            }
            Animate = Setting.Animation switch
            {
                1 => new CoverAnimate(),
                2 => new VerticalCoverAnimate(),
                3 => new SimulateAnimate(),
                4 => new ScrollAnimate(),
                _ => new NoneAnimate(),
            };
        }
    }
}
