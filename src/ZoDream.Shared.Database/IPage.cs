using System;
using System.Collections.Generic;
using System.Text;

namespace ZoDream.Shared.Database
{
    public interface IPage<T>: IEnumerable<T>
    {
        public long CurrentPage { get; set; }
        public long TotalPages { get; }
        public long TotalItems { get; set; }
        public long PerPage { get; set; }
        public List<T> Items { get; set; }

        public void Add(T item);
    }
}
