using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZoDream.Reader.Models;
using ZoDream.Reader.Renders;
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
                Tokenizer.Content = new StreamIterator(value.FileName);
            }
        }


        private ObservableCollection<ChapterItem> chapterItems = new ObservableCollection<ChapterItem>();

        public ObservableCollection<ChapterItem> ChapterItems
        {
            get => chapterItems;
            set => Set(ref chapterItems, value);
        }

    }
}
