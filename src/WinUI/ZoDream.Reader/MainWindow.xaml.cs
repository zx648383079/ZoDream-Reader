using Microsoft.UI.Xaml;
using System.Diagnostics;
using ZoDream.Reader.Repositories;
using ZoDream.Reader.ViewModels;
using ZoDream.Shared.Interfaces.Route;

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
            BindRouter();
        }

        public AppViewModel ViewModel = App.GetService<AppViewModel>();

        private void BindRouter()
        {
            if (App.GetService<IRouter>() is not Router router)
            {
                return;
            }
            router.BindMain(AppFrame, ReadFrame);
            
        }
    }
}
