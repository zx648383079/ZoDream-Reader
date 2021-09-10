using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace ZoDream.Shared.Models
{
    public class PageItem: PagePositionItem, IEnumerable<CharItem>
    {
        public IList<CharItem> Data { get; set; }

        public double Left { get; set; } = 0;

        public double Top { get; set; } = 0;


        public IEnumerator<CharItem> GetEnumerator()
        {
            return Data.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)Data).GetEnumerator();
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var item in Data)
            {
                sb.Append(item.Code);
            }
            return sb.ToString();
        }
    }
}
