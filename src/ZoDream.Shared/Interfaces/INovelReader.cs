using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ZoDream.Shared.Interfaces.Entities;

namespace ZoDream.Shared.Interfaces
{
    public interface INovelReader: IDisposable
    {

        public INovelSource CreateSource(INovelSourceEntity entry);
        public Task<(INovel?, INovelChapter[])> LoadAsync(INovelSource source);
        public Task<INovelChapter[]> GetChaptersAsync(INovelSource source);

        public Task<INovelDocument> GetChapterAsync(INovelSource source, INovelChapter chapter);

        public string Serialize(INovelChapter chapter);
        public INovelChapter UnSerialize(string data);
    }
}
