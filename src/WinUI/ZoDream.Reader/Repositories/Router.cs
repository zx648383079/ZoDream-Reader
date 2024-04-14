using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using ZoDream.Shared.Interfaces.Route;
using ZoDream.Reader.ViewModels;
using System.Linq;
using Microsoft.UI.Xaml.Navigation;

namespace ZoDream.Reader.Repositories
{
    public class Router: IRouter
    {
        public const string HomeRoute = "home";
        public const string MainRoute = "main";

        private Frame? MainFrame;
        private Frame? SingleFrame;
        private Frame? InnerFrame;
        private readonly Dictionary<string, RouteItem> Routes = [];

        private readonly List<string> Histories = [];

        public RouteItem? CurrentRoute => Histories.Count == 0 ? null : 
            Routes[Histories.Last()];

        public object? CurrentPage 
        {
            get {
                if (MainFrame is null)
                {
                    return null;
                }
                var route = CurrentRoute!;
                return route.RouteType switch
                {
                    RouteType.Single => SingleFrame?.Content,
                    RouteType.None => InnerFrame?.Content,
                    _ => MainFrame?.Content,
                };
            }
        }
        public object? CurrentRootPage {
            get {
                if (MainFrame is null)
                {
                    return null;
                }
                var route = CurrentRoute!;
                return route.RouteType switch
                {
                    RouteType.Single => SingleFrame?.Content,
                    _ => MainFrame?.Content,
                };
            }
        }

        public event RoutedEventHandler? RouteChanged;

        public bool IsBackVisible => CanGoBack;

        public bool CanGoBack => Histories.Count > 1 || Histories.Last() != HomeRoute;

        public void RegisterRoute(string routeName, Type page)
        {
            RegisterRoute(routeName, page, null, RouteType.Main);
        }

        public void RegisterRoute(string routeName, Type page, bool isInner)
        {
            RegisterRoute(routeName, page, isInner ? RouteType.None : RouteType.Main);
        }
        public void RegisterRoute(string routeName, Type page, RouteType routeType)
        {
            RegisterRoute(routeName, page, null, routeType);
        }

        public void RegisterRoute(string routeName, Type page, Type viewModel)
        {
            RegisterRoute(routeName, page, viewModel, RouteType.None);
        }

        public void RegisterRoute(string routeName, Type page, Type? viewModel, RouteType routeType)
        {
            var route = new RouteItem(routeName, page, viewModel, routeType);
            if (!Routes.TryAdd(routeName, route))
            {
                Routes[routeName] = route;
            }
        }

        public void GoToAsync(string routeName, IDictionary<string, object>? queries)
        {
            if (CurrentRoute?.RouteName == routeName)
            {
                return;
            }
            Navigate(routeName, queries);
        }

        public void GoToAsync(string routeName)
        {
            GoToAsync(routeName, null);
        }

        private void Navigate(RouteItem route, IDictionary<string, object>? queries = null)
        {
            App.GetService<AppViewModel>().DispatcherQueue?.TryEnqueue(() => {
                switch (route.RouteType)
                {
                    case RouteType.Main:
                        ToggleFrame(true);
                        MainFrame?.Navigate(route.PageType, queries);
                        CallViewModel(MainFrame, queries);
                        break;
                    case RouteType.Single:
                        ToggleFrame(false);
                        SingleFrame?.Navigate(route.PageType, queries);
                        CallViewModel(SingleFrame, queries);
                        break;
                    case RouteType.None:
                    default:
                        ToggleFrame(true);
                        if (InnerFrame is null)
                        {
                            NavigatePage(MainFrame, MainRoute);
                            break;
                        }
                        InnerFrame.Navigate(route.PageType, queries);
                        CallViewModel(InnerFrame, queries);
                        break;
                }
            });
            if (Histories.Count == 0 || Histories.Last() != route.RouteName)
            {
                Histories.Add(route.RouteName);
            }
            RouteChanged?.Invoke(this, null);
        }

        private void CallViewModel(Frame? frame, IDictionary<string, object>? queries = null)
        {
            if (frame is null || queries is null)
            {
                return;
            }
            void call(object s, NavigationEventArgs o)
            {
                frame.Navigated -= call;
                if (frame.Content is FrameworkElement target && 
                    target.DataContext is IQueryAttributable vm)
                {
                    vm.ApplyQueryAttributes(queries);
                }
            }
            frame.Navigated += call;
        }

        private void NavigatePage(Frame? frame, string routeName)
        {
            if (!Routes.TryGetValue(routeName, out var route))
            {
                return;
            }
            frame?.Navigate(route.PageType);
        }

        private RouteType GetType(string routeName)
        {
            if (!Routes.TryGetValue(routeName, out var route))
            {
                return RouteType.None;
            }
            return route.RouteType;
        }

        private void Navigate(string routeName, IDictionary<string, object>? queries = null)
        {
            if (MainFrame is null)
            {
                Histories.Add(routeName);
                return;
            }
            if (!Routes.TryGetValue(routeName, out var route))
            {
                return;
            }
            Navigate(route, queries);
            //var page = CreatePage(route);
            //if (!InnerFrame.Navigate(page))
            //{
            //    return;
            //}
            //if (page is FrameworkElement o)
            //{
            //    Current = o;
            //    CurrentRoute = route;
            //}
            //if (route.DataContext is not null && Current is not null)
            //{
            //    if (Current.DataContext is null)
            //    {
            //        Current.DataContext = Activator.CreateInstance(route.DataContext);
            //    }
            //}
        }

        //private object? CreatePage(ShellRoute route)
        //{
        //    if (Histories.TryGetValue(route.Name, out var page))
        //    {
        //        return page;
        //    }
        //    var instance = Activator.CreateInstance(route.Page);
        //    if (instance is FrameworkElement o)
        //    {
        //        Histories.Add(route.Name, o);
        //    }
        //    return instance;
        //}

        public void GoBackAsync()
        {
            if (Histories.Count < 1)
            {
                Histories.Clear();
                GoToAsync(HomeRoute);
                return;
            }
            var last = CurrentRoute!;
            Histories.RemoveAt(Histories.Count - 1);
            App.GetService<AppViewModel>().DispatcherQueue?.TryEnqueue(() => {
                if (last.RouteType == RouteType.Single)
                {
                    if (CurrentRoute?.RouteType == RouteType.Single)
                    {
                        SingleFrame?.GoBack();
                        return;
                    }
                    ToggleFrame(true);
                    return;
                }
                if (InnerFrame is not null && InnerFrame.CanGoBack)
                {
                    InnerFrame.GoBack();
                }
            });
            RouteChanged?.Invoke(this, null);
        }

        private void ToggleFrame(bool isMain)
        {
            if (MainFrame is null || SingleFrame is null)
            {
                return;
            }
            if (isMain == (MainFrame.Visibility == Visibility.Visible))
            {
                return;
            }
            MainFrame.Visibility = isMain ? Visibility.Visible : Visibility.Collapsed;
            SingleFrame.Visibility = isMain ? Visibility.Collapsed : Visibility.Visible;
        }

        public void BindMain(Frame frame, Frame singleFrame)
        {
            MainFrame = frame;
            SingleFrame = singleFrame;
            if (frame is null || Histories.Count == 0)
            {
                return;
            }
            Navigate(CurrentRoute!);
        }

        public void BindInner(Frame? frame)
        {
            InnerFrame = frame;
            if (frame is null || Histories.Count == 0)
            {
                return;
            }
            Navigate(CurrentRoute!);
        }

        public void Dispose()
        {
            Routes.Clear();
            Histories.Clear();
        }
    }

    public class RouteItem
    {
        public string RouteName { get; private set; } = string.Empty;
        public Type PageType { get; private set; }

        public Type? ViewType { get; private set; }

        public RouteType RouteType { get; private set; } = RouteType.None;

        public RouteItem(string name, Type pageType)
        {
            RouteName = name;
            PageType = pageType;
        }

        public RouteItem(string name, Type pageType, RouteType routeType)
            : this(name, pageType)
        {
            RouteType = routeType;
        }

        public RouteItem(string name, Type pageType, Type? viewType, RouteType routeType)
            : this(name, pageType, routeType)
        {
            ViewType = viewType;
        }
    }

    public enum RouteType
    {
        None,
        Main,
        Single,
    }


}
