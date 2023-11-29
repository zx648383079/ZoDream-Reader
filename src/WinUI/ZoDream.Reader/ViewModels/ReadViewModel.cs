using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ZoDream.Reader.Dialogs;
using ZoDream.Shared.Repositories.Entities;
using ZoDream.Shared.ViewModels;

namespace ZoDream.Reader.ViewModels
{
    public class ReadViewModel: BindableBase
    {

        public ReadViewModel()
        {
            CatalogCommand = new RelayCommand(TapCatalog);
            PreviousCommand = new RelayCommand(TapPrevious);
            NextCommand = new RelayCommand(TapNext);
            SourceCommand = new RelayCommand(TapSource);
            SettingCommand = new RelayCommand(TapSetting);
        }


        private ObservableCollection<ChapterModel> item = new();

        public ObservableCollection<ChapterModel> Items {
            get => item;
            set => Set(ref item, value);
        }

        private bool catalogOpen;

        public bool CatalogOpen {
            get => catalogOpen;
            set => Set(ref catalogOpen, value);
        }

        public ICommand CatalogCommand { get; private set; }
        public ICommand SourceCommand { get; private set; }
        public ICommand SettingCommand { get; private set; }
        public ICommand PreviousCommand { get; private set; }
        public ICommand NextCommand { get; private set; }

        private void TapCatalog(object? _)
        {
            CatalogOpen = !CatalogOpen;
        }

        private void TapPrevious(object? _)
        {

        }
        private void TapNext(object? _)
        {

        }

        private async void TapSource(object? _)
        {
            var app = App.GetService<AppViewModel>();
            var dialog = new SearchSourceDialog();
            await app.OpenDialogAsync(dialog);
        }
        private void TapSetting(object? _)
        {

        }

    }
}
