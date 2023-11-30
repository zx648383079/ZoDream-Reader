using System;
using System.Collections.Generic;
using System.Text;

namespace ZoDream.Shared.Interfaces.Entities
{
    public interface INovelMark
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Remark { get; set; }
    }
}
