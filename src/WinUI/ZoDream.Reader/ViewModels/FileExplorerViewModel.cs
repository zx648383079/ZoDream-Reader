using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using ZoDream.Shared.Interfaces;
using ZoDream.Shared.Interfaces.Route;

namespace ZoDream.Reader.ViewModels
{
    public class FileExplorerViewModel : ObservableObject, IQueryAttributable
    {

        private StorageFolder? _baseFolder;
        private StorageFolder? _currentFolder;

        private ObservableCollection<StorageFolder> _routeItems = [];

        public ObservableCollection<StorageFolder> RouteItems {
            get => _routeItems;
            set => SetProperty(ref _routeItems, value);
        }


        private AsyncObservableCollection<NovelSourceViewModel> _items = [];

        public AsyncObservableCollection<NovelSourceViewModel> Items {
            get => _items;
            set => SetProperty(ref _items, value);
        }

        public async void ApplyQueryAttributes(IDictionary<string, object> queries)
        {
            if (!queries.TryGetValue("folder", out var arg) || arg is not StorageFolder folder)
            {
                return;
            }
            _currentFolder = _baseFolder = folder;
            RouteItems.Clear();
            await LoadAsync();
        }

        private async Task LoadAsync()
        {
            if (_currentFolder is null)
            {
                return;
            }
            await LoadAsync(_currentFolder);
        }

        private async Task LoadAsync(StorageFolder folder)
        {
            Items.Clear();
            Items.Start();
            if (folder != _baseFolder)
            {
                Items.Add(new NovelSourceViewModel()
                {
                    Name = "上一级",
                    IsDirectory = true,
                    Source = await folder.GetParentAsync()
                });
            }
            foreach (var item in await folder.GetFoldersAsync())
            {
                Items.Add(new NovelSourceViewModel()
                {
                    Name = item.Name,
                    IsDirectory = true,
                    Source = item
                });
            }
            foreach (var item in await folder.GetFilesAsync())
            {
                Items.Add(new NovelSourceViewModel()
                {
                    Name = item.Name,
                    Tag = item.FileType.Length > 1 ? item.FileType[1..].ToUpper() : string.Empty,
                    IsDirectory = false,
                    CreateTime = item.DateCreated.LocalDateTime,
                    Source = item
                });
            }
            Items.Stop();
        }

        private Task<INovelBasic?> LoadAsync(StorageFile file)
        {
            switch (file.FileType)
            {
                case ".txt":
                case ".epub":
                case ".npk":
                case ".umd":

                default:
                    return null;
            }
        }
    }
}
