using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ZoDream.Shared.Interfaces.Entities;

namespace ZoDream.Shared.Interfaces
{
    public interface INovelReader: IDisposable
    {
        public Task<List<INovelChapter>> GetChaptersAsync(string fileName);

        public Task<string> GetChapter(string fileName, INovelChapter chapter);

        public string Serialize(INovelChapter chapter);
        public INovelChapter UnSerialize(string data);
    }
}
