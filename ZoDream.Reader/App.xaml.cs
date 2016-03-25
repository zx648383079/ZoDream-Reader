using System.Windows;
using GalaSoft.MvvmLight.Threading;
using ZoDream.Reader.Helper;

namespace ZoDream.Reader
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        static App()
        {
            DispatcherHelper.Initialize();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            //DatabaseHelper.ChooseSqlite();
            //DatabaseHelper.Init();
        }
    }
}
