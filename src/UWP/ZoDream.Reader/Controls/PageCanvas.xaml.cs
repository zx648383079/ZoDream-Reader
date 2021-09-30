using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Text;
using System;
using System.Collections.Generic;
using System.Numerics;
using Windows.Foundation;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using ZoDream.Reader.Drawing;
using ZoDream.Reader.Events;
using ZoDream.Shared.Events;
using ZoDream.Shared.Interfaces;
using ZoDream.Shared.Models;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace ZoDream.Reader.Controls
{
    public sealed partial class PageCanvas : UserControl, ICanvasRender
    {
        public PageCanvas()
        {
            this.InitializeComponent();
        }

        private CanvasRenderTarget renderTarget;
        private Point lastMousePoint = new Point(0, 0);

        private int currentPage = -1;
        private CanvasLayer[] layerItems = new CanvasLayer[3];

        /// <summary>
        /// 翻页动画方向
        /// </summary>
        private bool? swapDirect = null;
        private DispatcherTimer timer;
        public ICanvasSource Source { get; set; }

        public event PageChangedEventHandler PageChanged;
        public event CanvasReadyEventHandler OnReady;

        private void DrawerCanvas_Draw(Microsoft.Graphics.Canvas.UI.Xaml.CanvasControl sender, Microsoft.Graphics.Canvas.UI.Xaml.CanvasDrawEventArgs args)
        {
            if (renderTarget == null)
            {
                return;
            }
            args.DrawingSession.DrawImage(renderTarget);
        }

        private void DrawerCanvas_CreateResources(Microsoft.Graphics.Canvas.UI.Xaml.CanvasControl sender, Microsoft.Graphics.Canvas.UI.CanvasCreateResourcesEventArgs args)
        {
            OnReady?.Invoke(this);
        }

        public void Draw(IList<PageItem> pages)
        {
            renderTarget = new CanvasRenderTarget(DrawerCanvas, (float)DrawerCanvas.ActualWidth,
                   (float)DrawerCanvas.ActualHeight, 96);
            var font = new CanvasTextFormat()
            {
                FontFamily = FontFamily.Source, // name.ttf#name
                FontSize = (float)FontSize
            };
            var color = ColorHelper.FromArgb(255, 0, 0, 0);
            using (var ds = renderTarget.CreateDrawingSession())
            {
                ds.Clear(ColorHelper.FromArgb(255, 255, 255, 255));
                foreach (var page in pages)
                {
                    foreach (var item in page.Data)
                    {
                        ds.DrawText(item.Code.ToString(), 
                            new Vector2((float)item.X, (float)item.Y), 
                            color, font);
                    }
                }
            }
            DrawerCanvas.Invalidate();
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
                PageChanged?.Invoke(this, page, pages[0].Begin);
            });
        }

        public async void SwapTo(int page)
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
            swapDirect = toNext;
            if (renderTarget == null)
            {
                renderTarget = new CanvasRenderTarget(DrawerCanvas, (float)DrawerCanvas.ActualWidth,
                   (float)DrawerCanvas.ActualHeight, 96);
            }
            var setting = App.ViewModel.Setting;
            var font = new CanvasTextFormat()
            {
                FontFamily = setting.FontFamily, // name.ttf#name
                FontSize = (float)setting.FontSize
            };
            var color = ColorHelper.From(setting.Foreground);
            using (var ds = renderTarget.CreateDrawingSession())
            {
                ds.Clear(ColorHelper.FromArgb(255, 255, 255, 255));
                layerItems[1].Draw(ds, font, color, null);
            }
            DrawerCanvas.Invalidate();
            finished?.Invoke();
        }

        public void SwapNext()
        {
            SwapTo(currentPage++);
        }

        public void SwapPrevious()
        {
            SwapTo(currentPage--);
        }

        public void Flush()
        {
            if (renderTarget == null)
            {
                return;
            }
            using (var ds = renderTarget.CreateDrawingSession())
            {
                ds.Clear(ColorHelper.FromArgb(255, 255, 255, 255));
            }
            DrawerCanvas.Invalidate();
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            timer?.Stop();
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
