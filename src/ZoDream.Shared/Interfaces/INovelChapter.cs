using System;
using System.Collections.Generic;
using System.Text;

namespace ZoDream.Shared.Interfaces
{
    public interface INovelChapter
    {
        public string Title { get;}

        public string Description { get; }

        public DateTime PublishedAt { get; }

    }
}
