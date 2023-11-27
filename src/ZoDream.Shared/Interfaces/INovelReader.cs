using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ZoDream.Shared.Interfaces.Entities;

namespace ZoDream.Shared.Interfaces
{
    public interface INovelReader: IDisposable
    {
        public Task<INovelChapter> GetChaptersAsync();

        public Task<string> GetChapter(INovelChapter chapter);

        public string Serialize(INovelChapter chapter);
        public INovelChapter UnSerialize(string data);
    }
}
