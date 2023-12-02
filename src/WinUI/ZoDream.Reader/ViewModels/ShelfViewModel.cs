using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
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
