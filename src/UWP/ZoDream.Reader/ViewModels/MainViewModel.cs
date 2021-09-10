using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace ZoDream.Reader.ViewModels
{
    public class MainViewModel: BindableBase
    {
        internal Frame RootFrame;
        internal Frame ChildFrame;

        public void Navigate(Type page, object parameter = null, bool isRoot = false)
        {
            if (isRoot)
            {
                RootFrame.Navigate(page, parameter);
                return;
            }
            if (ChildFrame == null)
            {
                RootFrame.Navigate(typeof(MainPage), parameter);
            }
            ChildFrame?.Navigate(page, parameter);
        }
    }
}
