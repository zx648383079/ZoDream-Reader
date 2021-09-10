using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZoDream.Reader.Models
{
    public class ChapterItem
    {
        public string Title { get; set; }

        public int Position { get; set; } = 0;

        public int Length { get; set; } = 0;

        public int HistoryPosition { get; set; } = 0;
    }
}
