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
using ZoDream.Reader.Drawing.Animations;
using ZoDream.Reader.Events;
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

        
        private int currentPage = -1;
        public int CurrentPage
        {
            get { return currentPage; }
            set {
                if (currentPage == value)
                {
                    return;
                }
                currentPage = value;
                PageChanged?.Invoke(this, value, null);
            }
        }
        internal CanvasLayer[] layerItems = new CanvasLayer[3];
        private CanvasTextFormat cacheFont;
        private Color cacheForeground;
        private Color cacheBackground;
        private CanvasBitmap cacheImage;

        private ILayerAnimate swapAnimate;
        private bool isPointerOn = false;
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

        private static async void OnSettingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var canvas = d as PageCanvas;
            
            var setting = e.NewValue as AppOption;
            canvas.swapAnimate?.Dispose();
            canvas.swapAnimate = canvas.CreateAnimate(setting.Animation);
            canvas.cacheForeground = ColorHelper.From(setting.Foreground, Colors.Black);
            canvas.cacheBackground = ColorHelper.From(setting.Background);
            canvas.cacheFont = new CanvasTextFormat()
            {
                FontFamily = await App.ViewModel.DiskRepository.GetFontFamilyAsync(setting.FontFamily), // name.ttf#name
                FontSize = (float)setting.FontSize
            };
            if (!canvas.IsReady)
            {
                return;
            }
            if (!string.IsNullOrWhiteSpace(setting.BackgroundImage))
            {
                var bgImage = await App.ViewModel.DiskRepository.GetFilePathAsync(setting.BackgroundImage);
                canvas.cacheImage = await CanvasBitmap.LoadAsync(canvas.DrawerCanvas, bgImage);
            } else
            {
                canvas.cacheImage = null;
            }
            foreach (var item in canvas.layerItems)
            {
                item?.Update(canvas.cacheFont, canvas.cacheForeground, canvas.cacheBackground, canvas.cacheImage);
            }
        }

        private ILayerAnimate CreateAnimate(int val)
        {
            switch (val)
            {
                case 1:
                    return new SimulateAnimate();
                case 2:
                    return new CoverAnimate(this);
                case 3:
                    return new VerticalCoverAnimate(this);
                case 4:
                    return new ScrollAnimate();
                default:
                    return new NoneAnimate(this);
            }
        }

        private void DrawerCanvas_Draw(Microsoft.Graphics.Canvas.UI.Xaml.CanvasControl sender, Microsoft.Graphics.Canvas.UI.Xaml.CanvasDrawEventArgs args)
        {
            swapAnimate.Draw(args.DrawingSession);
        }

        private void DrawerCanvas_CreateResources(Microsoft.Graphics.Canvas.UI.Xaml.CanvasControl sender, Microsoft.Graphics.Canvas.UI.CanvasCreateResourcesEventArgs args)
        {
            IsReady = true;
            OnReady?.Invoke(this);
        }

        public void Draw(IList<PageItem> pages)
        {
            //renderTarget = new CanvasRenderTarget(DrawerCanvas, (float)DrawerCanvas.ActualWidth,
            //       (float)DrawerCanvas.ActualHeight, 96);
            //var font = new CanvasTextFormat()
            //{
            //    FontFamily = FontFamily.Source, // name.ttf#name
            //    FontSize = (float)FontSize
            //};
            //var color = ColorHelper.FromArgb(255, 0, 0, 0);
            //using (var ds = renderTarget.CreateDrawingSession())
            //{
            //    ds.Clear(ColorHelper.FromArgb(255, 255, 255, 255));
            //    foreach (var page in pages)
            //    {
            //        foreach (var item in page.Data)
            //        {
            //            ds.DrawText(item.Code.ToString(), 
            //                new Vector2((float)item.X, (float)item.Y), 
            //                color, font);
            //        }
            //    }
            //}
            //DrawerCanvas.Invalidate();
        }

        /// <summary>
        /// 使用过渡动画切换到新的页面，下一页
        /// </summary>
        /// <param name="pages"></param>
        public void SwapTo(IList<PageItem> pages)
        {
            SwapTo(pages, 0);
        }

        private CanvasLayer CreateLayer(IList<PageItem> pages, int page)
        {
            var layer = new CanvasLayer(DrawerCanvas, (float)DrawerCanvas.ActualWidth,
                   (float)DrawerCanvas.ActualHeight);
            layer.Page = page;
            layer.Add(pages);
            layer.Update(cacheFont, cacheForeground, cacheBackground, cacheImage);
            return layer;
        }

        public CanvasLayer CreateLayer(int page)
        {
            if (!Source.Canable(page))
            {
                return null;
            }
            var layer = CreateLayer(new List<PageItem>(), page);
            Task.Factory.StartNew(async () =>
            {
                var data = await Source.GetAsync(page);
                layer.Clear();
                layer.Add(data);
                layer.Update(cacheFont, cacheForeground, cacheBackground, cacheImage);
            });
            return layer;
        }

        public void SwapTo(IList<PageItem> pages, int page)
        {
            layerItems[0] = layerItems[1];
            layerItems[1] = CreateLayer(pages, page);
            swapAnimate.Animate(true, () =>
            {
                currentPage = page;
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
            layerItems[2] = layerItems[1];
            layerItems[1] = CreateLayer(pages, page);
            swapAnimate.Animate(false, () =>
            {
                currentPage = page;
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

        public void Invalidate()
        {
            DrawerCanvas.Invalidate();
        }

        public async void SwapNext()
        {
            await SwapTo(currentPage + App.ViewModel.Setting.ColumnCount);
        }

        public async void SwapPrevious()
        {
            await SwapTo(currentPage - App.ViewModel.Setting.ColumnCount);
        }

        public void Flush()
        {
            DrawerCanvas.Invalidate();
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            swapAnimate?.Dispose();
            DrawerCanvas.RemoveFromVisualTree();
            DrawerCanvas = null;
        }

        private void UserControl_ManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            swapAnimate.TouchMoveBegin(e.Position);
        }

        private void UserControl_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            MouseMoveEnd(e.Position);
        }

        private void MouseMoveEnd(Point p)
        {
            swapAnimate.TouchMoveEnd(p);
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            for (int i = 0; i < layerItems.Length; i++)
            {
                layerItems[i]?.Dispose();
                layerItems[i] = null;
            }
        }

        private void UserControl_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            isPointerOn = true;
            swapAnimate.TouchMoveBegin(e.GetCurrentPoint(this).Position);
        }

        private void UserControl_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            isPointerOn = false;
            MouseMoveEnd(e.GetCurrentPoint(this).Position);
        }

        private void UserControl_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Right || e.Key == VirtualKey.PageDown)
            {
                SwapNext();
                return;
            }
            if (e.Key == VirtualKey.Left || e.Key == VirtualKey.PageUp)
            {
                SwapPrevious();
                return;
            }
        }

        private void UserControl_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            swapAnimate.TouchMove(e.Position);
        }

        private void UserControl_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            if (isPointerOn)
            {
                swapAnimate.TouchMove(e.GetCurrentPoint(this).Position);
            }
        }
    }
}
