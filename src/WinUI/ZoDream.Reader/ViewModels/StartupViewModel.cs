using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.WinUI.Helpers;
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

namespace ZoDream.Reader.ViewModels
{
    public class StartupViewModel : ObservableObject
    {

        public StartupViewModel()
        {
            OpenCommand = new RelayCommand(TapOpen);
            CreateCommand = new RelayCommand(TapCreate);
            Version = App.GetService<AppViewModel>().Version;
        }

        private string version;

        public string Version {
            get => version;
            set => SetProperty(ref version, value);
        }

        private string tip = string.Empty;

        public string Tip {
            get => tip;
            set => SetProperty(ref tip, value);
        }

        public ICommand OpenCommand { get; private set; }

        public ICommand CreateCommand { get; private set; }

        private async void TapOpen()
        {
            var picker = new FolderPicker();
            App.GetService<AppViewModel>().InitializePicker(picker);
            picker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            var folder = await picker.PickSingleFolderAsync();
            if (folder is null)
            {
                return;
            }
            var checkFile = await folder.GetFileAsync(AppConstants.DatabaseFileName);
            if (checkFile is null)
            {
                // 不存在
                return;
            }
            StorageApplicationPermissions.FutureAccessList.AddOrReplace(AppConstants.WorkspaceToken, folder);
            await App.GetService<AppViewModel>().InitializeWorkspaceAsync(folder);
            App.GetService<IRouter>().GoToAsync(Router.HomeRoute);
        }

        private async void TapCreate()
        {
            var picker = new FolderPicker();
            App.GetService<AppViewModel>().InitializePicker(picker);
            picker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            var folder = await picker.PickSingleFolderAsync();
            if (folder is null)
            {
                return;
            }
            StorageApplicationPermissions.FutureAccessList.AddOrReplace(AppConstants.WorkspaceToken, folder);
            await App.GetService<AppViewModel>().InitializeWorkspaceAsync(folder, true);
            App.GetService<IRouter>().GoToAsync(Router.HomeRoute);
        }
    }
}
