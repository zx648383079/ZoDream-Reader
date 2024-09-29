﻿using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using ZoDream.Shared.Interfaces;
using ZoDream.Shared.Interfaces.Entities;
using ZoDream.Shared.Interfaces.Tokenizers;
using ZoDream.Shared.Models;
using ZoDream.Shared.Plugins;
using ZoDream.Shared.Plugins.EPub;
using ZoDream.Shared.Plugins.Net;
using ZoDream.Shared.Plugins.Txt;
using ZoDream.Shared.Renders;
using ZoDream.Shared.Repositories.Entities;
using ZoDream.Shared.Tokenizers;

namespace ZoDream.Reader.ViewModels
{
    public class ReadViewModel: ObservableObject, INovelEnvironment
    {
        public ReadViewModel()
        {
            _app = App.GetService<AppViewModel>();
        }

        private readonly AppViewModel _app;
        private BookEntity _novel;
        private INovelSource? _source;
        private INovelReader? _reader;

        private string chapterTitle = string.Empty;

        public string ChapterTitle
        {
            get => chapterTitle;
            set => SetProperty(ref chapterTitle, value);
        }

        private ObservableCollection<ChapterModel> chapterItems = [];

        public ObservableCollection<ChapterModel> ChapterItems
        {
            get => chapterItems;
            set => SetProperty(ref chapterItems, value);
        }

        public int ChapterIndex { 
            get {
                return _novel.CurrentChapterIndex;
            }
            set {
                _novel.CurrentChapterIndex = value;
            } 
        }

        public double ChapterProgresss {
            get {
                return _novel.CurrentChapterOffset / 10000;
            }
            set {
                _novel.CurrentChapterOffset = (int)(value * 10000);
            }
        }

        public string NovelId => _novel.Id;

        public double Width { get; internal set; }

        public double Height { get; internal set; }


        public Task<INovelReader> GetReaderAsync()
        {
            return Task.FromResult(_reader ??= ReaderFactory.GetReader(_novel));
        }

        public Task<IPageTokenizer> GetTokenizerAsync(INovelDocument document)
        {
            return Task.FromResult(ReaderFactory.GetTokenizer(document));
        }

        public async Task<IList<INovelPage>> PageParseAsync(INovelDocument document)
        {
            var tokenizer = await GetTokenizerAsync(document);
            return tokenizer.Parse(document, _app.ReadTheme, this);
        }

        public Task<INovelChapter[]> LoadChaptersAsync()
        {
            return Task.FromResult(ChapterItems.Select(i => (INovelChapter)i).ToArray());
        }

        public async Task<INovelDocument> GetChapterAsync(int chapterId)
        {
            
            var reader = await GetReaderAsync();
            if (_source is null)
            {
                _source = reader.CreateSource(_novel);
                if (_source is FileSource s)
                {
                    s.FileName = await _app.Storage.GetBookPathAsync(_novel);
                }
            }
            return await reader.GetChapterAsync(_source, 
                ChapterItems.Where(i => i.Id == chapterId).First());
        }

        public Task<IReadTheme> GetReadThemeAsync()
        {
            return Task.FromResult(_app.ReadTheme);
        }

        public async Task LoadAsync(INovel novel)
        {
            if (novel is BookEntity e)
            {
                _novel = e;
            } else
            {
                _novel = await _app.Database.GetBookAsync<BookEntity>(novel.Id);
            }
            var items = await _app.Database.GetChapterAsync<ChapterModel>(NovelId);
            foreach (var item in items)
            {
                ChapterItems.Add(item);
            }
            ChapterTitle = ChapterItems[_novel.CurrentChapterIndex].Title;
        }

        public void GotoChapter(int index)
        {
            var chapter = ChapterItems[index];
            ChapterTitle = chapter.Title;
            ChapterIndex = index;
            ChapterProgresss = 0;
        }
    }
}
