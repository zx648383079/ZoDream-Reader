using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ZoDream.Shared.Interfaces
{
    public interface ICache: IDisposable
    {
        public Task SetAsync<T>(string key, T value);

        public Task SetImageAsync(string key, byte[] value);

        public Task<T?> GetAsync<T>(string key);

        public Task<bool> HasAsync(string key);

        public Task RemoveAsync(params string[] keys);
    }
}
