using System.Numerics;
using ZoDream.Shared.Interfaces;
using ZoDream.Shared.Interfaces.Entities;

namespace ZoDream.Shared.Tokenizers
{
    public class CanvasTheme: ICanvasTheme
    {
        /// <summary>
        /// 对齐，0 居左 1 居中 3 隐藏
        /// </summary>
        public byte TextAlign { get; }

        public string FontFamily { get; } = string.Empty;
        public byte FontSize { get; }
        /// <summary>
        /// 字体加粗，500 正常
        /// </summary>
        public ushort FontWeight { get; }
        public bool FontItalic { get; }
        public bool Underline { get; }

        public Vector4 Padding { get; private set; }

        public Vector2 Spacing { get; private set; }

        

        public Vector2 Size { get; private set; }

        public int ColumnCount { get; } = 1;

        public int Gap { get; }

        public Vector2 BodySize => new(
            (Size.X - Padding.X - Padding.Z - (ColumnCount - 1) * Gap) / ColumnCount, 
            Size.Y - Padding.Y - Padding.W
            );

        public double PageX(int column)
        {
            return Padding.X + column * (BodySize.X + Gap);
        }

        public double PageY(int column)
        {
            return Padding.Y;
        }
        public double LineY(int index)
        {
            return Padding.Y + index * (FontSize + Spacing.Y);
        }

        public double FontX(int column, int index)
        {
            return FontX(PageX(column), index);
        }

        public double FontX(double pageX, int index)
        {
            return pageX + index * (FontSize + Spacing.X);
        }
        public float FontWidth(float count)
        {
            return count * (FontSize + Spacing.X);
        }
        public float FontHeight()
        {
            return FontSize + Spacing.Y;
        }

        public Vector2 FontBound(char? code)
        {
            var height = FontHeight();
            if (code == null)
            {
                return new(0, height);
            }
            if (code == '\t')
            {
                return new(FontWidth(2), height);
            }
            if ((code >= 48 && code <= 57)
                || (code >= 64 && code <= 90))
            {
                return new(FontWidth(.8f), height);
            }
            if (code == 46 ||
                (code >= 97 && code <= 122))
            {
                return new(FontWidth(.6f), height);
            }
            return new(FontWidth(1), height);
        }

        public CanvasTheme(IReadTheme theme, ICanvasControl control, bool isTitle = false)
        {
            Size = control.Size;

            FontFamily = theme.FontFamily;
            var lineSpacing = 0;
            if (isTitle)
            {
                FontSize = (byte)theme.TitleFontSize;
                TextAlign = (byte)theme.TitleAlign;
                lineSpacing = theme.TitleSpacing;
            } else
            {
                FontSize = (byte)theme.FontSize;
                lineSpacing = theme.LineSpacing;
            }
            FontWeight = (byte)theme.FontWeight;
            Underline = theme.Underline;
            Padding = new(theme.PaddingLeft, theme.PaddingTop, theme.PaddingRight, theme.PaddingBottom);
            Spacing = new(theme.LetterSpacing, lineSpacing);
        }
    }
}
