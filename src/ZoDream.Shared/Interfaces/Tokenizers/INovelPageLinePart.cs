using System;
using System.Collections.Generic;
using System.Text;

namespace ZoDream.Shared.Interfaces.Tokenizers
{
    public interface INovelPageLinePart
    {
        public double X { get; }
        public double Y { get; }
        public double Height { get; }
        public double Width { get; }
    }
}
