using System;
using System.Collections.Generic;
using System.Text;
using ZoDream.Shared.Database;
using ZoDream.Shared.Interfaces.Entities;
using ZoDream.Shared.ViewModels;

namespace ZoDream.Shared.Repositories.Models
{
    public class ReadThemeModel: BindableBase, IReadTheme
    {
        private string name = string.Empty;

        public string Name {
            get => name;
            set => Set(ref name, value);
        }

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
        public string DarForeground { get; set; } = string.Empty;

        private bool isEnabled;

        public bool IsEnabled {
            get => isEnabled;
            set => Set(ref isEnabled, value);
        }

        private bool isChecked;

        public bool IsChecked {
            get => isChecked;
            set => Set(ref isChecked, value);
        }
    }
}
