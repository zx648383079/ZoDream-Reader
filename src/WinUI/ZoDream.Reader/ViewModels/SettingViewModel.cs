using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ZoDream.Shared.Interfaces.Route;
using ZoDream.Shared.ViewModels;

namespace ZoDream.Reader.ViewModels
{
    public class SettingViewModel: BindableBase
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
        }

        public ICommand ChapterCommand { get; private set; }

        public ICommand SourceCommand { get; private set; }
        public ICommand ReplaceCommand { get; private set; }
        public ICommand DictionaryCommand { get; private set; }
        public ICommand HistoryCommand { get; private set; }
        public ICommand AboutCommand { get; private set; }
        public ICommand BookmarkCommand { get; private set; }
        public ICommand BakCommand { get; private set; }
        public ICommand ThemeCommand { get; private set; }
        public ICommand OtherCommand { get; private set; }

        private void TapBookmark(object _)
        {
            App.GetService<IRouter>().GoToAsync("bookmark");
        }

        private void TapBak(object _)
        {
            App.GetService<IRouter>().GoToAsync("setting/bak");
        }

        private void TapTheme(object _)
        {
            App.GetService<IRouter>().GoToAsync("setting/theme");
        }

        private void TapOther(object _)
        {
            App.GetService<IRouter>().GoToAsync("setting/other");
        }

        private void TapSource(object _)
        {
            App.GetService<IRouter>().GoToAsync("rule/source");
        }

        private void TapHistory(object _)
        {
            App.GetService<IRouter>().GoToAsync("history");
        }

        private void TapReplace(object _)
        {
            App.GetService<IRouter>().GoToAsync("rule/replace");
        }

        private void TapDictionary(object _)
        {
            App.GetService<IRouter>().GoToAsync("rule/dictionary");
        }

        private void TapChapter(object _)
        {
            App.GetService<IRouter>().GoToAsync("rule/chapter");
        }

        private void TapAbout(object _)
        {
            App.GetService<IRouter>().GoToAsync("about");
        }
    }
}
