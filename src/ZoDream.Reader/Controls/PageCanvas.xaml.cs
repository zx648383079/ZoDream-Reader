using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using Vortice.Direct2D1;
using Vortice.DirectWrite;
using ZoDream.Reader.Drawing;
using ZoDream.Reader.Events;
using ZoDream.Shared.Events;
using ZoDream.Shared.Interfaces;
using ZoDream.Shared.Models;
using static Vortice.DirectWrite.DWrite;
using Point = System.Windows.Point;

namespace ZoDream.Reader.Controls
{
    /// <summary>
    /// PageCanvas.xaml 的交互逻辑
    /// </summary>
    public partial class PageCanvas : UserControl, ICanvasRender
    {
        public PageCanvas()
        {
            InitializeComponent();
        }

        private ID2D1RenderTarget? renderTarget;
        private Point lastMousePoint = new Point(0, 0);


        private int currentPage = -1;
        private CanvasLayer[] layerItems = new CanvasLayer[3];
        /// <summary>
        /// 翻页动画方向
        /// </summary>
        private bool? swapDirect;
        private DispatcherTimer? timer;
        public ICanvasSource? Source { get; set; }

        public event PageChangedEventHandler? PageChanged;
        public event CanvasReadyEventHandler? OnReady;


        public void Draw(IEnumerable<PageItem> pages)
        {
            if (renderTarget == null)
            {
                renderTarget = DrawerCanvas.CreateRenderTarget();
            }
            if (renderTarget == null)
            {
                return;
            }
            var dwriteFactory = DWriteCreateFactory<IDWriteFactory>();
            var font = dwriteFactory.CreateTextFormat(FontFamily.ToString(), (float)FontSize);
            var setting = App.ViewModel.Setting;
            var color = renderTarget.CreateSolidColorBrush(ColorHelper.From(setting.Foreground));
            renderTarget.BeginDraw();
            renderTarget.Clear(ColorHelper.From(setting.Background));
            if (!string.IsNullOrWhiteSpace(setting.BackgroundImage))
            {
                renderTarget.DrawBitmap(DrawerCanvas.LoadBitmap(setting.BackgroundImage));
            }
            foreach (var page in pages)
            {
                foreach (var item in page.Data)
                {
                    renderTarget.DrawText(item.Code.ToString(), font, 
                        new RectangleF((float)item.X, (float)item.Y, int.MaxValue, int.MaxValue),
                        color);
                }
            }
            renderTarget.EndDraw();
            DrawerCanvas.Invalidate();
        }

        /// <summary>
        /// 使用过渡动画切换到新的页面，下一页
        /// </summary>
        /// <param name="pages"></param>
        public void SwapTo(IEnumerable<PageItem> pages)
        {
            SwapTo(pages, 0);
        }

        private CanvasLayer CreateLayer(IEnumerable<PageItem> pages, int page)
        {
            var layer = DrawerCanvas.CreateLayer();
            layer.Page = page;
            layer.Add(pages);
            return layer;
        }

        public void SwapTo(IEnumerable<PageItem> pages, int page)
        {
            layerItems[0] = layerItems[1];
            layerItems[1] = CreateLayer(pages, page);
            swapDirect = true;
            Flush();
            layerItems[1].Draw(renderTarget);
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
        public void SwapFrom(IEnumerable<PageItem> pages)
        {
            SwapFrom(pages, 0);
        }

        public void SwapFrom(IEnumerable<PageItem> pages, int page)
        {
            layerItems[2] = layerItems[1];
            layerItems[1] = CreateLayer(pages, page);
            swapDirect = false;
            Flush();
            layerItems[1].Draw(renderTarget);
        }
        public async void SwapFrom(int page)
        {
            if (Source == null || !Source.Canable(page))
            {
                return;
            }
            SwapFrom(await Source.GetAsync(page), page);
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
            renderTarget.BeginDraw();
            renderTarget.Clear(ColorHelper.FromArgb(255, 255, 255, 255));
            renderTarget.EndDraw();
            DrawerCanvas.Invalidate();
        }

        private void UserControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            lastMousePoint = e.GetPosition(this);
        }

        private void UserControl_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var p = e.GetPosition(this);
            var diff = p - lastMousePoint;
            if (diff.Length < 5)
            {
                // TODO 点击
                if (p.X < ActualWidth / 3)
                {
                    SwapPrevious();
                } else if (p.X > ActualWidth * .7)
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
                } else if (diff.X < 0)
                {
                    SwapNext();
                }
            }
        }

        private void DrawerCanvas_Draw(object sender)
        {
            if (renderTarget == null)
            {
                return;
            }
            DrawerCanvas.DrawImage(renderTarget);
        }

        private void DrawerCanvas_CreateResources(object sender)
        {
            OnReady?.Invoke(this);
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            renderTarget?.Dispose();
            renderTarget = null;
        }

        private void UserControl_KeyUp(object sender, KeyEventArgs e)
        {
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

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            timer?.Stop();
            renderTarget?.Dispose();
            renderTarget = null;
        }
    }
}
