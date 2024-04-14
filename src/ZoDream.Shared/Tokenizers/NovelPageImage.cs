using System;
using System.Collections.Generic;
using System.Text;
using ZoDream.Shared.Interfaces.Tokenizers;

namespace ZoDream.Shared.Tokenizers
{
    public class NovelPageImage(string source): INovelPageImage
    {
        public double X { get; }
        public double Y { get; }
        public double Height { get; }
        public double Width { get; }
        public string Source { get; } = source;


    }
}
