using System.Numerics;
using ZoDream.Shared.Interfaces.Tokenizers;

namespace ZoDream.Shared.Tokenizers
{
    public struct NovelPageChar(string text): INovelPageChar
    {
        public Vector2 Position { get; set; }

        public Vector2 Size { get; set; }
        public string Text { get; } = text;


    }
}
