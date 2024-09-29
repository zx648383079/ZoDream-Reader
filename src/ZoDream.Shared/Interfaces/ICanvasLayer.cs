using System;
using ZoDream.Shared.Interfaces.Tokenizers;

namespace ZoDream.Shared.Interfaces
{
    public interface ICanvasLayer: IDisposable
    {

        public double Left { get; }
        public double Top { get; }
        public double ActualWidth { get; }
        public double ActualHeight { get; }

        public int Page { get; set; }

        public bool IsVisible { get; }

        public void Clear();

        public void Move(double x, double y);

        public void Resize(double x, double y, double width, double height);

        public void Toggle(bool visible);


        public void DrawText(INovelPage data);
    }
}
