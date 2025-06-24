using System.Collections.Generic;
using System.IO;
using System.Linq;
using ZoDream.Shared.Interfaces;

namespace ZoDream.Shared.Tokenizers
{
    public class RichDocument : INovelDocument
    {
        public RichDocument()
        {
            
        }
        public RichDocument(string name)
        {
            Name = name;
        }
        public string Name { get; set; } = string.Empty;

        public string Author { get; set; } = string.Empty;

        public Stream? Cover { get; set; }

        public string Brief { get; set; } = string.Empty;

        public IList<INovelVolume> Items { get; private set; } = [];

        public void Add(IEnumerable<INovelSection> items)
        {
            if (Items.Count == 0)
            {
                Items.Add(new NovelVolume(string.Empty));
            }
            var target = Items.Last();
            foreach (var item in items)
            {
                target.Add(item);
            }
        }
        public void Add(INovelSection section)
        {
            if (Items.Count == 0)
            {
                Items.Add(new NovelVolume(string.Empty));
            }
            Items.Last().Add(section);
        }
    }

    public class NovelVolume(string name): List<INovelSection>, INovelVolume
    {
        public string Name => name;
    }

    public class NovelSection(string title) : INovelSection
    {
        public NovelSection(string title, IEnumerable<INovelBlock> items)
            : this (title)
        {
            foreach (var item in items)
            {
                Items.Add(item);
            }
        }

        public string Title => title;

        public IList<INovelBlock> Items { get; private set; } = [];
    }

    public class NovelTextBlock(string text) : INovelTextBlock
    {
        public string Text => text;
    }

    public class NovelImageBlock(Stream source) : INovelImageBlock
    {
        public Stream Source => source;
    }
}
