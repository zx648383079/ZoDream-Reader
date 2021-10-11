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
using ZoDream.Shared.Events;
using ZoDream.Shared.Interfaces;
using ZoDream.Shared.Models;
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

        private Point lastMousePoint = new Point(0, 0);
        private CanvasRenderTarget renderTarget;
        private int currentPage = -1;
        private CanvasLayer[] layerItems = new CanvasLayer[3];
        private CanvasTextFormat cacheFont;
        private Color cacheForeground;
        private Color cacheBackground;
        private CanvasBitmap cacheImage;
        private Action onSwapFinished;

        /// <summary>
        /// 翻页动画方向
        /// </summary>
        private bool? swapDirect = null;
        private DispatcherTimer swapTimer;
        public ICanvasSource Source { get; set; }

        public event PageChangedEventHandler PageChanged;
        public event CanvasReadyEventHandler OnReady;

        private void DrawerCanvas_Draw(Microsoft.Graphics.Canvas.UI.Xaml.CanvasControl sender, Microsoft.Graphics.Canvas.UI.Xaml.CanvasDrawEventArgs args)
        {
            if (cacheFont == null)
            {
                return;
            }
            using (var ds = renderTarget.CreateDrawingSession())
            {
                if (swapDirect == null)
                {
                    layerItems[1]?.Draw(ds, cacheFont, cacheForeground, cacheBackground, cacheImage);
                }
                else if (swapDirect == true)
                {
                    layerItems[1].Draw(ds, cacheFont, cacheForeground, cacheBackground, cacheImage);
                    layerItems[0]?.Draw(ds, cacheFont, cacheForeground, cacheBackground, cacheImage);
                } else {
                    layerItems[3]?.Draw(ds, cacheFont, cacheForeground, cacheBackground, cacheImage);
                    layerItems[1].Draw(ds, cacheFont, cacheForeground, cacheBackground, cacheImage);
                }
            }
            args.DrawingSession.DrawImage(renderTarget);
        }

        private void DrawerCanvas_CreateResources(Microsoft.Graphics.Canvas.UI.Xaml.CanvasControl sender, Microsoft.Graphics.Canvas.UI.CanvasCreateResourcesEventArgs args)
        {
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
            return layer;
        }

        public void SwapTo(IList<PageItem> pages, int page)
        {
            layerItems[0] = layerItems[1];
            layerItems[1] = CreateLayer(pages, page);
            SwapAnimate(true, () =>
            {
                swapDirect = null;
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
            SwapAnimate(false, () =>
            {
                swapDirect = null;
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

        private async void SwapAnimate(bool toNext, Action finished)
        {
            swapDirect = toNext;
            onSwapFinished = finished;
            var setting = App.ViewModel.Setting;
            cacheFont = new CanvasTextFormat()
            {
                FontFamily = setting.FontFamily, // name.ttf#name
                FontSize = (float)setting.FontSize
            };
            cacheForeground = ColorHelper.From(setting.Foreground, Colors.Black);
            cacheBackground = ColorHelper.From(setting.Background);
            cacheImage = !string.IsNullOrWhiteSpace(setting.BackgroundImage) ? 
                await CanvasBitmap.LoadAsync(DrawerCanvas, setting.BackgroundImage) : null;
            if (renderTarget == null)
            {
                renderTarget = new CanvasRenderTarget(DrawerCanvas, (float)DrawerCanvas.ActualWidth,
                   (float)DrawerCanvas.ActualHeight, 96);
            }
            if (setting.Animation < 1)
            {
                SwapTimer_Tick(null, null);
                return;
            }
            if (swapTimer == null)
            {
                swapTimer = new DispatcherTimer()
                {
                    Interval = TimeSpan.FromMilliseconds(40),
                };
                swapTimer.Tick += SwapTimer_Tick;
            }
            swapTimer.Start();
        }

        private void SwapTimer_Tick(object sender, object e)
        {
            if (IsSwapFinished())
            {
                onSwapFinished?.Invoke();
                swapDirect = null;
            }
            if (swapDirect == true)
            {
                layerItems[0].X -= 10;
            } else
            {
                layerItems[1].X += 10;
            }
            DrawerCanvas.Invalidate();
        }

        private bool IsSwapFinished()
        {
            if (App.ViewModel.Setting.Animation < 1 || swapDirect == null)
            {
                return true;
            }
            if (swapDirect == true)
            {
                return layerItems[0] == null || layerItems[0].X <= 10 - layerItems[0].Width;
            }
            return layerItems[1] == null || layerItems[1].X <= -10;
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
            renderTarget?.Dispose();
            swapTimer?.Stop();
            DrawerCanvas.RemoveFromVisualTree();
            DrawerCanvas = null;
        }

        private void UserControl_ManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            lastMousePoint = e.Position;
        }

        private void UserControl_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            MouseMoveEnd(e.Position);
        }

        private void MouseMoveEnd(Point p)
        {
            var diffX = p.X - lastMousePoint.X;
            var diffY = p.Y - lastMousePoint.Y;
            var diff = Math.Pow(diffX, 2) + Math.Pow(diffY, 2);
            if (diff < 25)
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

            if (Math.Abs(diffX) > Math.Abs(diffY))
            {
                if (diffX > 0)
                {
                    SwapPrevious();
                }
                else if (diffX < 0)
                {
                    SwapNext();
                }
            }
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            renderTarget?.Dispose();
            renderTarget = null;
        }

        private void UserControl_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            lastMousePoint = e.GetCurrentPoint(this).Position;
        }

        private void UserControl_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
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
    }
}
