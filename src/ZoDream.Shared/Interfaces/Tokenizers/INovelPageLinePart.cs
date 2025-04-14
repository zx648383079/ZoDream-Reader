using System.Numerics;

namespace ZoDream.Shared.Interfaces.Tokenizers
{
    public interface INovelPageLinePart
    {
        public Vector2 Position { get; }
        public Vector2 Size { get; }
    }
}
