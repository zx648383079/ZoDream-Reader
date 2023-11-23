using System;
using System.Collections;
using System.Collections.Generic;

namespace ZoDream.Shared.Database
{
    public class Page<T> : IPage<T>
    {
        public long CurrentPage { get; set; }

        public long TotalPages => TotalItems <= 0 || PerPage <= 0 ? 0L : (long)Math.Ceiling((double)TotalItems / PerPage);

        public long TotalItems { get; set; }
        public long PerPage { get; set; }
        public List<T> Items { get; set; } = new();

        public void Add(T item)
        {
            Items.Add(item);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Items.GetEnumerator();
        }
    }
}
