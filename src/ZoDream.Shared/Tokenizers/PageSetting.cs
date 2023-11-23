using System;
using System.Collections.Generic;
using System.Text;

namespace ZoDream.Shared.Tokenizers
{
    public class PageSetting
    {
        public double Width { get; set; }

        public double Height { get; set; }

        /// <summary>
        /// 单个字的大小
        /// </summary>
        public int FontSize { get; set; } = 20;

        public int LineSpace { get; set; } = 2;

        public int LetterSpace { get; set; } = 2;

        public int Left { get; set; } = 10;

        public int Right { get; set; } = 10;

        public int Top { get; set; } = 10;

        public int Bottom { get; set; } = 10;

        public int Gap { get; set; } = 20;

        public int ColumnCount { get; set; } = 1;


        public double PageInnerWidth {
            get {
                return (Width - Left - Right - (ColumnCount - 1) * Gap) / ColumnCount;
            }
        }

        public double PageInnerHeight {
            get {
                return Height - Top - Bottom;
            }
        }

        public double PageX(int column)
        {
            return Left + column * (PageInnerWidth + Gap);
        }

        public double PageY(int column)
        {
            return Top;
        }
        public double LineY(int index)
        {
            return Top + index * (FontSize + LineSpace);
        }

        public double FontX(int column, int index)
        {
            return FontX(PageX(column), index);
        }

        public double FontX(double pageX, int index)
        {
            return pageX + index * (FontSize + LetterSpace);
        }
        public double FontWidth(double count)
        {
            return count * (FontSize + LetterSpace);
        }
        public double FontWidth(char? code)
        {
            if (code == null)
            {
                return 0;
            }
            if (code == '\t')
            {
                return FontWidth(2);
            }
            if ((code >= 48 && code <= 57)
                || (code >= 64 && code <= 90))
            {
                return FontWidth(.8);
            }
            if (code == 46 ||
                (code >= 97 && code <= 122))
            {
                return FontWidth(.6);
            }
            return FontWidth(1);
        }

    }
}
