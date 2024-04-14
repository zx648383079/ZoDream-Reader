using System;
using System.Collections.Generic;
using System.Text;

namespace ZoDream.Shared.Interfaces.Tokenizers
{
    public interface INovelPageImage: INovelPageLinePart
    {
        public string Source { get; }
    }
}
