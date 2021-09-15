using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZoDream.Shared.Models;

namespace ZoDream.Shared.Models
{
    public class BookItem
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string? Cover { get; set; }

        public string FileName { get; set; }

        public PositionItem Position { get; set; } = new PositionItem();
        public BookItem(string name, string fileName)
        {
            Name = name;
            FileName = fileName;
        }
    }
}
