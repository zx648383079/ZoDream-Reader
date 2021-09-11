using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Foundation.Metadata;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ZoDream.Reader.Pages;
using ZoDream.Reader.ViewModels;
using Windows.UI.ViewManagement;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace ZoDream.Reader
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            ShowStatusBar();
        }

        public MainViewModel ViewModel = App.ViewModel;

        private async void ShowStatusBar()
        {
            if (ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                var statusbar = StatusBar.GetForCurrentView();
                await statusbar.ShowAsync();
                statusbar.BackgroundColor = Colors.White;
                statusbar.BackgroundOpacity = 1;
                statusbar.ForegroundColor = Colors.Black;
            }
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ViewModel.ChildFrame = frame;
            if (e.NavigationMode == NavigationMode.New)
            {
                ViewModel.Navigate(typeof(BookPage));
            }
            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            if (ViewModel.ChildFrame == frame)
            {
                ViewModel.ChildFrame = null;
            }
        }

        private void NavigationView_BackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
        {
            frame.GoBack();
        }
    }
}
