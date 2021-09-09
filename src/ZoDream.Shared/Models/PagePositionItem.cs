using System;
using System.Collections.Generic;
using System.Text;

namespace ZoDream.Shared.Models
{
    public class PagePositionItem
    {
        public PositionItem Begin { get; set; }

        public PositionItem End {  get; set; }

        public double Length
        {
            get
            {
                return End.Position - Begin.Position;
            }
        }
    }
}
