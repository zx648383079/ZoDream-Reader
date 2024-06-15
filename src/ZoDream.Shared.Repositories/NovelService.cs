using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ZoDream.Shared.Interfaces;
using ZoDream.Shared.Interfaces.Entities;
using ZoDream.Shared.Interfaces.Tokenizers;
using ZoDream.Shared.Repositories.Entities;

namespace ZoDream.Shared.Repositories
{
    public class NovelService(IDatabaseRepository database, BookEntity book) : INovelService, ICanvasSource
    {
        private IList<string> _cacheChapters = [];
        private IList<string> _cachePages = [];
        public bool IsLoading { get; private set; }

        public INovelPage? Current { get; }

        public void Resize(double width)
        {

        }
        public void Resize(double width, double height)
        {

        }

        public Task<bool> ReadNextAsync()
        {
        }

        private void LoadChapter()
        {

        }

        protected virtual async Task<List<INovelChapter>> GetChaptersAsync()
        {
            return await database.GetChapterAsync<ChapterEntity>(book.Id);
        }
    }
}
