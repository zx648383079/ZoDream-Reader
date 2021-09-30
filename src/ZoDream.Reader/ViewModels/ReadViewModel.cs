﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZoDream.Shared.Interfaces;
using ZoDream.Shared.Models;
using ZoDream.Shared.Renders;
using ZoDream.Shared.Tokenizers;

namespace ZoDream.Reader.ViewModels
{
    public class ReadViewModel: BindableBase, ICanvasSource
    {

        public PageTokenizer Tokenizer { get; private set; } = new PageTokenizer();

        private BookItem book;

        public BookItem Book
        {
            get => book;
            set
            {
                Set(ref book, value);
                if (Tokenizer.Content != null)
                {

                }
                if (value == null)
                {
                    return;
                }
                ApplyTokenizer();
            }
        }

        private async void ApplyTokenizer()
        {
            Tokenizer.Content = new StreamIterator(await App.ViewModel.DiskRepository.GetBookPathAsync(book));
        }

        private string chapterTitle = string.Empty;

        public string ChapterTitle
        {
            get => chapterTitle;
            set => Set(ref chapterTitle, value);
        }

        private ObservableCollection<ChapterPositionItem> chapterItems = new ObservableCollection<ChapterPositionItem>();

        public ObservableCollection<ChapterPositionItem> ChapterItems
        {
            get => chapterItems;
            set => Set(ref chapterItems, value);
        }

        public void Load()
        {
            Task.Factory.StartNew(async () =>
            {
                var items = await Tokenizer.GetChaptersAsync();
                if (items == null)
                {
                    return;
                }
                App.Current.Dispatcher.Invoke(() =>
                {
                    foreach (var item in items)
                    {
                        if (item == null)
                        {
                            continue;
                        }
                        ChapterItems.Add(item);
                    }
                    ReloadChapter();
                });
                
            });
        }

        public void ReloadChapter()
        {
            var i = Tokenizer.GetChapter(ChapterItems, Book.Position);
            if (i < 0)
            {
                return;
            }
            ChapterTitle = ChapterItems[i].Title;
        }

        public Task<IList<PageItem>> GetAsync(int page)
        {
            return Tokenizer.GetAsync(page);
        }

        public bool Canable(int page)
        {
            return Tokenizer.Canable(page);
        }
    }
}
