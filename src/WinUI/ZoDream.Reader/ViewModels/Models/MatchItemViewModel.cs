using ZoDream.Shared.Interfaces;

namespace ZoDream.Reader.ViewModels
{
    public class MatchItemViewModel
    {
        /// <summary>
        /// 匹配到的内容
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// 在正文当中的开始位置
        /// </summary>
        public int Offset { get; set; }
        /// <summary>
        /// 章节标题
        /// </summary>
        public string Header { get; set; }
        /// <summary>
        /// 章节的编号
        /// </summary>
        public int Index { get; set; }

        public INovelTextBlock Source { get; set; }
    }
}
