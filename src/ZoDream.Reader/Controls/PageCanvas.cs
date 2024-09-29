using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
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
        private readonly List<ICanvasLayer> LayerItems = [];
        public ICanvasSource? Source { get; set; }

        public event PageChangedEventHandler? PageChanged;
        public event CanvasReadyEventHandler? OnReady;


        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            LayerPanel = GetTemplateChild(LayerPanelName) as Canvas;
            // InitLayer();
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            Animate.Resize(ActualWidth, ActualHeight);
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
            Animate.Ready(this);
        }

        private void CompositionTarget_Rendering(object? sender, EventArgs e)
        {
            if (SwapTween == null)
            {
                return;
            }
            //var isEnd = Animate.Animate(LayerItems, SwapTween.Get(), false);
            //if (!isEnd)
            //{
            //    return;
            //}
            //SwapFinished();
        }



        #region 设置layer的数据
        //private void SetPage(ICanvasLayer dist, ICanvasLayer source)
        //{
        //    SetPage(dist, source.Page, source.Content);
        //}

        //private void SetPage(int dist, int page, IList<PageItem> items)
        //{
        //    if (dist < 0 || dist >= LayerItems.Count)
        //    {
        //        return;
        //    }
        //    SetPage(LayerItems[dist], page, items);
        //}

        //private void SetPage(ICanvasLayer dist, int page, IList<PageItem> items)
        //{
        //    App.Current.Dispatcher.Invoke(() =>
        //    {
        //        dist.Page = page;
        //        if (items != null)
        //        {
        //            dist.Add(items);
        //        } else
        //        {
        //            dist.Clear();
        //        }
        //    });
        //}

        //private void SetPage(int dist, int source)
        //{
        //    if (dist < 0 || dist >= LayerItems.Count || source < 0 || source >= LayerItems.Count)
        //    {
        //        return;
        //    }
        //    SetPage(LayerItems[dist], LayerItems[source]);
        //}
        #endregion


        private void SwapAnimate(bool toNext, Action finished)
        {
            OnSwapFinished = finished;
            SwapTween = new Tween<double>(.0, toNext ? 100 : -100, 500, Tween<double>.EaseIn);
        }


        public void Flush()
        {

        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            IsTouchMove = false;
            LastMousePoint = e.GetPosition(this);
            Animate.OnTouchStart(LastMousePoint.X, LastMousePoint.Y);
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
            Animate.OnTouchMove(p.X, p.Y);
            // Animate.Animate(LayerItems, offset, true);
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
                    Animate.TurnPrevious();
                }
                else if (p.X > ActualWidth * .7)
                {
                    Animate.TurnNext();
                }
                return;
            }
            Animate.OnTouchMove(p.X, p.Y);
            //if (Math.Abs(offset) < 30)
            //{
            //    SwapTween = new Tween<double>(offset, 0, 300, Tween<double>.EaseIn);
            //    return;
            //}
            //if (offset > 0)
            //{
            //    _ = SwapNextAsync();
            //} else
            //{
            //    _ = SwapPreviousAsync();
            //}
        }


        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.Key == Key.Right || e.Key == Key.PageDown)
            {
                Animate.TurnNext();
                return;
            }
            if (e.Key == Key.Left || e.Key == Key.PageUp)
            {
                Animate.TurnPrevious();
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
            //if (Setting == null)
            //{
            //    return;
            //}
            //Background = string.IsNullOrWhiteSpace(Setting.BackgroundImage) ?
            //    ColorHelper.ColorToBrush(Setting.Background) : ColorHelper.ImageToBrush(Setting.BackgroundImage);
            //if (!string.IsNullOrWhiteSpace(Setting.FontFamily))
            //{
            //    FontFamily = new FontFamily(Setting.FontFamily);
            //}
            //FontSize = Setting.FontSize;
            //Foreground = ColorHelper.ColorToBrush(Setting.Foreground);
            //foreach (PageLayer item in LayerItems)
            //{
            //    item.FontFamily = FontFamily;
            //    item.FontSize = FontSize;
            //    item.Background = Background;
            //    item.Foreground = Foreground;
            //}
            //Animate = Setting.Animation switch
            //{
            //    1 => new CoverAnimate(),
            //    2 => new VerticalCoverAnimate(),
            //    3 => new SimulateAnimate(),
            //    4 => new ScrollAnimate(),
            //    _ => new NoneAnimate(),
            //};
        }

        public void SetAnimate(ICanvasAnimate animate)
        {
            Animate = animate;
            animate.Ready(this);
        }

        public ICanvasLayer CreateLayer(double width, double height)
        {
            return new PageLayer
            {
                Width = width,
                Height = height
            };
        }

        public void Invalidate()
        {
            Animate.Invalidate();
            Animate.OnDraw(this);
        }

        public void DrawLayer(ICanvasLayer layer)
        {
            if (layer is null)
            {
                return;
            }
            var l = (FrameworkElement)layer;
            Canvas.SetZIndex(l, 1);
            if (LayerPanel!.Children.Contains(l))
            {
                return;
            }
            LayerPanel.Children.Add(l);
        }

        public async Task ReloadAsync()
        {
            if (Source is null)
            {
                return;
            }
            await Source.InvalidateAsync();
            Invalidate();
        }
    }
}
