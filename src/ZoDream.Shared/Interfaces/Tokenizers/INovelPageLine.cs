using System.Collections.Generic;
using System.Numerics;

namespace ZoDream.Shared.Interfaces.Tokenizers
{
    public interface INovelPageLine: IList<INovelPageLinePart>
    {
        public Vector2 Position { get; set; }
        public Vector2 Size { get; }
        public byte FontSize { get; }
        public string FontFamily { get; }

        public ushort FontWeight { get; }
        public bool FontItalic { get; }
    }
}
