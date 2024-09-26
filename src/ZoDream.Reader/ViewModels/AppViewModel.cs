using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ZoDream.Reader.Repositories;
using ZoDream.Shared.Interfaces;
using ZoDream.Shared.Interfaces.Entities;
using ZoDream.Shared.Models;
using ZoDream.Shared.Repositories;
using ZoDream.Shared.Repositories.Models;

namespace ZoDream.Reader.ViewModels
{
    public class AppViewModel
    {
        public AppViewModel()
        {
            _setting = App.GetService<ISettingRepository>();
        }
        private readonly ISettingRepository _setting;
        public IDatabaseRepository Database { get; private set; }
        public DiskRepository Storage { get; private set; }

        public IAppOption Option { get; private set; } = new AppOption();

        public IAppTheme Theme { get; private set; }

        public IReadTheme ReadTheme { get; private set; }

        public bool IsFirstLaunchEver => !_setting.Exist(SettingNames.AppInstalled);


        public bool HasLibrary {
            get {
                var path = _setting.Get(SettingNames.LibraryPath, string.Empty);
                return !string.IsNullOrEmpty(path) && Directory.Exists(path);
            }
        }

        public async Task InitializeAsync()
        {
            await _setting.LoadAsync();
            InitializeTheme();
            var sysLan = CultureInfo.CurrentCulture.TwoLetterISOLanguageName.StartsWith("zh") ? "zh-CN" : "en-US";
            var customLan = _setting.Get(SettingNames.AppLanguage, sysLan);
            // ApplicationLanguages.PrimaryLanguageOverride = customLan;
            var root = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, AppConstants.WorkspaceToken);
            await InitializeWorkspaceAsync(root, !HasLibrary);
        }

        internal async Task InitializeWorkspaceAsync(string folder, bool createNew = false)
        {
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            _setting.Set(SettingNames.AppInstalled, true);
            _setting.Set(SettingNames.LibraryPath, folder);
            _ = _setting.SaveAsync();
            Storage = new DiskRepository(folder);
            Database = createNew ? await Storage.CreateDatabaseAsync() :
                await Storage.OpenDatabaseAsync();
            Option = await Database.LoadSettingAsync();
            Theme = new AppThemeModel();
            ReadTheme = new ReadThemeModel();
        }

        internal async Task InitializeWorkspaceAsync()
        {
            Database = await Storage.CreateDatabaseAsync();
        }

        public void InitializeTheme()
        {
            var localTheme = App.GetService<ISettingRepository>().Get(SettingNames.AppTheme, AppConstants.ThemeDefault);
            if (localTheme != AppConstants.ThemeDefault)
            {
                ResourceDictionary dict = new()
                {
                    Source = new Uri("/Themes/LightTheme.xaml", UriKind.Relative)
                };
                App.Current.Resources.MergedDictionaries.Add(dict);
            }
        }
    }
}
