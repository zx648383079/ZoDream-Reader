using System;
using System.Collections.Generic;
using System.Text;
using ZoDream.Shared.Interfaces.Tokenizers;

namespace ZoDream.Shared.Tokenizers
{
    public class NovelPageChar(string text): INovelPageChar
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Height { get; set; }
        public double Width { get; set; }
        public string Text { get; } = text;

    }
}
