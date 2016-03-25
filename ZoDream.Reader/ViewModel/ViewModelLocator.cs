/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocatorTemplate xmlns:vm="clr-namespace:ZoDream.Reader.ViewModel"
                                   x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"
*/

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using ZoDream.Reader.Model;

namespace ZoDream.Reader.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// <para>
    /// See http://www.mvvmlight.net
    /// </para>
    /// </summary>
    public class ViewModelLocator
    {
        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            if (ViewModelBase.IsInDesignModeStatic)
            {
                SimpleIoc.Default.Register<IDataService, Design.DesignDataService>();
            }
            else
            {
                SimpleIoc.Default.Register<IDataService, DataService>();
            }

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<AddBookViewModel>();
            SimpleIoc.Default.Register<AddRuleViewModel>();
            SimpleIoc.Default.Register<AddWebViewModel>();
            SimpleIoc.Default.Register<ReadViewModel>();
            SimpleIoc.Default.Register<SystemViewModel>();
            SimpleIoc.Default.Register<WebRulesViewModel>();
            SimpleIoc.Default.Register<WebsiteViewModel>();
        }

        /// <summary>
        /// Gets the Main property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }

        /// <summary>
        /// Gets the Main property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public AddBookViewModel AddBook
        {
            get
            {
                return ServiceLocator.Current.GetInstance<AddBookViewModel>();
            }
        }

        /// <summary>
        /// Gets the AddRule property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public AddRuleViewModel AddRule
        {
            get
            {
                return ServiceLocator.Current.GetInstance<AddRuleViewModel>();
            }
        }

        /// <summary>
        /// Gets the AddWeb property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public AddWebViewModel AddWeb
        {
            get
            {
                return ServiceLocator.Current.GetInstance<AddWebViewModel>();
            }
        }

        /// <summary>
        /// Gets the Read property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public ReadViewModel Read
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ReadViewModel>();
            }
        }


        /// <summary>
        /// Gets the System property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public SystemViewModel System
        {
            get
            {
                return ServiceLocator.Current.GetInstance<SystemViewModel>();
            }
        }
        /// <summary>
        /// Gets the WebRules property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public WebRulesViewModel WebRules
        {
            get
            {
                return ServiceLocator.Current.GetInstance<WebRulesViewModel>();
            }   
        }

        /// <summary>
        /// Gets the Wesite property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public WebsiteViewModel Website
        {
            get
            {
                return ServiceLocator.Current.GetInstance<WebsiteViewModel>();
            }
        }

        /// <summary>
        /// Cleans up all the resources.
        /// </summary>
        public static void Cleanup()
        {
        }
    }
}