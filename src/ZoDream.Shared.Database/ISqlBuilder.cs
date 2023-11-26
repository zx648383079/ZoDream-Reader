using System;
using System.Collections.Generic;
using System.Text;

namespace ZoDream.Shared.Database
{
    public interface ISqlBuilder<T>
    {

        public ISqlBuilder<T> From();

        public ISqlBuilder<T> Where();
        public ISqlBuilder<T> Select();
        public ISqlBuilder<T> Limit();
        public ISqlBuilder<T> OrderBy();
        public ISqlBuilder<T> GroupBy();

        public List<T> ToList();
        public T[] ToArray();
        public Page<T> Page();
    }
}
