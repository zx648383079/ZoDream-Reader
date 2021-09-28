using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vortice.Direct2D1;
using Vortice.DirectWrite;
using ZoDream.Shared.Models;
using static Vortice.DirectWrite.DWrite;

namespace ZoDream.Reader.Drawing
{
    public class Layer
    {

        public Layer(CanvasControl control, float width, float height)
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


        public void Draw(ID2D1RenderTarget target)
        {
            var layer = target.CreateLayer(new SizeF(Width, Height));
            var lpt = new LayerParameters();
            target.PushLayer(ref lpt, layer);
            var dwriteFactory = DWriteCreateFactory<IDWriteFactory>();
            var setting = App.ViewModel.Setting;
            var font = dwriteFactory.CreateTextFormat(setting.FontFamily, (float)setting.FontSize);
            
            var color = target.CreateSolidColorBrush(ColorHelper.From(setting.Foreground));
            target.Clear(ColorHelper.From(setting.Background));
            if (!string.IsNullOrWhiteSpace(setting.BackgroundImage))
            {
                target.DrawBitmap(Control.LoadBitmap(setting.BackgroundImage));
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
