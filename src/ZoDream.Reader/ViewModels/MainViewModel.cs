using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZoDream.Reader.Models;
using ZoDream.Reader.Repositories;

namespace ZoDream.Reader.ViewModels
{
    public class MainViewModel: BindableBase
    {
        private Database database = new Database();
        private Disk disk = new Disk();

        private ObservableCollection<BookItem> bookItems = new ObservableCollection<BookItem>();

        public ObservableCollection<BookItem> BookItems
        {
            get => bookItems;
            set => Set(ref bookItems, value);
        }

        public void Load()
        {
            var items = database.GetBookList();
            foreach (var item in items)
            {
                BookItems.Add(item);
            }
        }
        public void Load(IEnumerable<string> fileNames)
        {
            foreach (var item in fileNames)
            {
                Load(item);
            }
        }

        public void Load(string fileName)
        {
            if (!disk.AddTxt(fileName, out var bookFile, out var Name))
            {
                return;
            }
            var item = new BookItem()
            {
                Name = Name,
                FileName = bookFile,
            };
            database.AddBook(item);
            BookItems.Add(item);
        }

        public string GetFileName(BookItem book)
        {
            return disk.TxtFileName(book.FileName);
        }
    }
}
