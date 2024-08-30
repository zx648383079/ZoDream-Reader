using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ZoDream.Shared.Interfaces.Entities;

namespace ZoDream.Shared.Interfaces
{
    public interface INovelReader: IDisposable
    {
        public Task<List<INovelChapter>> GetChaptersAsync(string fileName);

        public Task<INovelDocument> GetChapterAsync(string fileName, INovelChapter chapter);

        public (INovel?, List<INovelChapter>) GetChapters(Stream input);

        public INovelDocument GetChapter(Stream input, INovelChapter chapter);

        public string Serialize(INovelChapter chapter);
        public INovelChapter UnSerialize(string data);
    }
}
