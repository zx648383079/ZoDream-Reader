using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Windows.Input;
using Windows.Storage.AccessCache;
using Microsoft.Windows.Storage.Pickers;
using ZoDream.Reader.Repositories;
using ZoDream.Shared.Interfaces.Route;
using ZoDream.Shared.Repositories;
using System.IO;
using Windows.Storage;

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

        private readonly AppViewModel _app = App.GetService<AppViewModel>();

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
            var picker = new FolderPicker(_app.AppWindowId);
            picker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            var folder = await picker.PickSingleFolderAsync();
            if (folder is null)
            {
                return;
            }
            var checkFile = Path.Combine(folder.Path, AppConstants.DatabaseFileName);
            if (!File.Exists(checkFile))
            {
                // 不存在
                return;
            }
            var target = await StorageFolder.GetFolderFromPathAsync(folder.Path);
            StorageApplicationPermissions.FutureAccessList.AddOrReplace(AppConstants.WorkspaceToken, target);
            await _app.InitializeWorkspaceAsync(target);
            App.GetService<IRouter>().GoToAsync(Router.HomeRoute);
        }

        private async void TapCreate()
        {
            var picker = new FolderPicker(_app.AppWindowId);
            picker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            var folder = await picker.PickSingleFolderAsync();
            if (folder is null)
            {
                return;
            }
            var target = await StorageFolder.GetFolderFromPathAsync(folder.Path);
            StorageApplicationPermissions.FutureAccessList.AddOrReplace(AppConstants.WorkspaceToken, target);
            await _app.InitializeWorkspaceAsync(target, true);
            App.GetService<IRouter>().GoToAsync(Router.HomeRoute);
        }
    }
}
