using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vortice.Direct2D1;
using Vortice.DirectWrite;
using ZoDream.Shared.Interfaces;
using ZoDream.Shared.Models;
using static Vortice.DirectWrite.DWrite;

namespace ZoDream.Reader.Drawing
{
    public class CanvasLayer: ICanvasLayer<ID2D1RenderTarget, Point, IDWriteTextFormat, ID2D1SolidColorBrush, ID2D1Bitmap>
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
        }

        /// <summary>
        /// 开始翻页
        /// </summary>
        /// <param name="point"></param>
        public void BeginSwap(Point point)
        {

        }

        /// <summary>
        /// 移动位置
        /// </summary>
        /// <param name="point"></param>
        public void MoveSwap(Point point)
        {

        }

        /// <summary>
        /// 翻页结束，自动完成后续动画
        /// </summary>
        public void EndSwap()
        {

        }

        public void Draw(ID2D1RenderTarget target)
        {
            var setting = App.ViewModel.Setting;
            var dwriteFactory = DWriteCreateFactory<IDWriteFactory>();
            Draw(target, dwriteFactory.CreateTextFormat(setting.FontFamily, (float)setting.FontSize),
                target.CreateSolidColorBrush(ColorHelper.From(setting.Foreground)),
                !string.IsNullOrWhiteSpace(setting.BackgroundImage) ? 
                Control.LoadBitmap(setting.BackgroundImage) : null);
        }

        public void Draw(ID2D1RenderTarget target, IDWriteTextFormat font, ID2D1SolidColorBrush color, ID2D1Bitmap? background)
        {
            var layer = target.CreateLayer(new SizeF(Width, Height));
            var lpt = new LayerParameters();
            target.PushLayer(ref lpt, layer);
            
            var setting = App.ViewModel.Setting;
            target.Clear(ColorHelper.From(setting.Background));
            if (background != null)
            {
                target.DrawBitmap(background);
            }
            foreach (var item in Data)
            {
                target.DrawText(item.Code.ToString(), font,
                    new RectangleF((float)item.X, (float)item.Y, int.MaxValue, int.MaxValue),
                    color);
            }
            target.PopLayer();
        }
    }
}
