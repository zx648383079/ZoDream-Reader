using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using ZoDream.Reader.Repositories;
using ZoDream.Reader.ViewModels;
using ZoDream.Shared.Interfaces.Route;
using ZoDream.Shared.ViewModels;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ZoDream.Reader.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            (App.GetService<IRouter>() as Router)?.BindInner(ContentFrame);
            if (App.GetService<AppViewModel>() is AppViewModel viewModel)
            {
                viewModel.TitleBar.MenuVisible = Visibility.Visible;
                viewModel.TitleBar.MenuCommand = new RelayCommand(_ => {
                    MenuBar.PaneDisplayMode = MenuBar.PaneDisplayMode ==
                    NavigationViewPaneDisplayMode.LeftCompact ? NavigationViewPaneDisplayMode.Auto : NavigationViewPaneDisplayMode.LeftCompact;
                });
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            (App.GetService<IRouter>() as Router)?.BindInner(null);
            if (App.GetService<AppViewModel>() is AppViewModel viewModel)
            {
                viewModel.TitleBar.MenuVisible = Visibility.Collapsed;
                viewModel.TitleBar.MenuCommand = null;
            }
        }

        private void NavigationView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            var router = (App.GetService<IRouter>() as Router);
            if (args.IsSettingsInvoked)
            {
                router?.GoToAsync("setting");
                return;
            }
            router?.GoToAsync(args.InvokedItemContainer.Tag.ToString());
        }
    }
}
