using System;
using System.Collections.Generic;
using System.Text;
using ZoDream.Shared.Interfaces;

namespace ZoDream.Shared.Models
{
    public class AppOption: IAppOption
    {
        public int FontSize { get; set; } = 18;

        public string FontFamily { get; set; } = string.Empty;

        public string Background { get; set; } = "#fff";

        public string BackgroundImage { get; set; } = string.Empty;

        public string Foreground { get; set; } = "#333";

        /// <summary>
        /// 开启夜间模式
        /// </summary>
        public bool OpenDark { get; set; } = false;

        public int ColumnCount { get; set; } = 2;

        public int LineSpace { get; set; } = 5;

        public int LetterSpace { get; set; } = 5;

        public int Padding { get; set; } = 10;

        public bool IsSimple { get; set; } = true;

        public int Animation { get; set; } = 0;

        public bool AutoFlip { get; set; } = false;

        public float FlipSpace { get; set; } = 100;

        public bool OpenSpeak { get; set; } = false;

        public float SpeakSpeed { get; set; } = 1;


        public int ColumnMaxWidth { get; set; } = 700;

        public int AppTheme { get; set; }

        public int ReadTheme { get; set; }
    }
}
