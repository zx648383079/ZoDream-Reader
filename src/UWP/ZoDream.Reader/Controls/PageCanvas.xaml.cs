using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Text;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.System;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using ZoDream.Reader.Drawing;
using ZoDream.Reader.Events;
using ZoDream.Shared.Animations;
using ZoDream.Shared.Events;
using ZoDream.Shared.Interfaces;
using ZoDream.Shared.Models;
using ZoDream.Shared.Utils;
using ColorHelper = ZoDream.Reader.Drawing.ColorHelper;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace ZoDream.Reader.Controls
{
    public sealed partial class PageCanvas : UserControl, ICanvasRender
    {
        public PageCanvas()
        {
            this.InitializeComponent();
        }

        internal List<ICanvasLayer> LayerItems = new List<ICanvasLayer>();
        private CanvasTextFormat CacheFont;
        private Color CacheForeground;
        private Color CacheBackground;
        private CanvasBitmap CacheImage;
        private DispatcherTimer Timer = new DispatcherTimer();

        private int CurrentPage = -1;
        private ICanvasAnimate Animate = new NoneAnimate();
        private Tween<double> SwapTween;
        private bool IsTouchMove = false;
        private bool IsPointerPressed = false;
        private Action OnSwapFinished;
        private Point LastMousePoint = new Point();
        public bool IsReady { get; private set; } = false;
        public ICanvasSource Source { get; set; }

        public event PageChangedEventHandler PageChanged;
        public event CanvasReadyEventHandler OnReady;



        public AppOption Setting
        {
            get { return (AppOption)GetValue(SettingProperty); }
            set { SetValue(SettingProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Setting.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SettingProperty =
            DependencyProperty.Register("Setting", typeof(AppOption), typeof(PageCanvas), new PropertyMetadata(null, OnSettingChanged));

        private static void OnSettingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as PageCanvas).UpdateSetting();
        }


        private void DrawerCanvas_Draw(Microsoft.Graphics.Canvas.UI.Xaml.CanvasControl sender, Microsoft.Graphics.Canvas.UI.Xaml.CanvasDrawEventArgs args)
        {
            foreach (PageLayer item in LayerItems)
            {
                item.Draw(args.DrawingSession);
            }
        }

        private void DrawerCanvas_CreateResources(Microsoft.Graphics.Canvas.UI.Xaml.CanvasControl sender, Microsoft.Graphics.Canvas.UI.CanvasCreateResourcesEventArgs args)
        {
            InitLayer();
            IsReady = true;
            OnReady?.Invoke(this);
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
            dist.Page = page;
            if (items != null)
            {
                dist.Add(items);
            }
            else
            {
                dist.Clear();
            }
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


        public void Invalidate()
        {
            DrawerCanvas.Invalidate();
        }

        public void Flush()
        {
            DrawerCanvas.Invalidate();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Timer.Interval = TimeSpan.FromMilliseconds(50);
            Timer.Start();
            Timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object sender, object e)
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

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            Timer.Stop();
            Animate?.Dispose();
            foreach (var item in LayerItems)
            {
                item?.Dispose();
            }
            DrawerCanvas.RemoveFromVisualTree();
            DrawerCanvas = null;
        }

        private void UserControl_ManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            TouchMoveStart(e.Position);
        }

        private void UserControl_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            TouchMoveEnd(e.Position);
        }

        private void TouchMoveStart(Point p)
        {
            IsTouchMove = false;
            LastMousePoint = p;
            Animate.MoveStart(LayerItems[1], LastMousePoint.X, LastMousePoint.Y);
        }

        private void TouchMove(Point p)
        {
            IsTouchMove = true;
            var offset = Animate.Move(LayerItems[1], p.X, p.Y);
            Animate.Animate(LayerItems, offset, true);
        }

        private void TouchMoveEnd(Point p)
        {
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
            }
            else
            {
                _ = SwapPreviousAsync();
            }
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            foreach (var item in LayerItems)
            {
                item.Resize(0, 0, ActualWidth, ActualHeight);
            }
        }

        private void UserControl_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            IsPointerPressed = true;
            TouchMoveStart(e.GetCurrentPoint(this).Position);
        }

        private void UserControl_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            IsPointerPressed = false;
            TouchMoveEnd(e.GetCurrentPoint(this).Position);
        }

        private void UserControl_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Right || e.Key == VirtualKey.PageDown)
            {
                _ = SwapNextAsync();
                return;
            }
            if (e.Key == VirtualKey.Left || e.Key == VirtualKey.PageUp)
            {
                _ = SwapPreviousAsync();
                return;
            }
        }

        private void UserControl_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            TouchMove(e.Position);
        }

        private void UserControl_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            if (IsPointerPressed)
            {
                TouchMove(e.GetCurrentPoint(this).Position);
            }
        }

        private void InitLayer()
        {
            LayerItems.Clear();
            for (int i = 0; i < 3; i++)
            {
                var layer = new PageLayer(DrawerCanvas, ActualWidth, ActualHeight)
                {
                    FontFamily = CacheFont,
                    Background = CacheBackground,
                    Foreground = CacheForeground,
                    BackgroundImage = CacheImage,
                };
                LayerItems.Add(layer);
                layer.Resize(0, 0, ActualWidth, ActualHeight);
            }
        }

        private async void UpdateSetting()
        {
            if (Setting == null)
            {
                return;
            }
            CacheForeground = ColorHelper.From(Setting.Foreground, Colors.Black);
            CacheBackground = ColorHelper.From(Setting.Background);
            CacheFont = new CanvasTextFormat()
            {
                FontFamily = await App.ViewModel.DiskRepository.GetFontFamilyAsync(Setting.FontFamily), // name.ttf#name
                FontSize = Setting.FontSize
            };
            if (!string.IsNullOrWhiteSpace(Setting.BackgroundImage))
            {
                var bgImage = await App.ViewModel.DiskRepository.GetFilePathAsync(Setting.BackgroundImage);
                CacheImage = await CanvasBitmap.LoadAsync(DrawerCanvas, bgImage);
            }
            else
            {
                CacheImage = null;
            }
            foreach (PageLayer item in LayerItems)
            {
                item.FontFamily = CacheFont;
                item.Background = CacheBackground;
                item.Foreground = CacheForeground;
                item.BackgroundImage = CacheImage;
            }
            switch (Setting.Animation)
            {
                case 1:
                    Animate = new CoverAnimate();
                    break;
                case 2:
                    Animate = new VerticalCoverAnimate();
                    break;
                case 3:
                    Animate = new SimulateAnimate();
                    break;
                case 4:
                    Animate = new ScrollAnimate();
                    break;
                default:
                    Animate = new NoneAnimate();
                    break;
            }
        }

        
    }
}
