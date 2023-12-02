using Microsoft.UI;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Input;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.Globalization;
using Windows.Graphics;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.UI;
using WinRT.Interop;
using ZoDream.Reader.Controls;
using ZoDream.Reader.Dialogs;
using ZoDream.Reader.Repositories;
using ZoDream.Shared.Interfaces;
using ZoDream.Shared.Interfaces.Route;
using ZoDream.Shared.Repositories;
using ZoDream.Shared.ViewModels;

namespace ZoDream.Reader.ViewModels
{
    public partial class AppViewModel
    {
        public AppViewModel()
        {
            BackCommand = new RelayCommand(TapBack);
            MenuCommand = new RelayCommand(TapMenu);
            _setting = App.GetService<ISettingRepository>();
            _router = (Router)App.GetService<IRouter>();
            _router.RouteChanged += Router_RouteChanged;
        }

        private readonly ISettingRepository _setting;
        private readonly Router _router;
        private Window _baseWindow;
        private IntPtr _baseWindowHandle;
        private AppWindow _appWindow;
        public Window BaseWindow {
            set {
                _baseWindow = value;
                _baseWindowHandle = WindowNative.GetWindowHandle(_baseWindow);
                var windowId = Win32Interop.GetWindowIdFromWindow(_baseWindowHandle);
                _appWindow = AppWindow.GetFromWindowId(windowId);
            }
        }

        /// <summary>
        /// UI线程.
        /// </summary>
        public DispatcherQueue DispatcherQueue  => _baseWindow!.DispatcherQueue;

        public XamlRoot BaseXamlRoot => _baseWindow!.Content.XamlRoot;

        public IDatabaseRepository Database { get; private set; }
        public DiskRepository Storage { get; private set; }


        public bool IsFirstLaunchEver => !_setting.Exist(SettingNames.AppInstalled);

        public bool HasLibrary {
            get {
                var path = _setting.Get(SettingNames.LibraryPath, string.Empty);
                return !string.IsNullOrEmpty(path) && Directory.Exists(path);
            }
        }

        /// <summary>
        /// 获取当前版本号.
        /// </summary>
        /// <returns>版本号.</returns>
        public string Version
        {
            get {
                var version = Package.Current.Id.Version;
                return $"{version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
            }
        }

        public async Task InitializeAsync()
        {
            await _setting.LoadAsync();
            InitializeTheme();
            var sysLan = CultureInfo.CurrentCulture.TwoLetterISOLanguageName.StartsWith("zh") ? "zh-CN" : "en-US";
            var customLan = _setting.Get(SettingNames.AppLanguage, sysLan);
            ApplicationLanguages.PrimaryLanguageOverride = customLan;
            if (HasLibrary)
            {
                await InitializeWorkspaceAsync(await StorageApplicationPermissions.FutureAccessList.GetFolderAsync(AppConstants.WorkspaceToken));
            }
            App.GetService<IRouter>().GoToAsync(HasLibrary ? Router.HomeRoute : "startup");
        }

        public void InitializePicker(object target)
        {
            InitializeWithWindow.Initialize(target, _baseWindowHandle);
        }

        public async Task<bool> ConfirmAsync(string message, string title = "提示")
        {
            var dialog = new ConfirmDialog
            {
                Title = title,
                Content = message
            };
            return await OpenDialogAsync(dialog) == ContentDialogResult.Primary;
        }

        public IAsyncOperation<ContentDialogResult> OpenDialogAsync(ContentDialog target)
        {
            target.XamlRoot = BaseXamlRoot;
            return target.ShowAsync();
        }

        public void InitializeTheme()
        {
            var localTheme = App.GetService<ISettingRepository>().Get(SettingNames.AppTheme, AppConstants.ThemeDefault);
            if (localTheme != AppConstants.ThemeDefault)
            {
                Application.Current.RequestedTheme = localTheme == AppConstants.ThemeLight ?
                                        ApplicationTheme.Light :
                                        ApplicationTheme.Dark;
            }
        }


        public void FullScreenAsync(bool isFull)
        {
            _appWindow.SetPresenter(isFull ? AppWindowPresenterKind.FullScreen : AppWindowPresenterKind.Default);
        }

        internal async Task InitializeWorkspaceAsync(StorageFolder folder, bool createNew = false)
        {
            _setting.Set(SettingNames.AppInstalled, true);
            _setting.Set(SettingNames.LibraryPath, folder.Path);
            Storage = new DiskRepository(folder);
            Database = createNew ? await Storage.CreateDatabaseAsync() : 
                await Storage.OpenDatabaseAsync();
        }

        internal async Task InitializeWorkspaceAsync()
        {
            Database = await Storage.CreateDatabaseAsync();
        }
    }
}
