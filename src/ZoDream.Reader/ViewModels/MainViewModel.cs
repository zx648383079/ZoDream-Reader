using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZoDream.Reader.Repositories;
using ZoDream.Shared.Models;

namespace ZoDream.Reader.ViewModels
{
    public class MainViewModel: ObservableObject
    {
        public MainViewModel()
        {
            Initialize();
        }

        private async void Initialize()
        {
            var dbFile = await DiskRepository.CreateDatabaseAsync();
            Database.Initialize(dbFile);
            DatabaseRepository = new Database(dbFile);
            Load();
            Setting = await DatabaseRepository.LoadSettingAsync();
        }

        public Database DatabaseRepository { get; private set; }
        public Disk DiskRepository { get; private set; } = new Disk();

        private ObservableCollection<BookItem> bookItems = new ObservableCollection<BookItem>();

        public ObservableCollection<BookItem> BookItems
        {
            get => bookItems;
            set => SetProperty(ref bookItems, value);
        }

        public AppOption Setting { get; set; }

        public void Load()
        {
            var items = DatabaseRepository.GetBooks();
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
            var item = DiskRepository.AddTxt(fileName);
            if (item == null)
            {
                return;
            }
            DatabaseRepository.AddBook(item);
            BookItems.Add(item);
        }

        public void RemoveBook(BookItem item)
        {
            BookItems.Remove(item);
            DatabaseRepository.DeleteBook(item);
            _ = DiskRepository.DeleteBookAsync(item);
        }
    }
}
