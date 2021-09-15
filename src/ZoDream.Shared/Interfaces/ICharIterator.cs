using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ZoDream.Shared.Interfaces
{
    public interface ICharIterator: IDisposable
    {

        public long Position { get; set; }

        public Task<string?> ReadLineAsync();

        public Task<string?> ReadLineAsync(long position);

    }
}
