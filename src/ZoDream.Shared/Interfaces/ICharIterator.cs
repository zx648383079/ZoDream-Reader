using System;
using System.Threading.Tasks;
using ZoDream.Shared.Models;

namespace ZoDream.Shared.Interfaces
{
    public interface ICharIterator: IDisposable
    {

        public long Position { get; }

        public long Seek(long position);
        public string? ReadLine();
        public Task SeekAsync(long position);

        public Task<string?> ReadLineAsync();

        public Task<ReadLineItem> ReadLineAsync(long position);

    }
}
