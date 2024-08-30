using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZoDream.Shared.Interfaces;
using ZoDream.Shared.Interfaces.Entities;
using ZoDream.Shared.Interfaces.Tokenizers;
using ZoDream.Shared.Repositories.Entities;

namespace ZoDream.Shared.Repositories
{
    public class NovelService(
        IDatabaseRepository database,
        BookEntity book) : INovelService, ICanvasSource, ICanvasControl
    {
        private INovelReader _reader;
        private IList<INovelChapter>? _chapterRecordItems;
        private IList<INovelDocument> _cacheChapters = [];
        private IList<INovelPage> _cachePages = [];
        private int _recordIndex = 0;
        private int _cacheRecordIndex = -1;
        private readonly int _cacheCount = 5;
        private int _chapterIndex = -1;
        private int _pageIndex = -1;
        private IReadTheme _theme;

        public double Width { get; private set; }
        public double Height { get; private set; }

        public bool IsLoading { get; private set; }

        public INovelPage? Current => _pageIndex >= 0 && _pageIndex < _cachePages.Count ? _cachePages[_pageIndex] : null;

        public void Resize(double width)
        {
            Width = width;
        }
        public void Resize(double width, double height)
        {
            Width = width;
            Height = height;
        }

        public async Task<bool> ReadNextAsync()
        {
            if (_cachePages.Count == 0)
            {
                await LoadChapterAsync();
                await ParsePageAsync();
            }
            _pageIndex++;
            if (_pageIndex >= _cachePages.Count)
            {
                _recordIndex++;
                if (_recordIndex >= _chapterRecordItems!.Count)
                {
                    IsLoading = false;
                    return false;
                }
                await LoadChapterAsync();
                await ParsePageAsync();
                _pageIndex = 0;
            }
            IsLoading = false;
            return true;
        }

        private async Task ParsePageAsync()
        {
            IsLoading = true;
            await LoadChapterAsync();
            var content = _cacheChapters[_recordIndex - _cacheRecordIndex];
            if (content is null)
            {
                _cachePages = [];
                _pageIndex = -1;
                return;
            }
            var tokenizer = await database.GetTokenizerAsync(content);
            _cachePages = tokenizer.Parse(content, _theme, this);
            _pageIndex = -1;
        }

        private async Task LoadChapterAsync()
        {
            IsLoading = true;
            _reader ??= await database.GetReaderAsync(book);
            _chapterRecordItems ??= await GetChaptersAsync();
            var items = new INovelDocument[_cacheCount];
            var begin = Math.Max(0, _recordIndex - _cacheCount / 3);
            if (begin == _cacheRecordIndex)
            {
                return;
            }
            for (var i = 0; i < items.Length; i++)
            {
                var n = begin + i;
                var j = n - _cacheRecordIndex;
                if (j >= 0 && j < _cacheChapters.Count)
                {
                    items[i] = _cacheChapters[j];
                    continue;
                }
                var res = await _reader.GetChapterAsync(book.FileName, _chapterRecordItems[n]);
                items[i] = res;
            }
            _cacheChapters = items;
            _cacheRecordIndex = begin;
        }

        

        protected virtual async Task<List<INovelChapter>> GetChaptersAsync()
        {
            var res = await database.GetChapterAsync<ChapterEntity>(book.Id);
            return res.Select(i => i as INovelChapter).ToList();
        }
    }
}
