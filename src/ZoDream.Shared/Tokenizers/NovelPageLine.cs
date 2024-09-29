using System;
using System.Collections.Generic;
using System.Text;
using ZoDream.Shared.Interfaces;
using ZoDream.Shared.Interfaces.Tokenizers;

namespace ZoDream.Shared.Tokenizers
{
    public class NovelPageLine: List<INovelPageLinePart>, INovelPageLine
    {
        public NovelPageLine()
        {
        }

        public NovelPageLine(ICanvasTheme theme)
        {
            FontSize = theme.FontSize;
            FontFamily = theme.FontFamily;
            FontWeight = theme.FontWeight;
            FontItalic = theme.FontItalic;
        }

        public double X { get; set; }
        public double Y { get; set; }

        public int FontSize { get; }
        public string FontFamily { get; } = string.Empty;

        public int FontWeight { get; }
        public bool FontItalic { get; }

        public double ActualHeight {
            get {
                var max = .0;
                foreach (var item in this)
                {
                    if (item.Height > max)
                    {
                        max = item.Height;
                    }
                }
                return max;
            }
        }
    }
}
