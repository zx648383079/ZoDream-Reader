using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Graphics.Canvas.Text;
using Microsoft.UI;
using Microsoft.UI.Xaml.Documents;
using System;
using System.Numerics;
using Windows.Foundation;
using Windows.UI;
using ZoDream.Shared.Interfaces;
using ZoDream.Shared.Interfaces.Tokenizers;

namespace ZoDream.Reader.Controls
{
    public class PageLayer(TextContainer container) : ICanvasLayer
    {
        public Vector2 Position { get; set; }
        public Vector2 Size { get; set; }

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
                target.DrawImage(_cacheEffect, Position.X, Position.Y);
                // return;
            }
            if (_cacheBitmap == null)
            {
                return;
            }
            target.DrawImage(_cacheBitmap, Position.X, Position.Y);
        }


        public void DrawText(INovelPage data)
        {
            if (_cacheBitmap == null)
            {
                _cacheBitmap = new CanvasRenderTarget(container.Canvas, Size.X,
                   Size.Y, 96);
            }
            using var ds = _cacheBitmap.CreateDrawingSession();
            ds.Clear(Background);
            var backgroundImage = container.BackgroundImage;
            if (backgroundImage != null)
            {
                ds.DrawImage(backgroundImage, 0, 0, new Rect(0, 0, Size.X,
                   Size.Y));
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
                            item.Position + page.Position,
                            Foreground, font);
                    }
                    if (item is INovelPageImage i)
                    {
                        var image = CanvasBitmap.LoadAsync(container.Canvas, i.Source).GetAwaiter().GetResult();
                        ds.DrawImage(image, new Rect((item.Position + page.Position).ToPoint(), item.Size.ToSize()));
                    }

                }
            }
        }

        public void Move(Vector2 point)
        {
            Position = point;
        }

        public void Resize(Vector4 bound)
        {
            Move(new(bound.X, bound.Y));
            Size = new(bound.Z, bound.W);
        }

        public void Resize(Vector2 point, Vector2 size)
        {
            Move(point);
            Size = size;
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
