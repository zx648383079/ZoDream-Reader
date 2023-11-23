using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using System.Globalization;
using System.Text;
using Windows.ApplicationModel.Activation;
using Windows.Globalization;
using ZoDream.Reader.Pages;
using ZoDream.Reader.Pages.Rules;
using ZoDream.Reader.Pages.Settings;
using ZoDream.Reader.Repositories;
using ZoDream.Reader.ViewModels;
using ZoDream.Shared.Interfaces;
using ZoDream.Shared.Interfaces.Route;
using ZoDream.Shared.Repositories;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ZoDream.Reader
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            RegisterServices();
            _ = GetService<AppViewModel>().InitializeAsync();
        }

        /// <summary>
        /// Invoked when the application is launched.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            if (args.UWPLaunchActivatedEventArgs.Kind == ActivationKind.File)
            {
                
            }
            m_window = new MainWindow();
            m_window.Activate();
        }

        private Window m_window;

        private static ServiceLocator Locator;

        public static T GetService<T>()
        {
            if (Locator is null)
            {
                RegisterServices();
            }
            return Locator.GetService<T>();
        }

        private static void RegisterServices()
        {
            Locator ??= new ServiceLocator();
            Locator.ServiceCollection.AddSingleton<AppViewModel>();
            Locator.ServiceCollection.AddSingleton<ISettingRepository, SettingRepository>();
            var router = new Router();
            router.RegisterRoute("startup", typeof(StartupPage));
            router.RegisterRoute(Router.HomeRoute, typeof(HomePage), true);
            router.RegisterRoute("explore", typeof(ExplorePage), true);
            router.RegisterRoute("novel", typeof(NovelPage), true);
            router.RegisterRoute("help", typeof(HelpPage), true);
            router.RegisterRoute("setting", typeof(SettingPage), true);
            router.RegisterRoute("setting/bak", typeof(BakPage), true);
            router.RegisterRoute("setting/theme", typeof(ThemePage), true);
            router.RegisterRoute("setting/other", typeof(OtherPage), true);
            router.RegisterRoute("about", typeof(AboutPage), true);
            router.RegisterRoute("history", typeof(HistoryPage), true);
            router.RegisterRoute("bookmark", typeof(BookmarkPage), true);
            router.RegisterRoute("rule/source", typeof(SourcePage), true);
            router.RegisterRoute("rule/chapter", typeof(ChapterRulePage), true);
            router.RegisterRoute("rule/replace", typeof(ReplaceRulePage), true);
            router.RegisterRoute("rule/dictionary", typeof(DictionaryRulePage), true);
            router.RegisterRoute("read", typeof(ReadPage), RouteType.Single);
            Locator.ServiceCollection.AddSingleton<IRouter>(router);
        }
    }
}
