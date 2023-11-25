using System;
using System.Collections.Generic;
using System.Text;

namespace ZoDream.Shared.Database.Migrations
{
    public interface IMigration
    {

        public string CreateTable<T>() where T : class;
        public void CreateTable<T>(StringBuilder builder) where T : class;
    }
}
