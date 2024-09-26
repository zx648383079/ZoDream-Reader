using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using ZoDream.Shared.Interfaces;
using ZoDream.Shared.Interfaces.Entities;

namespace ZoDream.Shared.Plugins.Pdf
{
    public class PdfReader : INovelReader
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }


        public (INovel?, List<INovelChapter>) GetChapters(Stream input)
        {
            throw new NotImplementedException();
        }

        public Task<List<INovelChapter>> GetChaptersAsync(string fileName)
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

        INovelDocument INovelReader.GetChapter(Stream input, INovelChapter chapter)
        {
            throw new NotImplementedException();
        }

        Task<INovelDocument> INovelReader.GetChapterAsync(string fileName, INovelChapter chapter)
        {
            throw new NotImplementedException();
        }
    }
}
