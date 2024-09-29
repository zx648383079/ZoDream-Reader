using System;
using System.Collections.Generic;
using System.Text;

namespace ZoDream.Shared.Interfaces
{
    public interface ICanvasTheme: ICanvasControl
    {
        /// <summary>
        /// 对齐，0 居左 1 居中 3 居右
        /// </summary>
        public int TextAlign { get; }

        public string FontFamily { get; }
        public int FontSize { get; }
        /// <summary>
        /// 字体加粗，500 正常
        /// </summary>
        public int FontWeight { get; }
        public bool FontItalic { get; }
        public bool Underline { get; }
        public int PaddingTop { get; }
        public int PaddingLeft { get; }
        public int PaddingRight { get; }
        public int PaddingBottom { get; }

        public int LineSpacing { get; }
        public int LetterSpacing { get; }

        public double PageInnerWidth { get; }
        public double PageInnerHeight { get; }

        public (double, double) FontBound(char? code);
    }
}
