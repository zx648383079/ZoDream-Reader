using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZoDream.Reader.Models;

namespace ZoDream.Reader.ViewModels
{
    public class MainViewModel: BindableBase
    {

        private ObservableCollection<BookItem> bookItems = new ObservableCollection<BookItem>();

        public ObservableCollection<BookItem> BookItems
        {
            get => bookItems;
            set => Set(ref bookItems, value);
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
            var info = new FileInfo(fileName);
            if (!info.Exists)
            {
                return;
            }
            BookItems.Add(new BookItem()
            {
                Name = info.Name.Replace(info.Extension, ""),
                FileName = fileName,
            });
        }
    }
}
