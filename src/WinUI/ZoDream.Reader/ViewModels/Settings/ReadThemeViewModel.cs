﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Storage.Pickers;
using ZoDream.Reader.Dialogs;
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
        }

        private ObservableCollection<ReadThemeModel> themeItems = new();

        public ObservableCollection<ReadThemeModel> ThemeItems {
            get => themeItems;
            set => Set(ref themeItems, value);
        }

        public ICommand AddCommand { get; private set; }

        public ICommand ImportCommand {  get; private set; }

        private async void TapAdd(object? _)
        {
            var picker = new AddReadThemeDialog();
            var res = await App.GetService<AppViewModel>().OpenDialogAsync(picker);
            if (res != Microsoft.UI.Xaml.Controls.ContentDialogResult.Primary)
            {
                return;
            }
            ThemeItems.Add(picker.ViewModel.Clone<ReadThemeModel>());
        }

        private async void TapImport(object? _)
        {
            var app = App.GetService<AppViewModel>();
            var dialog = new ImportDialog();
            var res = await app.OpenDialogAsync(dialog);
            if (res != Microsoft.UI.Xaml.Controls.ContentDialogResult.None)
            {
                return;
            }
            var picker = new FileOpenPicker();
            picker.FileTypeFilter.Add(".json");
            app.InitializePicker(picker);
            var file = await picker.PickSingleFileAsync();
            if (file is null)
            {
                return;
            }
            var items = await dialog.Importer.LoadReadThemeAsync<ReadThemeModel>(file.Path);
            foreach (var item in items)
            {
                ThemeItems.Add(item);
            }
        }
    }
}
