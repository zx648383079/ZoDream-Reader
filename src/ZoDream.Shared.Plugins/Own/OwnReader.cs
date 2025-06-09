using System.Threading.Tasks;
using ZoDream.Shared.Interfaces;
using ZoDream.Shared.Interfaces.Entities;
using ZoDream.Shared.Tokenizers;

namespace ZoDream.Shared.Plugins.Own
{
    public class OwnReader : INovelReader
    {
        public INovelSource CreateSource(INovelSourceEntity entry)
        {
            return new FileSource(entry);
        }

        public Task<INovelDocument> GetChapterAsync(INovelSource source, INovelChapter chapter)
        {
            throw new System.NotImplementedException();
        }

        public Task<INovelChapter[]> GetChaptersAsync(INovelSource source)
        {
            throw new System.NotImplementedException();
        }

        public Task<(INovel?, INovelChapter[])> LoadAsync(INovelSource source)
        {
            throw new System.NotImplementedException();
        }

        public string Serialize(INovelChapter chapter)
        {
            throw new System.NotImplementedException();
        }

        public INovelChapter UnSerialize(string data)
        {
            throw new System.NotImplementedException();
        }

        public void Dispose()
        {
        }
    }
}
