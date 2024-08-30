using ZoDream.Shared.Interfaces;

namespace ZoDream.Shared.Tokenizers
{
    public class XmlDocument(string title, string content) : TextDocument(title, content), INovelDocument
    {
    }
}
