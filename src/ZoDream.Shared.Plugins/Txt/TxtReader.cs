using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ZoDream.Shared.Interfaces;

namespace ZoDream.Shared.Plugins.Txt
{
    public class TxtReader : INovelReader
    {


        public Task<INovelChapter> GetChaptersAsync()
        {
            throw new NotImplementedException();
        }

        public Task<string> GetChapter(INovelChapter chapter)
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

        public void Dispose()
        {
            throw new NotImplementedException();
        }


    }
}
