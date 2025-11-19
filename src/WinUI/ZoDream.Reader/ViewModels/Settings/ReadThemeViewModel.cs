using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Microsoft.Windows.Storage.Pickers;
using ZoDream.Reader.Dialogs;
using ZoDream.Shared.Interfaces.Entities;
using ZoDream.Shared.Repositories.Extensions;
using ZoDream.Shared.Repositories.Models;

namespace ZoDream.Reader.ViewModels
{
    public class ReadThemeViewModel: ObservableObject
    {
        public ReadThemeViewModel()
        {
            AddCommand = new RelayCommand(TapAdd);
            ImportCommand = new RelayCommand(TapImport);
            EditCommand = new RelayCommand<ReadThemeModel>(TapEdit);
            DeleteCommand = new RelayCommand<ReadThemeModel>(TapDelete);
            ToggleCheckCommand = new RelayCommand(TapToggleCheck);
            ToggleCommand = new RelayCommand<ReadThemeModel>(TapToggle);
            LoadAsync();
        }
        private readonly AppViewModel _app = App.GetService<AppViewModel>();

        private ObservableCollection<ReadThemeModel> themeItems = new();

        public ObservableCollection<ReadThemeModel> ThemeItems {
            get => themeItems;
            set => SetProperty(ref themeItems, value);
        }

        public ICommand AddCommand { get; private set; }

        public ICommand ImportCommand {  get; private set; }
        public ICommand EditCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }

        public ICommand ToggleCheckCommand { get; private set; }
        public ICommand ToggleCommand { get; private set; }

        private void TapToggle(ReadThemeModel? arg)
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

        private void TapDelete(ReadThemeModel? arg)
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
            await _app.Database.DeleteReadThemeAsync(items);
            for (int i = ThemeItems.Count - 1; i >= 0; i--)
            {
                if (items.Contains(ThemeItems[i].Id))
                {
                    ThemeItems.RemoveAt(i);
                }
            }
        }

        private void TapEdit(ReadThemeModel? arg)
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

        private async void TapAdd()
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

        private async void TapImport()
        {
            var dialog = new ImportDialog();
            var res = await _app.OpenDialogAsync(dialog);
            if (res != Microsoft.UI.Xaml.Controls.ContentDialogResult.None)
            {
                return;
            }
            var picker = new FileOpenPicker(_app.AppWindowId);
            picker.FileTypeFilter.Add(".json");
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
