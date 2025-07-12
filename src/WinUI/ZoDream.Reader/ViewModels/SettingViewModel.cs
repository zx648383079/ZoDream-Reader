using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ZoDream.Shared.Interfaces.Route;

namespace ZoDream.Reader.ViewModels
{
    public class SettingViewModel: ObservableObject
    {
        public SettingViewModel()
        {
            SourceCommand = new RelayCommand(TapSource);
            ChapterCommand = new RelayCommand(TapChapter);
            AboutCommand = new RelayCommand(TapAbout);
            ReplaceCommand = new RelayCommand(TapReplace);
            DictionaryCommand = new RelayCommand(TapDictionary);
            HistoryCommand = new RelayCommand(TapHistory);
            BookmarkCommand = new RelayCommand(TapBookmark);
            BakCommand = new RelayCommand(TapBak);
            ThemeCommand = new RelayCommand(TapTheme);
            OtherCommand = new RelayCommand(TapOther);
            FileCommand = new RelayCommand(TapFile);
        }

        public ICommand ChapterCommand { get; private set; }

        public ICommand SourceCommand { get; private set; }
        public ICommand ReplaceCommand { get; private set; }
        public ICommand DictionaryCommand { get; private set; }
        public ICommand HistoryCommand { get; private set; }
        public ICommand FileCommand { get; private set; }
        public ICommand AboutCommand { get; private set; }
        public ICommand BookmarkCommand { get; private set; }
        public ICommand BakCommand { get; private set; }
        public ICommand ThemeCommand { get; private set; }
        public ICommand OtherCommand { get; private set; }

        private void TapBookmark()
        {
            App.GetService<IRouter>().GoToAsync("bookmark");
        }

        private void TapFile()
        {
            App.GetService<IRouter>().GoToAsync("file/explorer", new Dictionary<string, object>()
            {
                {"folder", App.GetService<AppViewModel>().Storage.BaseFolder}
            });
        }

        private void TapBak()
        {
            App.GetService<IRouter>().GoToAsync("setting/bak");
        }

        private void TapTheme()
        {
            App.GetService<IRouter>().GoToAsync("setting/theme");
        }

        private void TapOther()
        {
            App.GetService<IRouter>().GoToAsync("setting/other");
        }

        private void TapSource()
        {
            App.GetService<IRouter>().GoToAsync("rule/source");
        }

        private void TapHistory()
        {
            App.GetService<IRouter>().GoToAsync("history");
        }

        private void TapReplace()
        {
            App.GetService<IRouter>().GoToAsync("rule/replace");
        }

        private void TapDictionary()
        {
            App.GetService<IRouter>().GoToAsync("rule/dictionary");
        }

        private void TapChapter()
        {
            App.GetService<IRouter>().GoToAsync("rule/chapter");
        }

        private void TapAbout()
        {
            App.GetService<IRouter>().GoToAsync("about");
        }
    }
}
