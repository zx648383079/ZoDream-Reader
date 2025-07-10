using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Storage.Pickers;
using ZoDream.Reader.Controls;
using ZoDream.Reader.Dialogs;
using ZoDream.Shared.Interfaces.Route;
using ZoDream.Shared.Repositories.Entities;

namespace ZoDream.Reader.ViewModels
{
    public class ShelfViewModel: ObservableObject
    {
        public ShelfViewModel()
        {
            AddCommand = UICommand.Add(TapAdd);
            GroupCommand = UICommand.Group(TapGroup);
            SyncCommand = UICommand.Sync(TapSync);
            CreateCommand = UICommand.Create(TapCreate);
            LayoutCommand = new RelayCommand(TapLayout);
            EditCommand = new RelayCommand<NovelItemViewModel>(TapEdit);
            DeleteCommand = new RelayCommand<NovelItemViewModel>(TapDelete);
            DetailCommand = new RelayCommand<NovelItemViewModel>(TapDetail);
            ReadCommand = new RelayCommand<NovelItemViewModel>(TapRead);
            MultipleCommand = UICommand.MultipleSelect(TapMultiple);
            SelectAllCommand = UICommand.SelectAll(TapSelectAll);
            LoadAsync();
        }

        private readonly AppViewModel _app = App.GetService<AppViewModel>();

        private ObservableCollection<NovelItemViewModel> _items = [];

        public ObservableCollection<NovelItemViewModel> Items {
            get => _items;
            set => SetProperty(ref _items, value);
        }

        private bool _layoutMode;

        public bool LayoutMode {
            get => _layoutMode;
            set {
                SetProperty(ref _layoutMode, value);
                OnPropertyChanged(nameof(LayoutIcon));
                OnPropertyChanged(nameof(LayoutColumnCount));
            }
        }

        private bool _isMultipleSelect;

        public bool IsMultipleSelect {
            get => _isMultipleSelect;
            set => SetProperty(ref _isMultipleSelect, value);
        }

        public int LayoutColumnCount => LayoutMode ? 1 : 2;
        public string LayoutIcon => LayoutMode ? "\uF0E2" : "\uE8FD";


        public ICommand AddCommand { get; private set; }
        public ICommand GroupCommand { get; private set; }
        public ICommand SyncCommand { get; private set; }
        public ICommand CreateCommand { get; private set; }
        public ICommand LayoutCommand { get; private set; }
        public ICommand MultipleCommand { get; private set; }
        public ICommand SelectAllCommand { get; private set; }
        public ICommand EditCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }
        public ICommand DetailCommand { get; private set; }
        public ICommand ReadCommand { get; private set; }


        private void TapGroup()
        {

        }

        private void TapSync()
        {

        }

        private void TapSelectAll()
        {
            var isChecked = Items.Where(i => !i.IsChecked).Any();
            foreach (var item in Items)
            {
                item.IsChecked = isChecked;
            }
        }

        private void TapDetail(NovelItemViewModel? arg)
        {
            if (arg is null)
            {
                return;
            }
            App.GetService<IRouter>().GoToAsync("novel", new Dictionary<string, object>
            {
                {"novel", arg.Source},
                {"id", arg.Source.Id},
            });
        }

        private void TapRead(NovelItemViewModel? arg)
        {
            if (arg is null)
            {
                return;
            }
            App.GetService<IRouter>().GoToAsync("read", new Dictionary<string, object>
            {
                {"novel", arg.Source},
                {"id", arg.Source.Id},
            });
        }

        private void TapMultiple()
        {
            IsMultipleSelect = !IsMultipleSelect;
        }

        private void TapEdit(NovelItemViewModel? arg)
        {
            
        }

        private async void TapDelete(NovelItemViewModel? arg)
        {
            NovelItemViewModel[] items;
            if (arg is null)
            {
                items = Items.Where(i => i.IsChecked).ToArray();
            } else
            {
                items = [arg];
            }
            if (items.Length == 0)
            {
                return;
            }
            if (!await _app.ConfirmAsync($"确定删除 {items.Length} 项?"))
            {
                return;
            }
            foreach (var item in items)
            {
                Items.Remove(item);
            }
            await _app.Database.DeleteBookAsync(items.Select(i => i.Source.Id).ToArray());
        }

        private void TapLayout()
        {
            LayoutMode = !LayoutMode;
        }

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
            Items.Clear();
            var items = await _app.Database.GetBookAsync<BookEntity>();
            foreach (var item in items)
            {
                Items.Add(new NovelItemViewModel(this, item));
            }
        }

    }
}
