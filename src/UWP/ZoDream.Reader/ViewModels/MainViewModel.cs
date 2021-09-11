using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Controls;
using ZoDream.Reader.Models;

namespace ZoDream.Reader.ViewModels
{
    public class MainViewModel: BindableBase
    {
        internal Frame RootFrame;
        internal Frame ChildFrame;

        private ObservableCollection<BookItem> bookItems = new ObservableCollection<BookItem>();

        public ObservableCollection<BookItem> BookItems
        {
            get => bookItems;
            set => Set(ref bookItems, value);
        }

        public void Load(StorageFile file)
        {
            Load(file.Path);
        }

        public void Load(IEnumerable<StorageFile> files)
        {
            foreach (var item in files)
            {
                Load(item);
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
            if (string.IsNullOrWhiteSpace(fileName))
            {
                return;
            }
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

        public void Navigate(Type page, object parameter = null, bool isRoot = false)
        {
            if (isRoot)
            {
                RootFrame.Navigate(page, parameter);
                return;
            }
            if (ChildFrame == null)
            {
                RootFrame.Navigate(typeof(MainPage), parameter);
            }
            ChildFrame?.Navigate(page, parameter);
        }
    }
}
