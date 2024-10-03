using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Graphics.Canvas;
using System;
using ZoDream.Shared.Interfaces;
using ZoDream.Shared.Interfaces.Tokenizers;
using Microsoft.Graphics.Canvas.Text;
using Microsoft.UI;
using Windows.UI;
using Windows.Foundation;
using System.Numerics;

namespace ZoDream.Reader.Controls
{
    public class PageLayer(TextContainer container) : ICanvasLayer
    {
        public double Left { get; set; }

        public double Top { get; set; }

        public double ActualWidth { get; set; }

        public double ActualHeight { get; set; }

        public int Page { get; set; }

        public bool IsVisible { get; set; } = true;

        private CanvasRenderTarget? _cacheBitmap;

        private ICanvasEffect? _cacheEffect;
        public Color Foreground { get; set; } = Colors.Black;
        public Color Background { get; set; } = Colors.White;

        private void AddEffect()
        {
            _cacheEffect = new Transform2DEffect()
            {
                Source = new ShadowEffect()
                {
                    Source = _cacheBitmap,
                    BlurAmount = 2,
                },
                TransformMatrix = Matrix3x2.CreateTranslation(3, 3)
            };
        }

        public void Draw(CanvasDrawingSession target)
        {
            if (_cacheEffect != null)
            {
                target.DrawImage(_cacheEffect, (float)Left, (float)Top);
                // return;
            }
            if (_cacheBitmap == null)
            {
                return;
            }
            target.DrawImage(_cacheBitmap, (float)Left, (float)Top);
        }


        public void DrawText(INovelPage data)
        {
            if (_cacheBitmap == null)
            {
                _cacheBitmap = new CanvasRenderTarget(container.Canvas, (float)ActualWidth,
                   (float)ActualHeight, 96);
            }
            using var ds = _cacheBitmap.CreateDrawingSession();
            ds.Clear(Background);
            var backgroundImage = container.BackgroundImage;
            if (backgroundImage != null)
            {
                ds.DrawImage(backgroundImage, 0, 0, new Rect(0, 0, ActualWidth, ActualHeight));
            }
            foreach (var page in data)
            {
                using var font = new CanvasTextFormat()
                {
                    FontFamily = page.FontFamily,
                    FontSize = page.FontSize,
                };
                foreach (var item in page)
                {
                    if (item is INovelPageChar c)
                    {
                        ds.DrawText(c.Text,
                            new Vector2((float)(item.X + page.X), (float)(item.Y + page.Y)),
                            Foreground, font);
                    }
                    if (item is INovelPageImage i)
                    {
                        var image = CanvasBitmap.LoadAsync(container.Canvas, i.Source).GetAwaiter().GetResult();
                        ds.DrawImage(image, new Rect(page.X + item.X, page.Y + item.Y, item.Width, item.Height));
                    }

                }
            }
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

        public void Clear()
        {
            _cacheBitmap?.Dispose();
            _cacheEffect?.Dispose();
            _cacheBitmap = null;
            _cacheEffect = null;
        }

        public void Dispose()
        {
            _cacheBitmap?.Dispose();
            _cacheBitmap = null;
        }
    }
}
