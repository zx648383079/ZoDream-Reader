using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ZoDream.Shared.Models;

namespace ZoDream.Shared.Interfaces
{
    public interface ICharIterator: IDisposable
    {

        public long Position { get; }

        public Task SeekAsync(long position);

        public Task<string?> ReadLineAsync();

        public Task<ReadLineItem> ReadLineAsync(long position);

    }
}
