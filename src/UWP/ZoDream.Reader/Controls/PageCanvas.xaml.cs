using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ZoDream.Reader.Events;
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
        public event ControlEventHandler OnPrevious;
        public event ControlEventHandler OnNext;

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
            
        }

        public void Draw(CharItem item)
        {

        }

        public void Draw(PageItem page)
        {
            if (renderTarget == null)
            {
                renderTarget = new CanvasRenderTarget(DrawerCanvas, (float)DrawerCanvas.ActualWidth,
                   (float)DrawerCanvas.ActualHeight, 96);
            }
            var font = new CanvasTextFormat();
            font.FontFamily = FontFamily.ToString();
            font.FontSize = (float)FontSize;
            using (var ds = renderTarget.CreateDrawingSession())
            {
                ds.Clear(ColorHelper.FromArgb(255, 255, 255, 255));
                foreach (var item in page)
                {
                    ds.DrawText(item.Code.ToString(),
                        new Vector2((float)item.X, (float)item.Y),
                        ColorHelper.FromArgb(255, 0, 0, 0), font);
                }
            }
            DrawerCanvas.Invalidate();
        }

        public void Draw(IEnumerable<PageItem> pages)
        {
            renderTarget = new CanvasRenderTarget(DrawerCanvas, (float)DrawerCanvas.ActualWidth,
                   (float)DrawerCanvas.ActualHeight, 96);
            var font = new CanvasTextFormat();
            font.FontFamily = FontFamily.ToString();
            font.FontSize = (float)FontSize;
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

        public void Swap(IEnumerable<PageItem> pages)
        {
            throw new NotImplementedException();
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
            DrawerCanvas.RemoveFromVisualTree();
            DrawerCanvas = null;
        }

        private void UserControl_ManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            lastMousePoint = e.Position;
        }

        private void UserControl_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            var p = e.Position;
            var diffX = p.X - lastMousePoint.X;
            var diffY = p.Y - lastMousePoint.Y;
            var diff = Math.Pow(diffX, 2) + Math.Pow(diffY, 2);
            if (diff < 25)
            {
                // TODO 点击
                if (p.X < ActualWidth / 3)
                {
                    OnPrevious?.Invoke(this);
                }
                else if (p.X > ActualWidth * .7)
                {
                    OnNext?.Invoke(this);
                }
                return;
            }

            if (Math.Abs(diffX) > Math.Abs(diffY))
            {
                if (diffX > 0)
                {
                    OnPrevious?.Invoke(this);
                }
                else if (diffX < 0)
                {
                    OnNext?.Invoke(this);
                }
            }
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            renderTarget = null;
            DrawerCanvas.Invalidate();
        }
    }
}
