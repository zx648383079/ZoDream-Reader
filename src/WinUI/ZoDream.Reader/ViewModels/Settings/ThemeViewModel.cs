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
    public class ThemeViewModel: ObservableObject
    {

        public ThemeViewModel()
        {
            ThemeCommand = new RelayCommand(TapTheme);
            ReadThemeCommand = new RelayCommand(TapReadTheme);
        }

        public ICommand ThemeCommand { get; private set; }
        public ICommand ReadThemeCommand { get; private set; }

        private void TapReadTheme()
        {
            App.GetService<IRouter>().GoToAsync("setting/read_theme");
        }

        private void TapTheme()
        {
            App.GetService<IRouter>().GoToAsync("setting/app_theme");
        }

    }
}
