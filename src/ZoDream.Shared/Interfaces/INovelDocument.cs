using System.Collections.Generic;
using System.IO;

namespace ZoDream.Shared.Interfaces
{

    public interface INovelBasic
    {
        public string Name { get; }
        /// <summary>
        /// 评分 0 - 10
        /// </summary>
        public byte Rating { get; }
        public string Author { get; }
        public Stream? Cover { get; }
        public string Brief { get; }
    }

    public interface INovelDocument : INovelBasic
    {
        public IList<INovelVolume> Items { get; }
    }

    public interface INovelVolume : IList<INovelSection>
    {
        public string Name { get; }

    }

    public interface INovelSection
    {
        public string Title { get; }

        public IList<INovelBlock> Items { get; }
    }

    public interface INovelBlock
    {

    }

    public interface INovelTextBlock : INovelBlock
    {
        public string Text { get; }
    }

    public interface INovelImageBlock : INovelBlock
    {
        public Stream Source { get; }
    }
}
