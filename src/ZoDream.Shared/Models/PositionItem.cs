using System;
using System.Collections.Generic;
using System.Text;

namespace ZoDream.Shared.Models
{
    public class PositionItem
    {
        public long Position { get; set; }

        public int Offset { get; set; }

        public PositionItem(string s)
        {
            var args = s.Split(',');
            Position = int.Parse(args[0]);
            Offset = args.Length > 1 ? int.Parse(args[1]) : 0;
        }

        public PositionItem(long position = 0, int offset = 0)
        {
            Position = position;
            Offset = offset;
        }

        public override bool Equals(object target)
        {
            if (target == null)
            {
                return false;
            }
            if (target is not PositionItem)
            {
                return false;
            }
            var t = target as PositionItem;
            return Position == t.Position && Offset == t.Offset;
        }


        public override string ToString()
        {
            return $"{Position},{Offset}";
        }

        public static PositionItem operator +(PositionItem position, int offset)
        {
            if (position.Offset == 0 && offset > 0)
            {
                offset--;
            }
            return new PositionItem(position.Position, position.Offset + offset);
        }

        public static bool operator ==(PositionItem? position, PositionItem? con)
        {
            if (position is null)
            {
                return con is null;
            }
            return position.Equals(con);
        }

        public static bool operator !=(PositionItem? position, PositionItem? con)
        {
            if (position is null)
            {
                return con is not null;
            }
            return !position.Equals(con);
        }

        public PositionItem Clone()
        {
            return new PositionItem(Position, Offset);
        }
    }
}
