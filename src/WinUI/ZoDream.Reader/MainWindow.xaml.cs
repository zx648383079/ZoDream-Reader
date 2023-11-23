using Microsoft.UI.Xaml;
using System.Diagnostics;
using ZoDream.Reader.Repositories;
using ZoDream.Reader.ViewModels;
using ZoDream.Shared.Interfaces.Route;
using ZoDream.Shared.ViewModels;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ZoDream.Reader
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
            ViewModel.BaseWindow = this;
            ViewModel.TitleBar = AppTitleBar;
            BindRouter();
        }

        public AppViewModel ViewModel => App.GetService<AppViewModel>();

        private void BindRouter()
        {
            if (App.GetService<IRouter>() is not Router router)
            {
                return;
            }
            router.BindMain(AppFrame, ReadFrame);
            AppTitleBar.BackCommand = new RelayCommand(_ => {
                Debug.WriteLine(1);
                router.GoBackAsync();
            });
            router.RouteChanged += Router_RouteChanged;
        }


        private void Router_RouteChanged(object sender, RoutedEventArgs e)
        {
            if (sender is Router router)
            {
                AppTitleBar.BackVisible = router.IsBackVisible ? Visibility.Visible : Visibility.Collapsed;
                if (!router.IsMenuVisible)
                {
                    AppTitleBar.MenuVisible = Visibility.Collapsed;
                }
                
            }   
        }
    }
}
