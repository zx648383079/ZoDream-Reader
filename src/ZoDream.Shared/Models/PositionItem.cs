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

        public bool Equals(PositionItem target)
        {
            return Position == target.Position && Offset == target.Offset;
        }


        public override string ToString()
        {
            return $"{Position}>>{Offset}";
        }

        public static PositionItem operator +(PositionItem position, int offset)
        {
            if (position.Offset == 0 && offset > 0)
            {
                offset--;
            }
            return new PositionItem(position.Position, position.Offset + offset);
        }

        public static bool operator ==(PositionItem position, PositionItem con)
        {
            return position.Equals(con);
        }

        public static bool operator !=(PositionItem position, PositionItem con)
        {
            return !position.Equals(con);
        }
    }
}
