using ZoDream.Shared.Interfaces;

namespace ZoDream.Shared.Tokenizers
{
    public class HtmlDocument(string title, string content) : TextDocument(title, content), INovelDocument
    {
    }
}
