using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage.Pickers;
using ZoDream.Reader.Dialogs;
using ZoDream.Shared.Interfaces.Entities;
using ZoDream.Shared.Models;
using ZoDream.Shared.Plugins.Importers;
using ZoDream.Shared.Repositories.Entities;
using ZoDream.Shared.Repositories.Extensions;
using ZoDream.Shared.Repositories.Models;

namespace ZoDream.Reader.ViewModels
{
    public class AppThemeViewModel: ObservableObject
    {
        public AppThemeViewModel()
        {
            AddCommand = new RelayCommand(TapAdd);
            ImportCommand = new RelayCommand(TapImport);
            PasteCommand = new RelayCommand(TapPaste);
            EditCommand = new RelayCommand<AppThemeModel>(TapEdit);
            DeleteCommand = new RelayCommand<AppThemeModel>(TapDelete);
            ToggleCheckCommand = new RelayCommand(TapToggleCheck);
            ToggleCommand = new RelayCommand<AppThemeModel>(TapToggle);
            LoadAsync();
        }
        private readonly AppViewModel _app = App.GetService<AppViewModel>();

        private ObservableCollection<AppThemeModel> themeItems = [];

        public ObservableCollection<AppThemeModel> ThemeItems {
            get => themeItems;
            set => SetProperty(ref themeItems, value);
        }

        public ICommand AddCommand { get; private set; }

        public ICommand ImportCommand {  get; private set; }

        public ICommand PasteCommand { get; private set; }
        public ICommand EditCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }

        public ICommand ToggleCheckCommand { get; private set; }
        public ICommand ToggleCommand { get; private set; }

        private async void TapPaste()
        {
            var package = Clipboard.GetContent();
            if (package.Contains(StandardDataFormats.Text))
            {
                var text = await package.GetTextAsync();
            }
        }

        private void TapToggle(AppThemeModel? arg)
        {
            if (arg is null || !arg.IsEnabled)
            {
                return;
            }
            foreach (var item in ThemeItems)
            {
                if (item == arg)
                {
                    continue;
                }
                item.IsEnabled = false;
            }
        }
        private void TapToggleCheck()
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

        private void TapDelete(AppThemeModel? arg)
        {

            if (arg is null)
            {
                DeleteTheme(ThemeItems.Where(item => item.IsChecked).Select(item => item.Id).ToArray());
                return;
            }
            DeleteTheme(arg.Id);
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
            await _app.Database.DeleteThemeAsync(items);
            for (int i = ThemeItems.Count - 1; i >= 0; i--)
            {
                if (items.Contains(ThemeItems[i].Id))
                {
                    ThemeItems.RemoveAt(i);
                }
            }
        }

        private void TapEdit(AppThemeModel? arg)
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
            EditTheme(arg);
        }

        private async void EditTheme(AppThemeModel data)
        {
            var picker = new AddAppThemeDialog
            {
                DataContext = data.Clone<AppThemeModel>()
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
            await _app.Database.SaveThemeAsync(data);
        }

        private async void TapAdd()
        {
            var picker = new AddAppThemeDialog();
            var res = await _app.OpenDialogAsync(picker);
            if (res != Microsoft.UI.Xaml.Controls.ContentDialogResult.Primary)
            {
                return;
            }
            if (string.IsNullOrWhiteSpace(picker.ViewModel.Name))
            {
                return;
            }
            var item = picker.ViewModel.Clone<AppThemeModel>();
            ThemeItems.Add(item);
            await _app.Database.SaveThemeAsync(item);
        }

        private async void TapImport()
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
            var items = await dialog.Importer.LoadAppThemeAsync<AppThemeModel>(file.Path);
            foreach (var item in items)
            {
                if (Contains(item))
                {
                    continue;
                }
                ThemeItems.Add(item);
                await _app.Database.SaveThemeAsync(item);
            }
        }

        public bool Contains(IAppTheme data)
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
            var items = await _app.Database.GetThemeAsync<AppThemeModel>();
            foreach (var item in items)
            {
                ThemeItems.Add(item);
            }
        }
    }
}
