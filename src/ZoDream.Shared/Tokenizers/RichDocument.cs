using System.Collections.Generic;
using System.IO;
using System.Linq;
using ZoDream.Shared.Interfaces;
using ZoDream.Shared.Models;

namespace ZoDream.Shared.Tokenizers
{
    public class RichDocument : NovelBasic, INovelDocument
    {
        public RichDocument()
        {
            
        }
        public RichDocument(string name)
            : base(name)
        {
        }

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

        public void Add(INovelVolume volume)
        {
            Items.Add(volume);
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
                if (item is INovelTextBlock t && string.IsNullOrWhiteSpace(t.Text))
                {
                    continue;
                }
                Items.Add(item);
            }
        }

        public string Title => title;

        public IList<INovelBlock> Items { get; private set; } = [];

    }

    public class NovelTextBlock(string text) : INovelTextBlock
    {
        public string Text => text;

        public override string ToString()
        {
            return Text;
        }
    }

    public class NovelImageBlock(Stream source) : INovelImageBlock
    {
        public Stream Source => source;
    }
}
