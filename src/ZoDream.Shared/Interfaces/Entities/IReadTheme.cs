using System;
using System.Collections.Generic;
using System.Text;

namespace ZoDream.Shared.Interfaces.Entities
{
    public interface IReadTheme
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string FontFamily { get; set; }
        public int FontSize { get; set; }
        /// <summary>
        /// 字体加粗，500 正常
        /// </summary>
        public int FontWeight { get; set; }

        public bool Underline { get; set; }
        public int PaddingTop { get; set; }
        public int PaddingLeft { get; set; }
        public int PaddingRight { get; set; }
        public int PaddingBottom { get; set; }

        public int LineSpacing {  get; set; }
        public int LetterSpacing {  get; set; }

        public int TitleFontSize { get; set; }

        public int TitleSpacing { get; set; }
        /// <summary>
        /// 对齐，0 居左 1 居中 3 隐藏
        /// </summary>
        public int TitleAlign { get; set; }

        public int ParagraphSpacing { get; set; }
        public int ParagraphIndent { get; set; }

        public string BackgroundImage { get; set; }
        public string Background { get; set; }
        public string Foreground { get; set; }

        public string DarkBackgroundImage { get; set; }
        public string DarkBackground { get; set; }
        public string DarkForeground { get; set; }

        public string EInkBackgroundImage { get; set; }
        public string EInkBackground { get; set; }
        public string EInkForeground { get; set; }

    }
}
