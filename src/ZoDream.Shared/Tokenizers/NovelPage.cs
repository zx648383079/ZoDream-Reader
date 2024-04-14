using System.Collections.Generic;
using ZoDream.Shared.Interfaces.Tokenizers;

namespace ZoDream.Shared.Tokenizers
{
    public class NovelPage: List<INovelPageLine>, INovelPage
    {
        public int X { get; }
        public int Y { get; }
    }
}
