using ZoDream.Shared.Interfaces;

namespace ZoDream.Shared.Tokenizers
{
    public class TextDocument(string title, string content): ISectionSource
    {
        public string Title { get; private set; } = title;
        public string Content { get; private set; } = content;
    }
}
