using System;
using System.Collections.Generic;
using System.Text;

namespace ZoDream.Shared.Database.Migrations
{
    public interface IMigration
    {
        public void Up();

        public void Down();

        public void Seed();

        public string CreateTable<T>() where T : class;
        public string DropTable<T>() where T : class;
        public void CreateTable<T>(StringBuilder builder) where T : class;
        public void DropTable<T>(StringBuilder builder) where T : class;
    }
}
