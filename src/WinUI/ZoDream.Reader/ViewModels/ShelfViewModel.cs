using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Storage.Pickers;
using ZoDream.Reader.Dialogs;
using ZoDream.Shared.Interfaces.Route;
using ZoDream.Shared.Repositories.Entities;

namespace ZoDream.Reader.ViewModels
{
    public class ShelfViewModel: ObservableObject
    {
        public ShelfViewModel()
        {
            AddCommand = new RelayCommand(TapAdd);
            CreateCommand = new RelayCommand(TapCreate);
            LoadAsync();
        }

        private readonly AppViewModel _app = App.GetService<AppViewModel>();

        private ObservableCollection<NovelItemViewModel> novelItems = [];

        public ObservableCollection<NovelItemViewModel> NovelItems {
            get => novelItems;
            set => SetProperty(ref novelItems, value);
        }



        public ICommand AddCommand { get; private set; }
        public ICommand CreateCommand { get; private set; }

        private void TapCreate()
        {
            var router = App.GetService<IRouter>();
            router.GoToAsync("creator/novel");
        }

        private async void TapAdd()
        {
            var picker = new AddNovelDialog();
            if (await _app.OpenDialogAsync(picker) != Microsoft.UI.Xaml.Controls.ContentDialogResult.None)
            {
                return;
            }
            if (picker.SelectedIndex > 0)
            {
                LoadNet();
            } else
            {
                LoadLocal();
            }
        }

        private async void LoadLocal()
        {
            var picker = new FileOpenPicker();
            picker.FileTypeFilter.Add(".txt");
            picker.FileTypeFilter.Add(".epub");
            picker.FileTypeFilter.Add(".umd");
            _app.InitializePicker(picker);
            var file = await picker.PickSingleFileAsync();
            if (file is null)
            {
                return;
            }
            await _app.Storage.AddBookAsync(file);
            LoadAsync();
        }

        private async void LoadNet()
        {
            var picker = new SearchNovelDialog();
            await _app.OpenDialogAsync(picker);
        }

        public async void LoadAsync()
        {
            NovelItems.Clear();
            var items = await _app.Database.GetBookAsync<BookEntity>();
            foreach (var item in items)
            {
                NovelItems.Add(new NovelItemViewModel(item));
            }
        }

    }
}
