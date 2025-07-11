using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZoDream.Shared.Interfaces;

namespace ZoDream.Shared.Models
{
    public class NovelBasic : INovelBasic
    {
        public NovelBasic()
        {
            
        }
        public NovelBasic(string name)
        {
            Name = name;
        }

        public string Name { get; set; } = string.Empty;
        public byte Rating { get; set; }

        public string Author { get; set; } = string.Empty;

        public Stream? Cover { get; set; }

        public string Brief { get; set; } = string.Empty;
    }
}
