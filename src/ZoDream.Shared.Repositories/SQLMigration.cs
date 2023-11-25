using System;
using System.Collections.Generic;
using System.Text;
using ZoDream.Shared.Database.Migrations;
using ZoDream.Shared.Repositories.Entities;

namespace ZoDream.Shared.Repositories
{
    public class SQLMigration: Migration
    {
        public string Up()
        {
            var sb = new StringBuilder();
            CreateTable<ReplaceRuleEntity>(sb);
            CreateTable<DictionaryRuleEntity>(sb);
            return sb.ToString();
        }

        public string Down()
        {
            return string.Empty;
        }

        public string Seed()
        {
            return string.Empty;
        }


    }
}
