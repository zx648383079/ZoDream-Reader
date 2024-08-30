using System.Collections.Generic;
using ZoDream.Shared.Interfaces.Entities;
using ZoDream.Shared.Interfaces.Tokenizers;

namespace ZoDream.Shared.Interfaces
{
    public interface IPageTokenizer
    {
        public IList<INovelPage> Parse(INovelDocument content,
            IReadTheme setting, ICanvasControl control);
    }
}
