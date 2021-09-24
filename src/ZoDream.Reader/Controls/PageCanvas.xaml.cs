using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Vortice.Direct2D1;
using Vortice.DirectWrite;
using ZoDream.Reader.Drawing;
using ZoDream.Reader.Events;
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
        private IEnumerable<PageItem>? lastPage;
        private bool lastBooted = false;

        public event ControlEventHandler? OnPrevious;
        public event ControlEventHandler? OnNext;

        public void Draw(CharItem item)
        {
            
        }

        public void Draw(PageItem page)
        {
            
        }

        public void Draw(IEnumerable<PageItem> pages)
        {
            lastPage = pages;
            lastBooted = false;
            if (renderTarget == null)
            {
                renderTarget = DrawerCanvas.CreateRenderTarget();
            }
            if (renderTarget == null)
            {
                return;
            }
            lastBooted = true;
            var dwriteFactory = DWriteCreateFactory<IDWriteFactory>();
            var font = dwriteFactory.CreateTextFormat(FontFamily.ToString(), (float)FontSize);
            var color = renderTarget.CreateSolidColorBrush(ColorHelper.FromArgb(255, 0, 0, 0));
            renderTarget.BeginDraw();
            renderTarget.Clear(ColorHelper.FromArgb(255, 255, 255, 255));
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
                    OnPrevious?.Invoke(this);
                } else if (p.X > ActualWidth * .7)
                {
                    OnNext?.Invoke(this);
                }
                return;
            }

            if (Math.Abs(diff.X) > Math.Abs(diff.Y))
            {
                if (diff.X > 0)
                {
                    OnPrevious?.Invoke(this);
                } else if (diff.X < 0)
                {
                    OnNext?.Invoke(this);
                }
            }
        }

        public void Swap(IEnumerable<PageItem> pages)
        {
            Flush();
            Draw(pages);
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
            if (lastPage != null && !lastBooted)
            {
                Draw(lastPage);
            }
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            renderTarget = null;
        }
    }
}
