using System;
using System.Collections.Generic;
using System.Text;

namespace ZoDream.Shared.Interfaces.Tokenizers
{
    public interface INovelPageChar: INovelPageLinePart
    {
        public string Text { get; }
    }
}
