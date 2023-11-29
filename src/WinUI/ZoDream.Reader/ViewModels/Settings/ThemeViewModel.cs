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
    public class ThemeViewModel: BindableBase
    {

        public ThemeViewModel()
        {
            ThemeCommand = new RelayCommand(TapTheme);
        }

        public ICommand ThemeCommand { get; private set; }


        private void TapTheme(object? _)
        {
            App.GetService<IRouter>().GoToAsync("setting/app_theme");
        }

    }
}
