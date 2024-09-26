using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using ZoDream.Reader.ViewModels;
using ZoDream.Shared.Database;
using ZoDream.Shared.Interfaces;
using ZoDream.Shared.Interfaces.Entities;
using ZoDream.Shared.Models;
using ZoDream.Shared.Plugins.EPub;
using ZoDream.Shared.Plugins.Net;
using ZoDream.Shared.Plugins.Pdf;
using ZoDream.Shared.Plugins.Txt;
using ZoDream.Shared.Plugins.Umd;
using ZoDream.Shared.Repositories;
using ZoDream.Shared.Repositories.Entities;
using ZoDream.Shared.Repositories.Extensions;
using ZoDream.Shared.Storage;
using ZoDream.Shared.Tokenizers;
using ZoDream.Shared.Utils;

namespace ZoDream.Reader.Repositories
{
    public class DatabaseRepository : IDatabaseRepository, ICache
    {
        const int SortBegin = 10;
        public DatabaseRepository(StorageFile dbFile)
        {
            AppData.DefaultFileName = Path.Combine(ApplicationData.Current.LocalFolder.Path, "setting.xml");
            _connection = new Database(new SqliteConnection($"Data Source={dbFile.Path}"));
        }
        private readonly IDatabase _connection;


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
            _connection.Dispose();
        }


        public void Initialize()
        {
            new SQLMigration(_connection).Up();
        }

        public Task<IList<T>> GetBookAsync<T>() where T : INovel
        {
            return Task.FromResult(_connection.Build<T>().From<BookEntity>().ToList());
        }

        public Task<T?> GetBookAsync<T>(string id) where T : INovel
        {
            return Task.FromResult(_connection.Build<T>().Where("Id", id).First());
        }

        public Task SaveBookAsync(INovel item)
        {
            if (string.IsNullOrEmpty(item.Cover))
            {
                item.Cover = Utils.Converter.RandomCover();
            }
            var data = item is BookEntity o ? o :
                 item.Clone<BookEntity>();
            var insert = string.IsNullOrEmpty(data.Id);
            if (!insert)
            {
                insert = _connection.Build<BookEntity>().Where("Id", data.Id).Count() == 0;
            }
            if (insert)
            {
                _connection.Insert(data);
                item.Id = data.Id;
            } else
            {
                _connection.Update(data);
            }
            return Task.CompletedTask;
        }

        public Task DeleteBookAsync(params string[] items)
        {
            _connection.Build<BookEntity>().WhereIn("Id", Array.ConvertAll(items, i => (object)i)).Delete();
            _connection.Build<ChapterEntity>().WhereIn("BookId", Array.ConvertAll(items, i => (object)i)).Delete();
            return Task.CompletedTask;
        }

        public Task<IList<T>> GetChapterAsync<T>(string bookId) where T : INovelChapter
        {
            return Task.FromResult(_connection.Build<T>().From<ChapterEntity>().Where("BookId", bookId).OrderByAsc("Index").ToList());
        }

        public async Task SaveChapterAsync<T>(string bookId, IEnumerable<T> items) 
            where T : INovelChapter
        {
            var data = await GetChapterAsync<T>(bookId);
            var i = 0;
            foreach (var item in items)
            {
                ++i;
                item.Index = i;
                item.BookId = bookId;
                if (data.Count >= i)
                {
                    item.Id = data[i - 1].Id;
                }
                var entity = item is ChapterEntity o ? o :
                        item.Clone<ChapterEntity>();
                _connection.Save(entity);
                item.Id = entity.Id;
            }
            if (data.Count <= i)
            {
                return;
            }
            _connection.Build<ChapterEntity>().WhereIn("Id", 
                data.Skip(i).Select(item => (object)item.Id).ToArray()).Delete();
        }

        public Task<IList<T>> GetThemeAsync<T>() where T : IAppTheme
        {
            return Task.FromResult(_connection.Build<T>().From<AppThemeEntity>().ToList());
        }

        public Task<T?> GetThemeAsync<T>(int id) where T : IAppTheme
        {
            return Task.FromResult(_connection.Build<T>().From<AppThemeEntity>().Where("Id", id).First());
        }

        public Task SaveThemeAsync(IAppTheme item)
        {
            var data = item is AppThemeEntity o ? o :
             item.Clone<AppThemeEntity>();
            _connection.Save(data);
            item.Id = data.Id;
            return Task.CompletedTask;
        }

        public Task DeleteThemeAsync(params int[] items)
        {
            _connection.Build<AppThemeEntity>().WhereIn("Id", Array.ConvertAll(items, i => (object)i)).Delete();
            return Task.CompletedTask;
        }

        public Task<IList<T>> GetReadThemeAsync<T>() where T : IReadTheme
        {
            return Task.FromResult(_connection.Build<T>().From<ReadThemeEntity>().ToList());
        }

        public Task<T?> GetReadThemeAsync<T>(int id) where T : IReadTheme
        {
            return Task.FromResult(_connection.Build<T>().From<ReadThemeEntity>().Where("Id", id).First());
        }

        public Task SaveReadThemeAsync(IReadTheme item)
        {
            var data = item is ReadThemeEntity o ? o : item.Clone<ReadThemeEntity>();
            _connection.Save(data);
            item.Id = data.Id;
            return Task.CompletedTask;
        }

        public Task DeleteReadThemeAsync(params int[] items)
        {
            _connection.Build<ReadThemeEntity>().WhereIn("Id", Array.ConvertAll(items, i => (object)i)).Delete();
            return Task.CompletedTask;
        }

        public Task<IList<T>> GetDictionaryRuleAsync<T>() where T : IDictionaryRule
        {
            return Task.FromResult(_connection.Build<T>().From<DictionaryRuleEntity>().ToList());
        }

        public Task SaveDictionaryRuleAsync(IDictionaryRule item)
        {
            var data = item is DictionaryRuleEntity o ? o : item.Clone<DictionaryRuleEntity>();
            _connection.Save(data);
            item.Id = data.Id;
            return Task.CompletedTask;
        }

        public Task DeleteDictionaryRuleAsync(params int[] items)
        {
            _connection.Build<DictionaryRuleEntity>().WhereIn("Id", Array.ConvertAll(items, i => (object)i)).Delete();
            return Task.CompletedTask;
        }

        public Task<IList<T>> GetReplaceRuleAsync<T>() where T : IReplaceRule
        {
            return Task.FromResult(_connection.Build<T>().From<ReplaceRuleEntity>().ToList());
        }

        public Task SaveReplaceRuleAsync(IReplaceRule item)
        {
            var data = item is ReplaceRuleEntity o ? o : item.Clone<ReplaceRuleEntity>();
            _connection.Save(data);
            item.Id = data.Id;
            return Task.CompletedTask;
        }

        public Task DeleteReplaceRuleAsync(params int[] items)
        {
            _connection.Build<ReplaceRuleEntity>().WhereIn("Id", Array.ConvertAll(items, i => (object)i)).Delete();
            return Task.CompletedTask;
        }

        public Task<IList<T>> GetChapterRuleAsync<T>() where T : IChapterRule
        {
            return Task.FromResult(_connection.Build<T>().From<ChapterRuleEntity>().OrderByAsc("SortOrder")
                .OrderByAsc("Id").ToList());
        }

        public Task<string[]> GetEnabledChapterRuleAsync()
        {
            return Task.FromResult(_connection.Build<ChapterRuleEntity>()
                .From<ChapterRuleEntity>().Where("IsEnabled", 1).OrderByAsc("SortOrder")
                .OrderByAsc("Id").Select("MatchRule").ToList().Select(item => item.MatchRule).ToArray());
        }

        public Task SaveChapterRuleAsync(IChapterRule item)
        {
            var data = item is ChapterRuleEntity o ? o :
                item.Clone<ChapterRuleEntity>();
            _connection.Save(data);
            item.Id = data.Id;
            return Task.CompletedTask;
        }

        public Task DeleteChapterRuleAsync(int[] items)
        {
            _connection.Build<ChapterRuleEntity>().WhereIn("Id", Array.ConvertAll(items, i => (object)i)).Delete();
            return Task.CompletedTask;
        }

        public Task ToggleChapterRuleAsync(bool enabled, params int[] items)
        {
            _connection.Build<ChapterRuleEntity>().WhereIn("Id", Array.ConvertAll(items, i => (object)i))
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
                _connection.Build<ChapterRuleEntity>().Where(nameof(item.Id), item.Id)
                 .Update(nameof(item.SortOrder), sort);
            }
            return Task.CompletedTask;
        }

        public Task<IList<T>> GetSourceRuleAsync<T>() where T : ISourceRule
        {
            return Task.FromResult(_connection.Build<T>().From<SourceRuleEntity>().ToList());
        }

        public Task<T?> GetSourceRuleAsync<T>(int id) where T : ISourceRule
        {
            return Task.FromResult(_connection.Build<T>().From<SourceRuleEntity>().Where("Id", id).First());
        }

        public Task SaveSourceRuleAsync(ISourceRule item)
        {
            var data = item is SourceRuleEntity o ? o : item.Clone<SourceRuleEntity>();
            _connection.Save(data);
            item.Id = data.Id;
            return Task.CompletedTask;
        }

        public Task DeleteSourceRuleAsync(params int[] items)
        {
            _connection.Build<SourceRuleEntity>().WhereIn("Id", Array.ConvertAll(items, i => (object)i)).Delete();
            return Task.CompletedTask;
        }

        public Task<IList<T>> GetTTSSourceAsync<T>() where T : ITextToSpeech
        {
            return Task.FromResult(_connection.Build<T>().From<TextToSpeechEntity>().ToList());
        }

        public Task<T?> GetTTSSourceAsync<T>(int id) where T : ITextToSpeech
        {
            return Task.FromResult(_connection.Build<T>().From<TextToSpeechEntity>().Where("Id", id).First());
        }

        public Task SaveTTSSourceAsync(ITextToSpeech item)
        {
            var data = item is TextToSpeechEntity o ? o : item.Clone<TextToSpeechEntity>();
            _connection.Save(data);
            item.Id = data.Id;
            return Task.CompletedTask;
        }

        public Task DeleteTTSSourceAsync(params int[] items)
        {
            _connection.Build<TextToSpeechEntity>().WhereIn("Id", Array.ConvertAll(items, i => (object)i)).Delete();
            return Task.CompletedTask;
        }

        public Task<IList<T>> GetSearchRecordAsync<T>() where T : ISearchHistory
        {
            return Task.FromResult(_connection.Build<T>().From<SearchHistoryEntity>().ToList());
        }

        public Task SaveSearchRecordAsync(string word)
        {
            var data = _connection.Build<SearchHistoryEntity>().Where("Word", word).First();
            if (data is null)
            {
                _connection.Insert(new SearchHistoryEntity()
                {
                    Word = word,
                    UseCount = 1,
                    LastUseAt = Time.TimestampFrom(DateTime.Now)
                });
            } else
            {
                data.UseCount++;
                data.LastUseAt = Time.TimestampFrom(DateTime.Now);
                _connection.Update(data);
            }
            return Task.CompletedTask;
        }

        public Task ClearSearchRecordAsync()
        {
            _connection.Build<SearchHistoryEntity>().Delete();
            return Task.CompletedTask;
        }

        public Task<IList<T>> GetReadRecordAsync<T>() where T : IReadRecord
        {
            return Task.FromResult(_connection.Build<T>().From<ReadRecordEntity>().ToList());
        }

        public Task SaveReadRecordAsync(INovel novel, long time)
        {
            var data = _connection.Build<ReadRecordEntity>().Where("Name", novel.Name).First();
            if (data is null)
            {
                _connection.Insert(new ReadRecordEntity()
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
                _connection.Update(data);
            }
            return Task.CompletedTask;
        }

        public Task ClearReadRecordAsync()
        {
            _connection.Build<ReadRecordEntity>().Delete();
            return Task.CompletedTask;
        }

        public Task<IList<T>> GetMarkAsync<T>() where T : INovelMark
        {
            return Task.FromResult(_connection.Build<T>().From<BookmarkEntity>().ToList());
        }

        public Task AddMarkAsync(INovel novel, INovelMark mark)
        {
            if (mark is not BookmarkEntity data)
            {
                data = mark.Clone<BookmarkEntity>();
            }
            data.BookName = novel.Name;
            _connection.Save(data);
            return Task.CompletedTask;
        }

        public Task DeleteMarkAsync(params int[] items)
        {
            _connection.Build<BookmarkEntity>().WhereIn("Id", Array.ConvertAll(items, i => (object)i)).Delete();
            return Task.CompletedTask;
        }

        public Task ToggleDictionaryRuleAsync(bool enabled, params int[] items)
        {
            _connection.Build<DictionaryRuleEntity>().WhereIn("Id", Array.ConvertAll(items, i => (object)i))
                .Update("IsEnabled", enabled);
            return Task.CompletedTask;
        }

        public Task ToggleReplaceRuleAsync(bool enabled, params int[] items)
        {
            _connection.Build<ReplaceRuleEntity>().WhereIn("Id", Array.ConvertAll(items, i => (object)i))
                .Update("IsEnabled", enabled);
            return Task.CompletedTask;
        }

        public Task ToggleSourceRuleAsync(bool enabled, params int[] items)
        {
            _connection.Build<SourceRuleEntity>().WhereIn("Id", Array.ConvertAll(items, i => (object)i))
                .Update("IsEnabled", enabled);
            return Task.CompletedTask;
        }

        public Task ToggleTTSSourceAsync(bool enabled, params int[] items)
        {
            _connection.Build<TextToSpeechEntity>().WhereIn("Id", Array.ConvertAll(items, i => (object)i))
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
                _connection.Build<DictionaryRuleEntity>().Where(nameof(item.Id), item.Id)
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
                _connection.Build<ReplaceRuleEntity>().Where(nameof(item.Id), item.Id)
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
                _connection.Build<SourceRuleEntity>().Where(nameof(item.Id), item.Id)
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
                _connection.Build<TextToSpeechEntity>().Where(nameof(item.Id), item.Id)
                 .Update(nameof(item.SortOrder), sort);
            }
            return Task.CompletedTask;
        }

        public async Task SetAsync<T>(string key, T value)
        {
            if (await HasAsync(key))
            {
                _connection.Build<CacheEntity>().Where("Key", key).Update("Value", value!);
            } else
            {
                _connection.Build<CacheEntity>().Insert(new CacheEntity()
                {
                    Key = key,
                    Value = value?.ToString()!
                });
            }
        }

        public Task<T?> GetAsync<T>(string key)
        {
            return Task.FromResult(_connection.Build<CacheEntity>().Where("Key", key).Value<T>("Value"));
        }

        public Task<bool> HasAsync(string key)
        {
            return Task.FromResult(_connection.Build<CacheEntity>().Where("Key", key).Any());
        }

        public Task RemoveAsync(params string[] keys)
        {
            if (keys.Length > 0)
            {
                _connection.Build<CacheEntity>().WhereIn("Key", Array.ConvertAll(keys, i => (object)i)).Delete();
            }
            return Task.CompletedTask;
        }

        public async Task SetImageAsync(string key, byte[] value)
        {
            await App.GetService<AppViewModel>().Storage.AddImageAsync(key, value);
        }

        public async Task<INovelReader> GetReaderAsync(INovelSource novel)
        {
            var type = (NovelSourceType)novel.Type;
            if (type == NovelSourceType.Network)
            {
                return new NetReader();
            }
            return Path.GetExtension(novel.FileName)[1..].ToLower() switch
            {
                "epub" => new EPubReader(),
                "umd" => new UmdReader(),
                "pdf" => new PdfReader(),
                _ => new TxtReader()
            };
        }

        public async Task<IPageTokenizer> GetTokenizerAsync(INovelDocument content)
        {
            if (content is HtmlDocument)
            {
                return new HtmlTokenizer();
            }
            return new TextTokenizer();
        }
    }
}
