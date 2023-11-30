using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZoDream.Shared.Interfaces.Entities;

namespace ZoDream.Shared.Interfaces
{
    public interface IDatabaseRepository: IDisposable
    {

        public Task<List<T>> GetBookAsync<T>() where T: INovel;
        public Task<T> GetBookAsync<T>(object id) where T: INovel;

        public Task SaveBookAsync(INovel item);

        public Task DeleteBookAsync(object id);

        public Task<List<T>> GetThemeAsync<T>() where T : IAppTheme;
        public Task<T> GetThemeAsync<T>(object id) where T : IAppTheme;

        public Task SaveThemeAsync(IAppTheme item);

        public Task DeleteThemeAsync(object id);

        public Task<List<T>> GetReadThemeAsync<T>() where T : IReadTheme;
        public Task<T> GetReadThemeAsync<T>(object id) where T : IReadTheme;

        public Task SaveReadThemeAsync(IReadTheme item);

        public Task DeleteReadThemeAsync(object id);

        public Task<List<T>> GetDictionaryRuleAsync<T>() where T : IDictionaryRule;
        public Task SaveDictionaryRuleAsync(IDictionaryRule item);

        public Task DeleteDictionaryRuleAsync(object id);

        public Task<List<T>> GetReplaceRuleAsync<T>() where T : IReplaceRule;
        public Task SaveReplaceRuleAsync(IReplaceRule item);

        public Task DeleteReplaceRuleAsync(object id);

        public Task<List<T>> GetChapterRuleAsync<T>() where T : IChapterRule;
        public Task SaveChapterRuleAsync(IChapterRule item);

        public Task DeleteChapterRuleAsync(object id);

        public Task<List<T>> GetSourceRuleAsync<T>() where T : ISourceRule;
        public Task<T> GetSourceRuleAsync<T>(object id) where T : ISourceRule;
        public Task SaveSourceRuleAsync(ISourceRule item);

        public Task DeleteSourceRuleAsync(object id);

        public Task<List<T>> GetTTSSourceAsync<T>() where T : ITextToSpeech;
        public Task<T> GetTTSSourceAsync<T>(object id) where T : ITextToSpeech;
        public Task SaveTTSSourceAsync(ITextToSpeech item);

        public Task DeleteTTSSourceAsync(object id);

        public Task<List<T>> GetSearchRecordAsync<T>() where T : ISearchHistory;
        public Task SaveSearchRecordAsync(string word);
        public Task ClearSearchRecordAsync();

        public Task<List<T>> GetReadRecordAsync<T>() where T : IReadRecord;
        public Task SaveReadRecordAsync(INovel novel, long time);
        public Task ClearReadRecordAsync();

        public Task<List<T>> GetMarkAsync<T>() where T : INovelMark;
        public Task AddMarkAsync(INovel novel, INovelMark mark);
        public Task DeleteMarkAsync(object id);

        public Task<IAppOption> LoadSettingAsync();

        public Task SaveSettingAsync(IAppOption data);
    }
}
