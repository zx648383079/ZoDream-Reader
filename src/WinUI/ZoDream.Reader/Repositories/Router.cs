using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZoDream.Shared.Interfaces.Route;
using System.Collections;
using ZoDream.Reader.Pages;
using ZoDream.Reader.ViewModels;

namespace ZoDream.Reader.Repositories
{
    public class Router: IRouter
    {
        private Frame MainFrame;
        private Frame SingleFrame;
        private Frame InnerFrame;
        private readonly Dictionary<string, RouteItem> Routes = new();

        private string CurrentRoute = string.Empty;

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

        public void RegisterRoute(string routeName, Type page, Type viewModel, RouteType routeType)
        {
            var route = new RouteItem()
            {
                RouteName = routeName,
                PageType = page,
                ViewType = viewModel,
                RouteType = routeType
            };
            if (!Routes.TryAdd(routeName, route))
            {
                Routes[routeName] = route;
            }
        }

        public void GoToAsync(string routeName, IDictionary<string, object> queries)
        {
            GoToAsync(routeName);
            var type = GetType(routeName);
            var current = type switch
            {
                RouteType.Main => MainFrame?.Content,
                RouteType.Single => SingleFrame?.Content,
                _ => InnerFrame?.Content,
            } as FrameworkElement;
            if (current is not null && current.DataContext is IQueryAttributable o)
            {
                o.ApplyQueryAttributes(queries);
            }
        }

        public void GoToAsync(string routeName)
        {
            if (CurrentRoute == routeName)
            {
                return;
            }
            Navigate(routeName);
        }

        private void Navigate(RouteItem route)
        {
            App.GetService<AppViewModel>().DispatcherQueue?.TryEnqueue(() => {
                switch (route.RouteType)
                {
                    case RouteType.Main:
                        ToggleFrame(true);
                        MainFrame?.Navigate(route.PageType);
                        break;
                    case RouteType.Single:
                        ToggleFrame(false);
                        SingleFrame.Navigate(route.PageType);
                        break;
                    case RouteType.None:
                    default:
                        ToggleFrame(true);
                        if (InnerFrame is null)
                        {
                            MainFrame.Navigate(typeof(MainPage));
                            return;
                        }
                        InnerFrame.Navigate(route.PageType);
                        break;
                }
            });
            CurrentRoute = route.RouteName;
        }

        private RouteType GetType(string routeName)
        {
            if (!Routes.TryGetValue(routeName, out var route))
            {
                return RouteType.None;
            }
            return route.RouteType;
        }

        private void Navigate(string routeName)
        {
            if (MainFrame is null)
            {
                CurrentRoute = routeName;
                return;
            }
            if (!Routes.TryGetValue(routeName, out var route))
            {
                return;
            }
            Navigate(route);
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
            if (frame is null || string.IsNullOrEmpty(CurrentRoute))
            {
                return;
            }
            Navigate(CurrentRoute);
        }

        public void BindInner(Frame frame)
        {
            InnerFrame = frame;
            if (frame is null || string.IsNullOrEmpty(CurrentRoute))
            {
                return;
            }
            Navigate(CurrentRoute);
        }

        public void Dispose()
        {
            Routes.Clear();
            // Histories.Clear();
        }

        private class RouteItem
        {
            public string RouteName { get; set; } = string.Empty;
            public Type PageType { get; set; }

            public Type ViewType { get; set; }

            public RouteType RouteType { get; set; } = RouteType.None;

        }
    }

    public enum RouteType
    {
        None,
        Main,
        Single,
    }


}
