using System;
using System.Collections.Generic;
using System.Text;

namespace ZoDream.Shared.Interfaces.Route
{
    public interface IRouter
    {
        public void RegisterRoute(string routeName, Type page);
        public void RegisterRoute(string routeName, Type page, Type viewModel);

        public void GoToAsync(string routeName, IDictionary<string, object> queries);

        public void GoToAsync(string routeName);
    }
}
