using System;
using System.Threading.Tasks;
using ZoDream.Shared.Interfaces.Entities;

namespace ZoDream.Shared.Interfaces
{
    public interface INovelSerializer: IDisposable
    {

        public INovelSource CreateSource(INovelSourceEntity entry);
        public Task<(INovel?, INovelChapter[])> LoadAsync(INovelSource source);
        public Task<INovelChapter[]> GetChaptersAsync(INovelSource source);

        public Task<ISectionSource> GetChapterAsync(INovelSource source, INovelChapter chapter);

        public string Serialize(INovelChapter chapter);
        public INovelChapter UnSerialize(string data);
    }
}
