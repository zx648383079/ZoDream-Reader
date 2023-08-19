using Microsoft.UI;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Globalization;
using Windows.UI;
using WinRT.Interop;
using ZoDream.Reader.Repositories;
using ZoDream.Shared.Interfaces;
using ZoDream.Shared.Interfaces.Route;
using ZoDream.Shared.Repositories;

namespace ZoDream.Reader.ViewModels
{
    public class AppViewModel
    {


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
        public DispatcherQueue DispatcherQueue  => _baseWindow?.DispatcherQueue;

        public XamlRoot BaseXamlRoot => _baseWindow?.Content.XamlRoot;

        /// <summary>
        /// 获取当前版本号.
        /// </summary>
        /// <returns>版本号.</returns>
        public string GetVersionNumber()
        {
            var version = Package.Current.Id.Version;
            return $"{version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
        }

        public async Task Initialize()
        {
            var setting = App.GetService<ISettingRepository>();
            await setting.LoadAsync();
            InitializeTheme();
            var sysLan = CultureInfo.CurrentCulture.TwoLetterISOLanguageName.StartsWith("zh") ? "zh-CN" : "en-US";
            var customLan = setting.Get(SettingNames.AppLanguage, sysLan);
            ApplicationLanguages.PrimaryLanguageOverride = customLan;
            App.GetService<IRouter>().GoToAsync("startup");
        }

        public void InitializePicker(object target)
        {
            WinRT.Interop.InitializeWithWindow.Initialize(target, _baseWindowHandle);
        }

        public void InitializeTheme()
        {
            var localTheme = App.GetService<ISettingRepository>().Get<string>(SettingNames.AppTheme, AppConstants.ThemeDefault);
            if (localTheme != AppConstants.ThemeDefault)
            {
                Application.Current.RequestedTheme = localTheme == AppConstants.ThemeLight ?
                                        ApplicationTheme.Light :
                                        ApplicationTheme.Dark;
            }
        }

        public void InitializeTitleBar()
        {
            if (_appWindow is null)
            {
                return;
            }
            var bar = _appWindow.TitleBar;
            bar.ExtendsContentIntoTitleBar = true;
            if (Application.Current.RequestedTheme == ApplicationTheme.Light)
            {
                bar.BackgroundColor = Colors.White;
                bar.InactiveBackgroundColor = Colors.White;
                bar.ButtonBackgroundColor = Color.FromArgb(255, 240, 243, 249);
                bar.ButtonForegroundColor = Colors.DarkGray;
                bar.ButtonHoverBackgroundColor = Colors.LightGray;
                bar.ButtonHoverForegroundColor = Colors.DarkGray;
                bar.ButtonPressedBackgroundColor = Colors.Gray;
                bar.ButtonPressedForegroundColor = Colors.DarkGray;
                bar.ButtonInactiveBackgroundColor = Color.FromArgb(255, 240, 243, 249);
                bar.ButtonInactiveForegroundColor = Colors.Gray;
            }
            else
            {
                bar.BackgroundColor = Color.FromArgb(255, 240, 243, 249);
                bar.InactiveBackgroundColor = Colors.Black;
                bar.ButtonBackgroundColor = Color.FromArgb(255, 32, 32, 32);
                bar.ButtonForegroundColor = Colors.White;
                bar.ButtonHoverBackgroundColor = Color.FromArgb(255, 20, 20, 20);
                bar.ButtonHoverForegroundColor = Colors.White;
                bar.ButtonPressedBackgroundColor = Color.FromArgb(255, 40, 40, 40);
                bar.ButtonPressedForegroundColor = Colors.White;
                bar.ButtonInactiveBackgroundColor = Color.FromArgb(255, 32, 32, 32);
                bar.ButtonInactiveForegroundColor = Colors.Gray;
            }
        }

        public void FullScreenAsync(bool isFull)
        {
            _appWindow.SetPresenter(isFull ? AppWindowPresenterKind.FullScreen : AppWindowPresenterKind.Default);
        }
    }
}
