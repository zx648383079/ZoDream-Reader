using System;
using System.Collections.Generic;
using System.Text;

namespace ZoDream.Shared.Repositories.Entities
{
    public interface INovelEntity
    {
        public string Name { get; }

        public string Cover { get; }

        public string Description { get; }

        public string Author { get; }
    }
}
