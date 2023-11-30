using System;
using System.Collections.Generic;
using System.Text;

namespace ZoDream.Shared.Interfaces.Entities
{
    public interface IRuleItem
    {
        public int Id { get; set; }
        public string Name { get; set; }



        public bool IsEnabled { get; set; }

        public int SortOrder { get; set; }
    }
}
