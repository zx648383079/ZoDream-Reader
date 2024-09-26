using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using System.Windows;
using ZoDream.Reader.Repositories;
using ZoDream.Reader.ViewModels;
using ZoDream.Shared.Interfaces;

namespace ZoDream.Reader
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {


        protected override async void OnStartup(StartupEventArgs e)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            await GetService<AppViewModel>().InitializeAsync();
            base.OnStartup(e);
            
        }


        private static bool IsBooted = false;
        public static T GetService<T>()
        {
            if (!IsBooted)
            {
                IsBooted = true;
                RegisterServices();
            }
            return Ioc.Default.GetService<T>();
        }

        private static void RegisterServices()
        {
            var services = new ServiceCollection();
            services.AddSingleton<AppViewModel>();
            services.AddSingleton<ISettingRepository, SettingRepository>();
            Ioc.Default.ConfigureServices(services.BuildServiceProvider());
        }
    }
}
