using System.Collections.Generic;
using System.Numerics;
using ZoDream.Shared.Interfaces.Tokenizers;

namespace ZoDream.Shared.Tokenizers
{
    public class NovelPage: List<INovelPageLine>, INovelPage
    {
        public Vector2 Position { get; set; }
    }
}
