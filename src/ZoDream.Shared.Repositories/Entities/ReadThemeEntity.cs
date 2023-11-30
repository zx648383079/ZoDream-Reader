using System;
using System.Collections.Generic;
using System.Text;
using ZoDream.Shared.Database;
using ZoDream.Shared.Interfaces.Entities;

namespace ZoDream.Shared.Repositories.Entities
{
    [TableName("read_themes")]
    [PrimaryKey("Id", AutoIncrement = true)]
    public class ReadThemeEntity: IReadTheme
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public string FontFamily { get; set; } = string.Empty;
        public int FontSize { get; set; } = 16;

        public int FontWeight { get; set; } = 500;

        public bool Underline { get; set; }

        public int PaddingTop { get; set; }
        public int PaddingLeft { get; set; }
        public int PaddingRight { get; set; }
        public int PaddingBottom { get; set; }

        public int LineSpacing { get; set; }
        public int LetterSpacing { get; set; }

        public int TitleFontSize { get; set; }
        public int TitleSpacing { get; set; }

        public int TitleAlign { get; set; }

        public int ParagraphSpacing { get; set; }
        public int ParagraphIndent { get; set; } = 4;

        public string BackgroundImage { get; set; } = string.Empty;
        public string Background { get; set; } = string.Empty;
        public string Foreground { get; set; } = string.Empty;

        public string DarkBackgroundImage { get; set; } = string.Empty;
        public string DarkBackground { get; set; } = string.Empty;
        public string DarkForeground { get; set; } = string.Empty;

        public string EInkBackgroundImage { get; set; } = string.Empty;
        public string EInkBackground { get; set; } = string.Empty;
        public string EInkForeground { get; set; } = string.Empty;
    }
}
