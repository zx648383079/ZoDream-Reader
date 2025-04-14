using System.Numerics;
using ZoDream.Shared.Interfaces.Tokenizers;

namespace ZoDream.Shared.Tokenizers
{
    public struct NovelPageImage(string source): INovelPageImage
    {
        public string Source { get; } = source;

        public Vector2 Position { get; set; }

        public Vector2 Size { get; set; }
    }
}
