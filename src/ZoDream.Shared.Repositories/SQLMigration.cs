using System.Text;
using ZoDream.Shared.Database;
using ZoDream.Shared.Database.Migrations;
using ZoDream.Shared.Repositories.Entities;

namespace ZoDream.Shared.Repositories
{
    public class SQLMigration(IDatabase database) : Migration(database)
    {
        public override void Up()
        {
            var sb = new StringBuilder();
            CreateTable<ChapterRuleEntity>(sb);
            CreateTable<ReplaceRuleEntity>(sb);
            CreateTable<DictionaryRuleEntity>(sb);
            Database.Execute(sb.ToString());
        }

        public override void Down()
        {
            var sb = new StringBuilder();
            DropTable<ChapterRuleEntity>(sb);
            DropTable<ReplaceRuleEntity>(sb);
            DropTable<DictionaryRuleEntity>(sb);
            Database.Execute(sb.ToString());
        }

        public override void Seed()
        {
            Database.Insert<ReplaceRuleEntity>([
                new()
                {
                    
                }
            ]);
        }

     
    }
}
