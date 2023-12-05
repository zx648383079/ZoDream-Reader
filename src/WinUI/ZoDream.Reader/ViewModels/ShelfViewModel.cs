using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Storage.Pickers;
using ZoDream.Reader.Dialogs;
using ZoDream.Shared.Interfaces.Entities;
using ZoDream.Shared.Repositories.Entities;
using ZoDream.Shared.Repositories.Models;
using ZoDream.Shared.ViewModels;

namespace ZoDream.Reader.ViewModels
{
    public class ShelfViewModel: BindableBase
    {
        public ShelfViewModel()
        {
            AddCommand = new RelayCommand(TapAdd);
            LoadAsync();
        }

        private readonly AppViewModel _app = App.GetService<AppViewModel>();

        private ObservableCollection<BookEntity> novelItems = new();

        public ObservableCollection<BookEntity> NovelItems {
            get => novelItems;
            set => Set(ref novelItems, value);
        }



        public ICommand AddCommand { get; private set; }

        private async void TapAdd(object? _)
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
                NovelItems.Add(item);
            }
        }

    }
}
