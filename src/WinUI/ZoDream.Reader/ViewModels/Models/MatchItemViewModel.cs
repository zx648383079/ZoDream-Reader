using ZoDream.Shared.Interfaces;

namespace ZoDream.Reader.ViewModels
{
    public class MatchItemViewModel
    {
        /// <summary>
        /// 匹配到的内容
        /// </summary>
        public string MatchText => Source.Text[MatchBegin..MatchEnd];
        /// <summary>
        /// 在正文当中的开始位置
        /// </summary>
        public int MatchBegin { get; private set; }
        public int MatchLength { get; private set; }
        public int MatchEnd => MatchBegin + MatchLength;
        /// <summary>
        /// 章节标题
        /// </summary>
        public string Header { get; set; } = string.Empty;

        public int SectionIndex { get; set; }
        public int LineIndex { get; set; }

        public INovelSection? Section { get; set; }

        public INovelTextBlock Source { get; private set; }


        public MatchItemViewModel(INovelTextBlock source, int index, int length)
        {
            Source = source;
            MatchBegin = index;
            MatchLength = length;
        }
    }
}
