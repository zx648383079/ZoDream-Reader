using System.Collections.Generic;
using ZoDream.Shared.Interfaces;

namespace ZoDream.Shared.Tokenizers
{
    public class RichDocument(string title) : INovelDocument
    {

        public string Title => title;

        public IList<INodeLine> Nodes { get; private set; } = [];
    }

    public interface INodeLine
    {
    }

    public class TextNodeLine(string text) : INodeLine
    {
        public string Text => text;
    }

    public class ImageNodeLine : INodeLine
    {

    }
}
