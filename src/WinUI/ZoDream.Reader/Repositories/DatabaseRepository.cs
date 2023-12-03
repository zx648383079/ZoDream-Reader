using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using ZoDream.Shared.Database;
using ZoDream.Shared.Interfaces;
using ZoDream.Shared.Interfaces.Entities;
using ZoDream.Shared.Models;
using ZoDream.Shared.Repositories;
using ZoDream.Shared.Repositories.Entities;
using ZoDream.Shared.Repositories.Extensions;
using ZoDream.Shared.Storage;
using ZoDream.Shared.Utils;

namespace ZoDream.Reader.Repositories
{
    public class DatabaseRepository : IDatabaseRepository
    {
        const int SortBegin = 10;
        public DatabaseRepository(StorageFile dbFile)
        {
            AppData.DefaultFileName = Path.Combine(ApplicationData.Current.LocalFolder.Path, "setting.xml");
            connection = new Database(new SqliteConnection($"Data Source={dbFile.Path}"));
        }
        private readonly IDatabase connection;


        public async Task<IAppOption> LoadSettingAsync()
        {
            var option = await AppData.LoadAsync<AppOption>();
            if (option == null)
            {
                return new AppOption();
            }
            return option;
        }

        public async Task SaveSettingAsync(IAppOption data)
        {
            await AppData.SaveAsync(data);
        }


        public void Dispose()
        {
            connection.Dispose();
        }


        public void Initialize()
        {
            new SQLMigration(connection).Up();
        }

        public Task<List<T>> GetBookAsync<T>() where T : INovel
        {
            return Task.FromResult(connection.Build<T>().From<BookEntity>().ToList());
        }

        public Task<T?> GetBookAsync<T>(string id) where T : INovel
        {
            return Task.FromResult(connection.Build<T>().Where("Id", id).First());
        }

        public Task SaveBookAsync(INovel item)
        {
            if (string.IsNullOrEmpty(item.Cover))
            {
                item.Cover = Utils.Converter.RandomCover();
            }
            var data = item is BookEntity o ? o :
                 item.Clone<BookEntity>();
            connection.Save(data);
            item.Id = data.Id;
            return Task.CompletedTask;
        }

        public Task DeleteBookAsync(params string[] items)
        {
            connection.Build<BookEntity>().WhereIn("Id", Array.ConvertAll(items, i => (object)i)).Delete();
            connection.Build<ChapterEntity>().WhereIn("BookId", Array.ConvertAll(items, i => (object)i)).Delete();
            return Task.CompletedTask;
        }

        public Task<List<T>> GetChapterAsync<T>(string bookId) where T : INovelChapter
        {
            return Task.FromResult(connection.Build<T>().From<ChapterEntity>().Where("BookId", bookId).ToList());
        }

        public Task<List<T>> GetThemeAsync<T>() where T : IAppTheme
        {
            return Task.FromResult(connection.Build<T>().From<AppThemeEntity>().ToList());
        }

        public Task<T?> GetThemeAsync<T>(int id) where T : IAppTheme
        {
            return Task.FromResult(connection.Build<T>().From<AppThemeEntity>().Where("Id", id).First());
        }

        public Task SaveThemeAsync(IAppTheme item)
        {
            var data = item is AppThemeEntity o ? o :
             item.Clone<AppThemeEntity>();
            connection.Save(data);
            item.Id = data.Id;
            return Task.CompletedTask;
        }

        public Task DeleteThemeAsync(params int[] items)
        {
            connection.Build<AppThemeEntity>().WhereIn("Id", Array.ConvertAll(items, i => (object)i)).Delete();
            return Task.CompletedTask;
        }

        public Task<List<T>> GetReadThemeAsync<T>() where T : IReadTheme
        {
            return Task.FromResult(connection.Build<T>().From<ReadThemeEntity>().ToList());
        }

        public Task<T?> GetReadThemeAsync<T>(int id) where T : IReadTheme
        {
            return Task.FromResult(connection.Build<T>().From<ReadThemeEntity>().Where("Id", id).First());
        }

        public Task SaveReadThemeAsync(IReadTheme item)
        {
            var data = item is ReadThemeEntity o ? o : item.Clone<ReadThemeEntity>();
            connection.Save(data);
            item.Id = data.Id;
            return Task.CompletedTask;
        }

        public Task DeleteReadThemeAsync(params int[] items)
        {
            connection.Build<ReadThemeEntity>().WhereIn("Id", Array.ConvertAll(items, i => (object)i)).Delete();
            return Task.CompletedTask;
        }

        public Task<List<T>> GetDictionaryRuleAsync<T>() where T : IDictionaryRule
        {
            return Task.FromResult(connection.Build<T>().From<DictionaryRuleEntity>().ToList());
        }

        public Task SaveDictionaryRuleAsync(IDictionaryRule item)
        {
            var data = item is DictionaryRuleEntity o ? o : item.Clone<DictionaryRuleEntity>();
            connection.Save(data);
            item.Id = data.Id;
            return Task.CompletedTask;
        }

        public Task DeleteDictionaryRuleAsync(params int[] items)
        {
            connection.Build<DictionaryRuleEntity>().WhereIn("Id", Array.ConvertAll(items, i => (object)i)).Delete();
            return Task.CompletedTask;
        }

        public Task<List<T>> GetReplaceRuleAsync<T>() where T : IReplaceRule
        {
            return Task.FromResult(connection.Build<T>().From<ReplaceRuleEntity>().ToList());
        }

        public Task SaveReplaceRuleAsync(IReplaceRule item)
        {
            var data = item is ReplaceRuleEntity o ? o : item.Clone<ReplaceRuleEntity>();
            connection.Save(data);
            item.Id = data.Id;
            return Task.CompletedTask;
        }

        public Task DeleteReplaceRuleAsync(params int[] items)
        {
            connection.Build<ReplaceRuleEntity>().WhereIn("Id", Array.ConvertAll(items, i => (object)i)).Delete();
            return Task.CompletedTask;
        }

        public Task<List<T>> GetChapterRuleAsync<T>() where T : IChapterRule
        {
            return Task.FromResult(connection.Build<T>().From<ChapterRuleEntity>().OrderByAsc("SortOrder")
                .OrderByAsc("Id").ToList());
        }

        public Task SaveChapterRuleAsync(IChapterRule item)
        {
            var data = item is ChapterRuleEntity o ? o :
                item.Clone<ChapterRuleEntity>();
            connection.Save(data);
            item.Id = data.Id;
            return Task.CompletedTask;
        }

        public Task DeleteChapterRuleAsync(int[] items)
        {
            connection.Build<ChapterRuleEntity>().WhereIn("Id", Array.ConvertAll(items, i => (object)i)).Delete();
            return Task.CompletedTask;
        }

        public Task ToggleChapterRuleAsync(bool enabled, params int[] items)
        {
            connection.Build<ChapterRuleEntity>().WhereIn("Id", Array.ConvertAll(items, i => (object)i))
                .Update("IsEnabled", enabled);
            return Task.CompletedTask;
        }

        public Task SortChapterRuleAsync<T>(IEnumerable<T> items) where T : IChapterRule
        {
            var sort = SortBegin;
            foreach (var item in items)
            {
                sort++;
                if (item.SortOrder == sort)
                {
                    continue;
                }
                item.SortOrder = sort;
                connection.Build<ChapterRuleEntity>().Where(nameof(item.Id), item.Id)
                 .Update(nameof(item.SortOrder), sort);
            }
            return Task.CompletedTask;
        }

        public Task<List<T>> GetSourceRuleAsync<T>() where T : ISourceRule
        {
            return Task.FromResult(connection.Build<T>().From<SourceRuleEntity>().ToList());
        }

        public Task<T?> GetSourceRuleAsync<T>(int id) where T : ISourceRule
        {
            return Task.FromResult(connection.Build<T>().From<SourceRuleEntity>().Where("Id", id).First());
        }

        public Task SaveSourceRuleAsync(ISourceRule item)
        {
            var data = item is SourceRuleEntity o ? o : item.Clone<SourceRuleEntity>();
            connection.Save(data);
            item.Id = data.Id;
            return Task.CompletedTask;
        }

        public Task DeleteSourceRuleAsync(params int[] items)
        {
            connection.Build<SourceRuleEntity>().WhereIn("Id", Array.ConvertAll(items, i => (object)i)).Delete();
            return Task.CompletedTask;
        }

        public Task<List<T>> GetTTSSourceAsync<T>() where T : ITextToSpeech
        {
            return Task.FromResult(connection.Build<T>().From<TextToSpeechEntity>().ToList());
        }

        public Task<T?> GetTTSSourceAsync<T>(int id) where T : ITextToSpeech
        {
            return Task.FromResult(connection.Build<T>().From<TextToSpeechEntity>().Where("Id", id).First());
        }

        public Task SaveTTSSourceAsync(ITextToSpeech item)
        {
            var data = item is TextToSpeechEntity o ? o : item.Clone<TextToSpeechEntity>();
            connection.Save(data);
            item.Id = data.Id;
            return Task.CompletedTask;
        }

        public Task DeleteTTSSourceAsync(params int[] items)
        {
            connection.Build<TextToSpeechEntity>().WhereIn("Id", Array.ConvertAll(items, i => (object)i)).Delete();
            return Task.CompletedTask;
        }

        public Task<List<T>> GetSearchRecordAsync<T>() where T : ISearchHistory
        {
            return Task.FromResult(connection.Build<T>().From<SearchHistoryEntity>().ToList());
        }

        public Task SaveSearchRecordAsync(string word)
        {
            var data = connection.Build<SearchHistoryEntity>().Where("Word", word).First();
            if (data is null)
            {
                connection.Insert(new SearchHistoryEntity()
                {
                    Word = word,
                    UseCount = 1,
                    LastUseAt = Time.TimestampFrom(DateTime.Now)
                });
            } else
            {
                data.UseCount++;
                data.LastUseAt = Time.TimestampFrom(DateTime.Now);
                connection.Update(data);
            }
            return Task.CompletedTask;
        }

        public Task ClearSearchRecordAsync()
        {
            connection.Build<SearchHistoryEntity>().Delete();
            return Task.CompletedTask;
        }

        public Task<List<T>> GetReadRecordAsync<T>() where T : IReadRecord
        {
            return Task.FromResult(connection.Build<T>().From<ReadRecordEntity>().ToList());
        }

        public Task SaveReadRecordAsync(INovel novel, long time)
        {
            var data = connection.Build<ReadRecordEntity>().Where("Name", novel.Name).First();
            if (data is null)
            {
                connection.Insert(new ReadRecordEntity()
                {
                    Name = novel.Name,
                    ReadTime = time,
                    LastReadAt = Time.TimestampFrom(DateTime.Now)
                });
            }
            else
            {
                data.ReadTime += time;
                data.LastReadAt = Time.TimestampFrom(DateTime.Now);
                connection.Update(data);
            }
            return Task.CompletedTask;
        }

        public Task ClearReadRecordAsync()
        {
            connection.Build<ReadRecordEntity>().Delete();
            return Task.CompletedTask;
        }

        public Task<List<T>> GetMarkAsync<T>() where T : INovelMark
        {
            return Task.FromResult(connection.Build<T>().From<BookmarkEntity>().ToList());
        }

        public Task AddMarkAsync(INovel novel, INovelMark mark)
        {
            if (mark is not BookmarkEntity data)
            {
                data = mark.Clone<BookmarkEntity>();
            }
            data.BookName = novel.Name;
            connection.Save(data);
            return Task.CompletedTask;
        }

        public Task DeleteMarkAsync(params int[] items)
        {
            connection.Build<BookmarkEntity>().WhereIn("Id", Array.ConvertAll(items, i => (object)i)).Delete();
            return Task.CompletedTask;
        }

        public Task ToggleDictionaryRuleAsync(bool enabled, params int[] items)
        {
            connection.Build<DictionaryRuleEntity>().WhereIn("Id", Array.ConvertAll(items, i => (object)i))
                .Update("IsEnabled", enabled);
            return Task.CompletedTask;
        }

        public Task ToggleReplaceRuleAsync(bool enabled, params int[] items)
        {
            connection.Build<ReplaceRuleEntity>().WhereIn("Id", Array.ConvertAll(items, i => (object)i))
                .Update("IsEnabled", enabled);
            return Task.CompletedTask;
        }

        public Task ToggleSourceRuleAsync(bool enabled, params int[] items)
        {
            connection.Build<SourceRuleEntity>().WhereIn("Id", Array.ConvertAll(items, i => (object)i))
                .Update("IsEnabled", enabled);
            return Task.CompletedTask;
        }

        public Task ToggleTTSSourceAsync(bool enabled, params int[] items)
        {
            connection.Build<TextToSpeechEntity>().WhereIn("Id", Array.ConvertAll(items, i => (object)i))
                .Update("IsEnabled", enabled);
            return Task.CompletedTask;
        }

        public Task SortDictionaryRuleAsync<T>(IEnumerable<T> items) where T : IDictionaryRule
        {
            var sort = SortBegin;
            foreach (var item in items)
            {
                sort++;
                if (item.SortOrder == sort)
                {
                    continue;
                }
                item.SortOrder = sort;
                connection.Build<DictionaryRuleEntity>().Where(nameof(item.Id), item.Id)
                 .Update(nameof(item.SortOrder), sort);
            }
            return Task.CompletedTask;
        }

        public Task SortReplaceRuleAsync<T>(IEnumerable<T> items) where T : IReplaceRule
        {
            var sort = SortBegin;
            foreach (var item in items)
            {
                sort++;
                if (item.SortOrder == sort)
                {
                    continue;
                }
                item.SortOrder = sort;
                connection.Build<ReplaceRuleEntity>().Where(nameof(item.Id), item.Id)
                 .Update(nameof(item.SortOrder), sort);
            }
            return Task.CompletedTask;
        }

        public Task SortSourceRuleAsync<T>(IEnumerable<T> items) where T : ISourceRule
        {
            var sort = SortBegin;
            foreach (var item in items)
            {
                sort++;
                if (item.SortOrder == sort)
                {
                    continue;
                }
                item.SortOrder = sort;
                connection.Build<SourceRuleEntity>().Where(nameof(item.Id), item.Id)
                 .Update(nameof(item.SortOrder), sort);
            }
            return Task.CompletedTask;
        }

        public Task SortTTSSourceAsync<T>(IEnumerable<T> items) where T : ITextToSpeech
        {
            var sort = SortBegin;
            foreach (var item in items)
            {
                sort++;
                if (item.SortOrder == sort)
                {
                    continue;
                }
                item.SortOrder = sort;
                connection.Build<TextToSpeechEntity>().Where(nameof(item.Id), item.Id)
                 .Update(nameof(item.SortOrder), sort);
            }
            return Task.CompletedTask;
        }
    }
}
