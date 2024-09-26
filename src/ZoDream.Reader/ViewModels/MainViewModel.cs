using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using ZoDream.Reader.Events;
using ZoDream.Reader.Pages;
using ZoDream.Shared.Interfaces.Entities;
using ZoDream.Shared.Repositories.Entities;

namespace ZoDream.Reader.ViewModels
{
    public class MainViewModel: ObservableObject
    {
        public MainViewModel()
        {
            AddCommand = new RelayCommand(TapAdd);
            DragCommand = new RelayCommand<IEnumerable<string>>(TapDrag);
            ActionCommand = new RelayCommand<ActionHanlderArgs>(TapAction);
            LoadAsync();
        }

        private readonly AppViewModel _app = App.GetService<AppViewModel>();

        private ObservableCollection<BookEntity> novelItems = [];

        public ObservableCollection<BookEntity> NovelItems
        {
            get => novelItems;
            set => SetProperty(ref novelItems, value);
        }
        public ICommand AddCommand { get; private set; }
        public ICommand DragCommand { get; private set; }
        public ICommand ActionCommand { get; private set; }

        private void TapDrag(IEnumerable<string>? items)
        {
            if (items is null)
            {
                return;
            }
            _ = LoadAsync(items);
        }
        private void TapAction(ActionHanlderArgs? arg)
        {
            if (arg is null)
            {
                return;
            }
            if (arg.Action == ActionEvent.CLICK)
            {
                var page = new ReadView(arg.Source);
                page.ShowDialog();
                return;
            }
            if (arg.Action == ActionEvent.DELETE)
            {
                RemoveAsync(arg.Source);
                return;
            }
        }

        private void TapAdd()
        {
            var open = new Microsoft.Win32.OpenFileDialog
            {
                Multiselect = true,
                Filter = "文本文件|*.txt|所有文件|*.*",
                Title = "选择文件"
            };
            if (open.ShowDialog() != true)
            {
                return;
            }
            _ = LoadAsync(open.FileNames);
        }

        public async void LoadAsync()
        {
            NovelItems.Clear();
            var items = await _app.Database.GetBookAsync<BookEntity>();
            foreach (var item in items)
            {
                NovelItems.Add(item);
            }
        }
        public async Task LoadAsync(IEnumerable<string> fileNames)
        {
            foreach (var item in fileNames)
            {
                await _app.Storage.AddBookAsync(item);
            }
            LoadAsync();
        }


        public async void RemoveAsync(INovel item)
        {
            for (int i = NovelItems.Count - 1; i >= 0; i--)
            {
                if (NovelItems[i].Id == item.Id)
                {
                    NovelItems.RemoveAt(i);
                }
            }
            await _app.Storage.DeleteBookAsync(item);
            await _app.Database.DeleteBookAsync(item.Id);
        }
    }
}
