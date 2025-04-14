using System;
using System.Numerics;
using ZoDream.Shared.Interfaces.Tokenizers;

namespace ZoDream.Shared.Interfaces
{
    public interface ICanvasLayer: IDisposable
    {

        public Vector2 Position { get; }
        public Vector2 Size { get; }

        public int Page { get; set; }

        public bool IsVisible { get; }

        public void Clear();

        public void Move(Vector2 point);

        public void Resize(Vector4 bound);
        public void Resize(Vector2 point, Vector2 size);

        public void Toggle(bool visible);


        public void DrawText(INovelPage data);
    }
}
