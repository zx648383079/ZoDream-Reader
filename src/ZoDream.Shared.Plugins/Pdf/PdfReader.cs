using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using ZoDream.Shared.Interfaces;
using ZoDream.Shared.Interfaces.Entities;
using ZoDream.Shared.Tokenizers;

namespace ZoDream.Shared.Plugins.Pdf
{
    public class PdfReader : INovelReader
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public INovelSource CreateSource(INovelSourceEntity entry)
        {
            return new FileSource(entry);
        }

        public Task<INovelDocument> GetChapterAsync(INovelSource source, INovelChapter chapter)
        {
            throw new NotImplementedException();
        }

        public Task<INovelChapter[]> GetChaptersAsync(INovelSource source)
        {
            throw new NotImplementedException();
        }

        public Task<(INovel?, INovelChapter[])> LoadAsync(INovelSource source)
        {
            throw new NotImplementedException();
        }

        public string Serialize(INovelChapter chapter)
        {
            throw new NotImplementedException();
        }

        public INovelChapter UnSerialize(string data)
        {
            throw new NotImplementedException();
        }
    }
}
