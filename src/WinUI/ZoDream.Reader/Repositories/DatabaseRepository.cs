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

        public Task<T?> GetBookAsync<T>(object id) where T : INovel
        {
            return Task.FromResult(connection.Build<T>().Where("Id", id).First());
        }

        public Task SaveBookAsync(INovel item)
        {
            if (string.IsNullOrEmpty(item.Cover))
            {
                item.Cover = Utils.Converter.RandomCover();
            }
            if (item is not BookEntity)
            {
                item = item.Clone<BookEntity>();
            }
            connection.Save(item);
            return Task.CompletedTask;
        }

        public Task DeleteBookAsync(object id)
        {
            connection.Delete<BookEntity>(id);
            return Task.CompletedTask;
        }

        public Task<List<T>> GetChapterAsync<T>(object bookId) where T : INovelChapter
        {
            return Task.FromResult(connection.Build<T>().From<ChapterEntity>().Where("BookId", bookId).ToList());
        }

        public Task<List<T>> GetThemeAsync<T>() where T : IAppTheme
        {
            return Task.FromResult(connection.Build<T>().From<AppThemeEntity>().ToList());
        }

        public Task<T?> GetThemeAsync<T>(object id) where T : IAppTheme
        {
            return Task.FromResult(connection.Build<T>().From<AppThemeEntity>().Where("Id", id).First());
        }

        public Task SaveThemeAsync(IAppTheme item)
        {
            throw new NotImplementedException();
        }

        public Task DeleteThemeAsync(object id)
        {
            connection.Delete<AppThemeEntity>(id);
            return Task.CompletedTask;
        }

        public Task<List<T>> GetReadThemeAsync<T>() where T : IReadTheme
        {
            return Task.FromResult(connection.Build<T>().From<ReadThemeEntity>().ToList());
        }

        public Task<T?> GetReadThemeAsync<T>(object id) where T : IReadTheme
        {
            return Task.FromResult(connection.Build<T>().From<ReadThemeEntity>().Where("Id", id).First());
        }

        public Task SaveReadThemeAsync(IReadTheme item)
        {
            if (item is not ReadThemeEntity)
            {
                item = item.Clone<ReadThemeEntity>();
            }
            connection.Save(item);
            return Task.CompletedTask;
        }

        public Task DeleteReadThemeAsync(object id)
        {
            connection.Delete<ReadThemeEntity>(id);
            return Task.CompletedTask;
        }

        public Task<List<T>> GetDictionaryRuleAsync<T>() where T : IDictionaryRule
        {
            return Task.FromResult(connection.Build<T>().From<DictionaryRuleEntity>().ToList());
        }

        public Task SaveDictionaryRuleAsync(IDictionaryRule item)
        {
            if (item is not DictionaryRuleEntity)
            {
                item = item.Clone<DictionaryRuleEntity>();
            }
            connection.Save(item);
            return Task.CompletedTask;
        }

        public Task DeleteDictionaryRuleAsync(object id)
        {
            connection.Delete<DictionaryRuleEntity>(id);
            return Task.CompletedTask;
        }

        public Task<List<T>> GetReplaceRuleAsync<T>() where T : IReplaceRule
        {
            return Task.FromResult(connection.Build<T>().From<ReplaceRuleEntity>().ToList());
        }

        public Task SaveReplaceRuleAsync(IReplaceRule item)
        {
            if (item is not ReplaceRuleEntity)
            {
                item = item.Clone<ReplaceRuleEntity>();
            }
            connection.Save(item);
            return Task.CompletedTask;
        }

        public Task DeleteReplaceRuleAsync(object id)
        {
            connection.Delete<ReplaceRuleEntity>(id);
            return Task.CompletedTask;
        }

        public Task<List<T>> GetChapterRuleAsync<T>() where T : IChapterRule
        {
            return Task.FromResult(connection.Build<T>().From<ChapterRuleEntity>().ToList());
        }

        public Task SaveChapterRuleAsync(IChapterRule item)
        {
            if (item is not ChapterRuleEntity)
            {
                item = item.Clone<ChapterRuleEntity>();
            }
            connection.Save(item);
            return Task.CompletedTask;
        }

        public Task DeleteChapterRuleAsync(object id)
        {
            connection.Delete<ChapterRuleEntity>(id);
            return Task.CompletedTask;
        }

        public Task<List<T>> GetSourceRuleAsync<T>() where T : ISourceRule
        {
            return Task.FromResult(connection.Build<T>().From<SourceRuleEntity>().ToList());
        }

        public Task<T?> GetSourceRuleAsync<T>(object id) where T : ISourceRule
        {
            return Task.FromResult(connection.Build<T>().From<SourceRuleEntity>().Where("Id", id).First());
        }

        public Task SaveSourceRuleAsync(ISourceRule item)
        {
            if (item is not SourceRuleEntity)
            {
                item = item.Clone<SourceRuleEntity>();
            }
            connection.Save(item);
            return Task.CompletedTask;
        }

        public Task DeleteSourceRuleAsync(object id)
        {
            connection.Delete<SourceRuleEntity>(id);
            return Task.CompletedTask;
        }

        public Task<List<T>> GetTTSSourceAsync<T>() where T : ITextToSpeech
        {
            return Task.FromResult(connection.Build<T>().From<TextToSpeechEntity>().ToList());
        }

        public Task<T?> GetTTSSourceAsync<T>(object id) where T : ITextToSpeech
        {
            return Task.FromResult(connection.Build<T>().From<TextToSpeechEntity>().Where("Id", id).First());
        }

        public Task SaveTTSSourceAsync(ITextToSpeech item)
        {
            if (item is not TextToSpeechEntity)
            {
                item = item.Clone<TextToSpeechEntity>();
            }
            connection.Save(item);
            return Task.CompletedTask;
        }

        public Task DeleteTTSSourceAsync(object id)
        {
            connection.Delete<TextToSpeechEntity>(id);
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

        public Task DeleteMarkAsync(object id)
        {
            connection.Delete<BookmarkEntity>(id);
            return Task.CompletedTask;
        }

    }
}
