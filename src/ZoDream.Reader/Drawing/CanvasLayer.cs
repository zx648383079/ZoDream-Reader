using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Vortice.Direct2D1;
using Vortice.DirectWrite;
using ZoDream.Shared.Interfaces;
using ZoDream.Shared.Models;
using static Vortice.DirectWrite.DWrite;

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

        private ID2D1RenderTarget? CacheBitmap;

        private ID2D1RenderTarget? CacheEffect;

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
            Data.Clear();
            CacheBitmap?.Dispose();
            CacheEffect?.Dispose();
        }

        /// <summary>
        /// 开始翻页
        /// </summary>
        /// <param name="point"></param>
        public void BeginSwap(Point point)
        {
            X = (float)point.X;
            Y = (float)point.Y;
            AddEffect();
        }

        private void AddEffect()
        {
            
        }

        /// <summary>
        /// 移动位置
        /// </summary>
        /// <param name="point"></param>
        public void MoveSwap(Point point)
        {
            X = (float)point.X;
            Y = (float)point.Y;
            if (CacheEffect == null)
            {
                AddEffect();
            }
        }

        public void MoveSwap(float diff)
        {
            if (CacheEffect == null)
            {
                AddEffect();
            }
            X += diff;
        }

        /// <summary>
        /// 翻页结束，自动完成后续动画
        /// </summary>
        public void EndSwap()
        {
            CacheEffect?.Dispose();
            CacheEffect = null;
        }


        public void Draw(ID2D1RenderTarget target)
        {
            // target.DrawBitmap(CacheBitmap);
            if (CacheEffect != null)
            {
                Control.DrawImage(CacheEffect);
                // return;
            }
            if (CacheBitmap == null)
            {
                return;
            }
            Control.DrawImage(CacheBitmap);
        }

        public void Draw(ID2D1RenderTarget target, IDWriteTextFormat font, ID2D1SolidColorBrush foreground, ID2D1SolidColorBrush background, ID2D1Bitmap? backgroundImage)
        {
            // var layer = target.CreateLayer(new SizeF(Width, Height));
            // var lpt = new LayerParameters();
            // lpt.Opacity = 1;
            // target.PushLayer(ref lpt, layer);
            // target.Transform = Matrix3x2.CreateTranslation(0, 0);
            
            // target.PopLayer();
            // layer.Dispose();
        }

        public void Update(IDWriteTextFormat font, ID2D1SolidColorBrush foreground, Color background, ID2D1Bitmap? backgroundImage)
        {
            if (CacheBitmap == null)
            {
                CacheBitmap = Control.CreateRenderTarget();
            }
            CacheBitmap.BeginDraw();
            CacheBitmap.Clear(background);
            if (backgroundImage != null)
            {
                CacheBitmap.DrawBitmap(backgroundImage);
            }
            foreach (var item in Data)
            {
                CacheBitmap.DrawText(item.Code.ToString(), font,
                    new RectangleF((float)item.X, (float)item.Y, int.MaxValue, int.MaxValue),
                    foreground);
            }
            CacheBitmap.EndDraw();
        }

        public void Dispose()
        {
            Data.Clear();
            CacheBitmap?.Dispose();
            CacheBitmap = null;
        }
    }
}
