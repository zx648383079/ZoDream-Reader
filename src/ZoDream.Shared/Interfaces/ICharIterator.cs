using System;
using System.Collections.Generic;
using System.Text;

namespace ZoDream.Shared.Interfaces
{
    public interface ICharIterator: IDisposable
    {

        public long Position { get; set; }

        public string? ReadLine();

    }
}
