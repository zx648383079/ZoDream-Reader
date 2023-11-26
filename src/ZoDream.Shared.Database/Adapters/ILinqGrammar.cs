using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using ZoDream.Shared.Database.Models;

namespace ZoDream.Shared.Database.Adapters
{
    public interface ILinqGrammar
    {

        public string Visit(Expression exp);
    }
}
