using System;
using System.Collections.Generic;
using System.Text;

namespace ZoDream.Shared.Models
{
    public class PositionItem
    {
        public long Position { get; set; }

        public int Offset { get; set; }

        public PositionItem(long position = 0, int offset = 0)
        {
            Position = position;
            Offset = offset;
        }

    }
}
