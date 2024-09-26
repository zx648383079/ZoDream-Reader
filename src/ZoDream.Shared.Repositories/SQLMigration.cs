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
            CreateTable<AppThemeEntity>(sb);
            CreateTable<CacheEntity>(sb);
            CreateTable<CookieEntity>(sb);
            CreateTable<ChapterRuleEntity>(sb);
            CreateTable<ReplaceRuleEntity>(sb);
            CreateTable<DictionaryRuleEntity>(sb);
            CreateTable<ReadRecordEntity>(sb);
            CreateTable<ReadThemeEntity>(sb);
            CreateTable<SearchHistoryEntity>(sb);
            CreateTable<SourceRuleEntity>(sb);
            CreateTable<SubscribeSourceEntity>(sb);
            CreateTable<TextToSpeechEntity>(sb);
            CreateTable<BookEntity>(sb);
            CreateTable<BookGroupEntity>(sb);
            CreateTable<ChapterEntity>(sb);
            Database.Execute(sb.ToString());
        }

        public override void Down()
        {
            var sb = new StringBuilder();
            DropTable<AppThemeEntity>(sb);
            DropTable<CacheEntity>(sb);
            DropTable<CookieEntity>(sb);
            DropTable<ChapterRuleEntity>(sb);
            DropTable<ReplaceRuleEntity>(sb);
            DropTable<DictionaryRuleEntity>(sb);
            DropTable<ReadRecordEntity>(sb);
            DropTable<ReadThemeEntity>(sb);
            DropTable<SearchHistoryEntity>(sb);
            DropTable<SourceRuleEntity>(sb);
            DropTable<SubscribeSourceEntity>(sb);
            DropTable<TextToSpeechEntity>(sb);
            DropTable<BookEntity>(sb);
            DropTable<BookGroupEntity>(sb);
            DropTable<ChapterEntity>(sb);
            Database.Execute(sb.ToString());
        }

        public override void Seed()
        {
            //Database.Insert<ReplaceRuleEntity>([
            //    new()
            //    {
                    
            //    }
            //]);
        }

     
    }
}
