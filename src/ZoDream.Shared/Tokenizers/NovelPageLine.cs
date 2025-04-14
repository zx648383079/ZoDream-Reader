using System;
using System.Collections.Generic;
using System.Numerics;
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

        public Vector2 Position { get; set; }



        public byte FontSize { get; }
        public string FontFamily { get; } = string.Empty;

        public ushort FontWeight { get; }
        public bool FontItalic { get; }

        public Vector2 Size 
        {
            get {
                var maxX = .0f;
                var maxY = .0f;
                foreach (var item in this)
                {
                    maxX = Math.Max(maxX, item.Size.X);
                    maxY = Math.Max(maxY, item.Size.Y);
                }
                return new(maxX, maxY);
            }
        }

    }
}
