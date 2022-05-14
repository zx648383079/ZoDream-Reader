using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Graphics.Canvas.Text;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;
using ZoDream.Shared.Interfaces;
using ZoDream.Shared.Models;

namespace ZoDream.Reader.Drawing
{
    public class PageLayer: ICanvasLayer
    {

        public PageLayer(CanvasControl control, double width, double height)
        {
            ActualWidth = width;
            ActualHeight = height;
            Control = control;
        }

        private CanvasControl Control;


        public double Left { get; set; } = 0;

        public double Top { get; set; } = 0;

        public double ActualWidth { get; set; }

        public double ActualHeight { get; set; }

        public int Page { get; set; }
        public IList<PageItem> Content { get; set; }

        public bool IsVisible { get; set; } = true;

        private CanvasRenderTarget CacheBitmap;

        private ICanvasEffect CacheEffect;

        public CanvasTextFormat FontFamily { get; set; }
        public Color Foreground { get; set; } = Colors.Black;
        public Color Background { get; set; } = Colors.White;
        public ICanvasImage BackgroundImage { get; set; }

        public void Add(IEnumerable<CharItem> items)
        {
            Add(new List<PageItem>
            {
                new PageItem() { Data = items.ToList() }
            });
        }

        public void Add(IEnumerable<PageItem> items)
        {
            Content = items.ToList();
        }

        public void Clear()
        {
            Content?.Clear();
            CacheBitmap?.Dispose();
            CacheEffect?.Dispose();
            CacheBitmap = null;
            CacheEffect = null;
        }


        private void AddEffect()
        {
            CacheEffect = new Transform2DEffect() {
                Source = new ShadowEffect()
                {
                    Source = CacheBitmap,
                    BlurAmount = 2,
                },
                TransformMatrix = Matrix3x2.CreateTranslation(3, 3)
            };
        }

        public void Draw(CanvasDrawingSession target)
        {
            if (CacheEffect != null)
            {
                target.DrawImage(CacheEffect, (float)Left, (float)Top);
                // return;
            }
            if (CacheBitmap == null)
            {
                return;
            }
            target.DrawImage(CacheBitmap, (float)Left, (float)Top);
        }

        public void InvalidateVisual()
        {
            if (CacheBitmap == null)
            {
                CacheBitmap = new CanvasRenderTarget(Control, (float)ActualWidth,
                   (float)ActualHeight, 96);
            }
            using (var ds = CacheBitmap.CreateDrawingSession())
            {
                ds.Clear(Background);
                if (BackgroundImage != null)
                {
                    ds.DrawImage(BackgroundImage, 0, 0, new Rect(0, 0, ActualWidth, ActualHeight));
                }
                foreach (var page in Content)
                {
                    foreach (var item in page)
                    {
                        ds.DrawText(item.Code.ToString(),
                                new Vector2((float)item.X, (float)item.Y),
                                Foreground, FontFamily);
                    }
                }
            }
        }

        public void Dispose()
        {
            Content.Clear();
            CacheBitmap?.Dispose();
            CacheBitmap = null;
        }

        public void Move(double x, double y)
        {
            Left = x;
            Top = y;
        }

        public void Resize(double x, double y, double width, double height)
        {
            Move(x, y);
            ActualWidth = width;
            ActualHeight = height;
        }

        public void Toggle(bool visible)
        {
            IsVisible = visible;
        }
    }
}
