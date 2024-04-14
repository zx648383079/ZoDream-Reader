using System;
using System.Collections.Generic;
using System.Text;

namespace ZoDream.Shared.Interfaces.Tokenizers
{
    public interface INovelPageLine: IList<INovelPageLinePart>
    {
        public double X { get; }
        public double Y { get; set; }

        public int FontSize { get; }
        public string FontFamily { get; }

        public int FontWeight { get; }
        public bool FontItalic { get; }

        public double ActualHeight { get; }
    }
}
