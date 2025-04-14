using System.Collections.Generic;
using System.Numerics;

namespace ZoDream.Shared.Interfaces.Tokenizers
{
    public interface INovelPage: IList<INovelPageLine>
    {
        public Vector2 Position { get; }
    }
}
