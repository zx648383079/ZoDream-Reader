using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZoDream.Shared.Interfaces;
using ZoDream.Shared.Interfaces.Entities;
using ZoDream.Shared.Interfaces.Tokenizers;

namespace ZoDream.Shared.Repositories
{
    public class NovelService(
        INovelEnvironment environment) : INovelService, ICanvasSource
    {
        private IList<INovelChapter>? _chapterRecordItems;
        private IList<ISectionSource> _cacheChapters = [];
        private IList<INovelPage> _cachePages = [];
        private string _novelId = string.Empty;
        private int _recordIndex = -1;
        private int _cacheRecordIndex = -1;
        private readonly int _cacheCount = 10;
        private int _pageIndex = -1;

        public bool IsLoading { get; private set; }

        public INovelPage? Current => _pageIndex >= 0 && _pageIndex < _cachePages.Count ? _cachePages[_pageIndex] : null;

        public ICanvasAnimate Animator => environment.Animator;

        public async Task ReadyAsync(ICanvasRender render)
        {
            await environment.ReadyAsync(render);
            await InvalidateAsync();
            render.Invalidate();
        }

        public async Task InvalidateAsync()
        {
            if (_novelId != environment.NovelId)
            {
                _chapterRecordItems = null;
            }
            _recordIndex = environment.ChapterIndex;
            await LoadChapterAsync();
            _novelId = environment.NovelId;
            await ParsePageAsync();
            _pageIndex = (int)Math.Floor(environment.ChapterProgresss * _cachePages.Count);
        }

        public async Task<bool> ReadNextAsync()
        {
            if (_cachePages.Count == 0)
            {
                await LoadChapterAsync();
                await ParsePageAsync();
            }
            var pageIndex = _pageIndex + 1;
            if (pageIndex >= _cachePages.Count)
            {
                var recordIndex = _recordIndex + 1;
                if (recordIndex >= _chapterRecordItems!.Count)
                {
                    IsLoading = false;
                    return false;
                }
                _recordIndex = recordIndex;
                await LoadChapterAsync();
                await ParsePageAsync();
                pageIndex = 0;
            }
            _pageIndex = pageIndex;
            Sync();
            IsLoading = false;
            return true;
        }

        public async Task<bool> ReadPreviousAsync()
        {
            if (_cachePages.Count == 0)
            {
                await LoadChapterAsync();
                await ParsePageAsync();
            }
            var pageIndex = _pageIndex - 1;
            if (pageIndex < 0)
            {
                var recordIndex = _recordIndex - 1;
                if (recordIndex < 0)
                {
                    IsLoading = false;
                    return false;
                }
                _recordIndex = recordIndex;
                await LoadChapterAsync();
                await ParsePageAsync();
                pageIndex = 0;
            }
            _pageIndex = pageIndex;
            Sync();
            IsLoading = false;
            return true;
        }

        private void Sync()
        {
            environment.ChapterIndex = _recordIndex;
            environment.ChapterProgresss = _cachePages.Count == 0 ? 0 : (_pageIndex / _cachePages.Count);
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
            _cachePages = await environment.PageParseAsync(content);
            _pageIndex = -1;
        }

        private async Task LoadChapterAsync()
        {
            IsLoading = true;
            _chapterRecordItems ??= await GetChaptersAsync();
            if (_recordIndex < 0)
            {
                _recordIndex = environment.ChapterIndex;
            }
            var items = new ISectionSource[_cacheCount];
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
                var res = await environment.GetChapterAsync(_chapterRecordItems[n].Id);
                items[i] = res;
            }
            _cacheChapters = items;
            _cacheRecordIndex = begin;
        }

        protected virtual async Task<IList<INovelChapter>> GetChaptersAsync()
        {
            return await environment.LoadChaptersAsync();
        }
    }
}
