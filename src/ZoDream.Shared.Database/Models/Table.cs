using System;
using System.Collections.Generic;
using System.Text;

namespace ZoDream.Shared.Database.Models
{
    public class Table
    {
        public string Name { get; set; } = string.Empty;

        public string Comment { get; set; } = string.Empty;

        public IList<TableField> Items { get; set; } = new List<TableField>();
    }
}
