using Microsoft.Graphics.Canvas;
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
    public class CanvasLayer: ICanvasLayer<CanvasDrawingSession, Point, CanvasTextFormat, Color, ICanvasImage>
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

        public async void Draw(CanvasDrawingSession target)
        {
            var setting = App.ViewModel.Setting;
            var font = new CanvasTextFormat()
            {
                FontFamily = setting.FontFamily, // name.ttf#name
                FontSize = (float)setting.FontSize
            };
            var color = ColorHelper.From(setting.Foreground);
            Draw(target, font, color, ColorHelper.From(setting.Background), !string.IsNullOrWhiteSpace(setting.BackgroundImage) ?
                await CanvasBitmap.LoadAsync(Control, setting.BackgroundImage) : null);
        }

        public void Draw(CanvasDrawingSession target, 
            CanvasTextFormat font, Color foreground, Color background, ICanvasImage backgroundImage)
        {
            var cl = new CanvasCommandList(Control);
            using (var ds = cl.CreateDrawingSession())
            {
                ds.Clear(background);
                if (backgroundImage != null)
                {
                    ds.DrawImage(backgroundImage);
                }
                foreach (var item in Data)
                {
                    ds.DrawText(item.Code.ToString(),
                                new Vector2((float)item.X, (float)item.Y),
                                foreground, font);
                }
            }
            using (target.CreateLayer(1))
            {
                target.DrawImage(cl);
            }
        }
    }
}
