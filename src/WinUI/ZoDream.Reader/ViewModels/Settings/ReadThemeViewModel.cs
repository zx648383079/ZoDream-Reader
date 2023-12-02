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
using ZoDream.Shared.Models;
using ZoDream.Shared.Plugins.Importers;
using ZoDream.Shared.Repositories.Entities;
using ZoDream.Shared.Repositories.Extensions;
using ZoDream.Shared.Repositories.Models;
using ZoDream.Shared.ViewModels;

namespace ZoDream.Reader.ViewModels
{
    public class ReadThemeViewModel: BindableBase
    {
        public ReadThemeViewModel()
        {
            AddCommand = new RelayCommand(TapAdd);
            ImportCommand = new RelayCommand(TapImport);
            EditCommand = new RelayCommand(TapEdit);
            DeleteCommand = new RelayCommand(TapDelete);
            ToggleCheckCommand = new RelayCommand(TapToggleCheck);
            ToggleCommand = new RelayCommand(TapToggle);
            LoadAsync();
        }
        private readonly AppViewModel _app = App.GetService<AppViewModel>();

        private ObservableCollection<ReadThemeModel> themeItems = new();

        public ObservableCollection<ReadThemeModel> ThemeItems {
            get => themeItems;
            set => Set(ref themeItems, value);
        }

        public ICommand AddCommand { get; private set; }

        public ICommand ImportCommand {  get; private set; }
        public ICommand EditCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }

        public ICommand ToggleCheckCommand { get; private set; }
        public ICommand ToggleCommand { get; private set; }

        private void TapToggle(object? arg)
        {
            if (arg is not ReadThemeModel data || !data.IsEnabled)
            {
                return;
            }
            foreach (var item in ThemeItems)
            {
                if (item == data)
                {
                    continue;
                }
                item.IsEnabled = false;
            }
        }

        private void TapToggleCheck(object? _)
        {
            if (ThemeItems.Count == 0)
            {
                return;
            }
            var isChecked = !ThemeItems[0].IsChecked;
            foreach (var item in ThemeItems)
            {
                item.IsChecked = isChecked;
            }
        }

        private void TapDelete(object? arg)
        {

            if (arg is null)
            {
                DeleteTheme(ThemeItems.Where(item => item.IsChecked).Select(item => item.Id).ToArray());
                return;
            }
            if (arg is AppThemeModel data)
            {
                DeleteTheme(data.Id);
            }
        }

        private async void DeleteTheme(params int[] items)
        {
            if (items.Length == 0)
            {
                return;
            }
            if (!await _app.ConfirmAsync($"确定删除 {items.Length} 个主题？"))
            {
                return;
            }
            await _app.Database.DeleteReadThemeAsync(items);
            for (int i = ThemeItems.Count - 1; i >= 0; i--)
            {
                if (items.Contains(ThemeItems[i].Id))
                {
                    ThemeItems.RemoveAt(i);
                }
            }
        }

        private void TapEdit(object? arg)
        {
            if (arg is null)
            {
                foreach (var item in ThemeItems)
                {
                    if (item.IsChecked)
                    {
                        EditTheme(item);
                        break;
                    }
                }
                return;
            }
            if (arg is ReadThemeModel data)
            {
                EditTheme(data);
            }

        }

        private async void EditTheme(ReadThemeModel data)
        {
            var picker = new AddReadThemeDialog
            {
                DataContext = data.Clone<ReadThemeModel>()
            };
            var res = await _app.OpenDialogAsync(picker);
            if (res != Microsoft.UI.Xaml.Controls.ContentDialogResult.Primary)
            {
                return;
            }
            if (string.IsNullOrWhiteSpace(picker.ViewModel.Name))
            {
                return;
            }
            picker.ViewModel.CopyTo(data);
            await _app.Database.SaveReadThemeAsync(data);
        }

        private async void TapAdd(object? _)
        {
            var picker = new AddReadThemeDialog();
            var res = await _app.OpenDialogAsync(picker);
            if (res != Microsoft.UI.Xaml.Controls.ContentDialogResult.Primary)
            {
                return;
            }
            if (string.IsNullOrWhiteSpace(picker.ViewModel.Name))
            {
                return;
            }
            var item = picker.ViewModel.Clone<ReadThemeModel>();
            ThemeItems.Add(item);
            await _app.Database.SaveReadThemeAsync(item);
        }

        private async void TapImport(object? _)
        {
            var dialog = new ImportDialog();
            var res = await _app.OpenDialogAsync(dialog);
            if (res != Microsoft.UI.Xaml.Controls.ContentDialogResult.None)
            {
                return;
            }
            var picker = new FileOpenPicker();
            picker.FileTypeFilter.Add(".json");
            _app.InitializePicker(picker);
            var file = await picker.PickSingleFileAsync();
            if (file is null)
            {
                return;
            }
            var items = await dialog.Importer.LoadReadThemeAsync<ReadThemeModel>(file.Path);
            foreach (var item in items)
            {
                if (Contains(item))
                {
                    continue;
                }
                ThemeItems.Add(item);
                await _app.Database.SaveReadThemeAsync(item);
            }
        }

        public bool Contains(IReadTheme data)
        {
            foreach (var item in ThemeItems)
            {
                if (item.Name == data.Name)
                {
                    return true;
                }
            }
            return false;
        }
        public async void LoadAsync()
        {
            ThemeItems.Clear();
            var items = await _app.Database.GetReadThemeAsync<ReadThemeModel>();
            foreach (var item in items)
            {
                ThemeItems.Add(item);
            }
        }
    }
}
