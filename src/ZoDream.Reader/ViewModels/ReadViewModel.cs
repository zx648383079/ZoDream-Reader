using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZoDream.Reader.Models;
using ZoDream.Shared.Models;
using ZoDream.Shared.Renders;
using ZoDream.Shared.Tokenizers;

namespace ZoDream.Reader.ViewModels
{
    public class ReadViewModel: BindableBase
    {

        public PageTokenizer Tokenizer { get; private set; } = new PageTokenizer();

        private BookItem book;

        public BookItem Book
        {
            get => book;
            set {
                Set(ref book, value);
                if (Tokenizer.Content != null)
                {

                }
                if (value == null)
                {
                    return;
                }
                Tokenizer.Content = new StreamIterator(App.ViewModel.GetFileName(book));
            }
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
            Task.Factory.StartNew(() =>
            {
                var items = Tokenizer.GetChapters();
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
    }
}
