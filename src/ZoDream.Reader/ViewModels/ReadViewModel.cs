using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using ZoDream.Shared.Interfaces;
using ZoDream.Shared.Interfaces.Entities;
using ZoDream.Shared.Interfaces.Tokenizers;
using ZoDream.Shared.Models;
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
                _novel.CurrentChapterIndex = (int)(value * 10000);
            }
        }

        public string NovelId => _novel.Id;

        public double Width { get; internal set; }

        public double Height { get; internal set; }


        public async Task<INovelReader> GetReaderAsync()
        {
            switch (_novel.Type)
            {
                case 2:
                    return new NetReader();
                case 1:
                    return new EPubReader();
                default:
                    return new TxtReader();
            }
        }

        public async Task<IPageTokenizer> GetTokenizerAsync(INovelDocument document)
        {
            if (document is HtmlDocument)
            {
                return new HtmlTokenizer();
            }
            return new TextTokenizer();
        }

        public async Task<IList<INovelPage>> PageParseAsync(INovelDocument document)
        {
            var tokenizer = await GetTokenizerAsync(document);
            return tokenizer.Parse(document, _app.ReadTheme, this);
        }

        public Task<IList<INovelChapter>> LoadChaptersAsync()
        {
            return Task.FromResult(ChapterItems as IList<INovelChapter>);
        }

        public async Task<INovelDocument> GetChapterAsync(int chapterId)
        {
            var reader = await GetReaderAsync();
            return await reader.GetChapterAsync(_novel.FileName, ChapterItems.Where(i => i.Id == chapterId).First());
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

    }
}
