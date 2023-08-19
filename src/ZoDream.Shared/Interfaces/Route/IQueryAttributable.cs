using System;
using System.Collections.Generic;
using System.Text;

namespace ZoDream.Shared.Interfaces.Route
{
    public interface IQueryAttributable
    {
        public void ApplyQueryAttributes(IDictionary<string, object> queries);
    }
}
