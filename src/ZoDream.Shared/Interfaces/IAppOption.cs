namespace ZoDream.Shared.Interfaces
{
    public interface IAppOption
    {
        /// <summary>
        /// 开启夜间模式
        /// </summary>
        public bool OpenDark { get; set; }

        public int ColumnCount { get; set; }
        public int ColumnMaxWidth { get; set; }

        public bool IsSimple { get; set; }

        public int Animation { get; set; }

        public bool AutoFlip { get; set; }

        public float FlipSpace { get; set; }

        public bool OpenSpeak { get; set; }

        public float SpeakSpeed { get; set; }

        public int AppTheme { get; set; }

        public int ReadTheme { get; set; }
    }
}
