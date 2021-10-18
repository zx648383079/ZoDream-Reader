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
    public class CanvasLayer: ICanvasLayer
    {

        public CanvasLayer(CanvasControl control, float width, float height)
        {
            Width = width;
            Height = height;
            Control = control;
        }

        private CanvasControl Control;

        public IList<CharItem> Data { get; set; } = new List<CharItem>();

        public float X { get; set; } = 0;

        public float Y { get; set; } = 0;

        public float Width { get; set; }

        public float Height { get; set; }

        public int Page { get; set; }

        private CanvasRenderTarget CacheBitmap;

        private ICanvasEffect CacheEffect;

        public void Add(IEnumerable<CharItem> items)
        {
            foreach (var item in items)
            {
                Data.Add(item);
            }
        }

        public void Add(IEnumerable<PageItem> items)
        {
            foreach (var item in items)
            {
                Add(item);
            }
        }

        public void Clear()
        {
            Data?.Clear();
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
                target.DrawImage(CacheEffect, X, Y);
                // return;
            }
            if (CacheBitmap == null)
            {
                return;
            }
            target.DrawImage(CacheBitmap, X, Y);
        }

        public void Update(CanvasTextFormat font, Color foreground, Color background, ICanvasImage backgroundImage)
        {
            if (CacheBitmap == null)
            {
                CacheBitmap = new CanvasRenderTarget(Control, (float)Width,
                   (float)Height, 96);
            }
            using (var ds = CacheBitmap.CreateDrawingSession())
            {
                ds.Clear(background);
                if (backgroundImage != null)
                {
                    ds.DrawImage(backgroundImage, 0, 0, new Rect(0, 0, Width, Height));
                }
                foreach (var item in Data)
                {
                    ds.DrawText(item.Code.ToString(),
                                new Vector2((float)item.X, (float)item.Y),
                                foreground, font);
                }
            }
        }

        public void Dispose()
        {
            Data.Clear();
            CacheBitmap?.Dispose();
            CacheBitmap = null;
        }
    }
}
