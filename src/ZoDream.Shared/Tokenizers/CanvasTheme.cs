using ZoDream.Shared.Interfaces;
using ZoDream.Shared.Interfaces.Entities;

namespace ZoDream.Shared.Tokenizers
{
    public class CanvasTheme: ICanvasTheme
    {
        public double Width { get; }
        public double Height { get; }
        /// <summary>
        /// 对齐，0 居左 1 居中 3 隐藏
        /// </summary>
        public int TextAlign { get; }

        public string FontFamily { get; } = string.Empty;
        public int FontSize { get; }
        /// <summary>
        /// 字体加粗，500 正常
        /// </summary>
        public int FontWeight { get; }

        public bool Underline { get; }
        public int PaddingTop { get; }
        public int PaddingLeft { get; }
        public int PaddingRight { get; }
        public int PaddingBottom { get; }

        public int LineSpacing { get; }
        public int LetterSpacing { get; }

        public int ColumnCount { get; } = 1;

        public int Gap { get; }

        public double PageInnerWidth {
            get {
                return (Width - PaddingLeft - PaddingRight - 
                    (ColumnCount - 1) * Gap) / ColumnCount;
            }
        }

        public double PageInnerHeight {
            get {
                return Height - PaddingTop - PaddingBottom;
            }
        }

        public double PageX(int column)
        {
            return PaddingLeft + column * (PageInnerWidth + Gap);
        }

        public double PageY(int column)
        {
            return PaddingTop;
        }
        public double LineY(int index)
        {
            return PaddingTop + index * (FontSize + LineSpacing);
        }

        public double FontX(int column, int index)
        {
            return FontX(PageX(column), index);
        }

        public double FontX(double pageX, int index)
        {
            return pageX + index * (FontSize + LetterSpacing);
        }
        public double FontWidth(double count)
        {
            return count * (FontSize + LetterSpacing);
        }
        public double FontHeight()
        {
            return FontSize + LineSpacing;
        }

        public (double, double) FontBound(char? code)
        {
            var height = FontHeight();
            if (code == null)
            {
                return (0, height);
            }
            if (code == '\t')
            {
                return (FontWidth(2), height);
            }
            if ((code >= 48 && code <= 57)
                || (code >= 64 && code <= 90))
            {
                return (FontWidth(.8), height);
            }
            if (code == 46 ||
                (code >= 97 && code <= 122))
            {
                return (FontWidth(.6), height);
            }
            return (FontWidth(1), height);
        }

        public CanvasTheme(IReadTheme theme, ICanvasControl control, bool isTitle = false)
        {
            Width = control.Width;
            Height = control.Height;

            FontFamily = theme.FontFamily;
            if (isTitle)
            {
                FontSize = theme.TitleFontSize;
                TextAlign = theme.TitleAlign;
                LineSpacing = theme.TitleSpacing;
            } else
            {
                FontSize = theme.FontSize;
                LineSpacing = theme.LineSpacing;
            }
            FontWeight = theme.FontWeight;
            Underline = theme.Underline;
            PaddingTop = theme.PaddingTop;
            PaddingLeft = theme.PaddingLeft;
            PaddingRight = theme.PaddingRight;
            PaddingBottom = theme.PaddingBottom;
            LetterSpacing = theme.LetterSpacing;
        }
    }
}
