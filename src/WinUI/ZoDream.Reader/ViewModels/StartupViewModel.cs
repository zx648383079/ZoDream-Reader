using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.Storage.Pickers;
using ZoDream.Reader.Repositories;
using ZoDream.Shared.Interfaces.Route;
using ZoDream.Shared.Repositories;
using ZoDream.Shared.ViewModels;

namespace ZoDream.Reader.ViewModels
{
    public class StartupViewModel : BindableBase
    {

        public StartupViewModel()
        {
            OpenCommand = new RelayCommand(TapOpen);
            CreateCommand = new RelayCommand(TapCreate);
            Version = App.GetService<AppViewModel>().GetVersionNumber();
        }

        private string version;

        public string Version {
            get => version;
            set => Set(ref version, value);
        }

        private string tip = string.Empty;

        public string Tip {
            get => tip;
            set => Set(ref tip, value);
        }

        public ICommand OpenCommand { get; private set; }

        public ICommand CreateCommand { get; private set; }

        private async void TapOpen(object _)
        {
            var picker = new FolderPicker();
            App.GetService<AppViewModel>().InitializePicker(picker);
            picker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            var folder = await picker.PickSingleFolderAsync();
            if (folder != null)
            {
                StorageApplicationPermissions.FutureAccessList.AddOrReplace(AppConstants.WorkspaceToken, folder);
            }
        }

        private void TapCreate(object _)
        {
            App.GetService<IRouter>().GoToAsync("home");
        }
    }
}
