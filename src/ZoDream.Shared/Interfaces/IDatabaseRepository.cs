﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZoDream.Shared.Interfaces.Entities;

namespace ZoDream.Shared.Interfaces
{
    public interface IDatabaseRepository: IDisposable
    {

        public Task<List<T>> GetBookAsync<T>() where T: INovel;
        public Task<T?> GetBookAsync<T>(string id) where T: INovel;

        public Task SaveBookAsync(INovel item);

        public Task DeleteBookAsync(params string[] items);

        public Task<List<T>> GetChapterAsync<T>(string bookId) where T : INovelChapter;
        public Task SaveChapterAsync<T>(string bookId, IEnumerable<T> items) where T : INovelChapter;

        public Task<List<T>> GetThemeAsync<T>() where T : IAppTheme;
        public Task<T?> GetThemeAsync<T>(int id) where T : IAppTheme;

        public Task SaveThemeAsync(IAppTheme item);

        public Task DeleteThemeAsync(params int[] items);

        public Task<List<T>> GetReadThemeAsync<T>() where T : IReadTheme;
        public Task<T?> GetReadThemeAsync<T>(int id) where T : IReadTheme;

        public Task SaveReadThemeAsync(IReadTheme item);

        public Task DeleteReadThemeAsync(params int[] items);

        public Task<List<T>> GetDictionaryRuleAsync<T>() where T : IDictionaryRule;
        public Task SaveDictionaryRuleAsync(IDictionaryRule item);

        public Task DeleteDictionaryRuleAsync(params int[] items);

        public Task ToggleDictionaryRuleAsync(bool enabled, params int[] items);

        public Task SortDictionaryRuleAsync<T>(IEnumerable<T> items) where T : IDictionaryRule;

        public Task<List<T>> GetReplaceRuleAsync<T>() where T : IReplaceRule;
        public Task SaveReplaceRuleAsync(IReplaceRule item);

        public Task DeleteReplaceRuleAsync(params int[] items);

        public Task ToggleReplaceRuleAsync(bool enabled, params int[] items);

        public Task SortReplaceRuleAsync<T>(IEnumerable<T> items) where T : IReplaceRule;

        public Task<List<T>> GetChapterRuleAsync<T>() where T : IChapterRule;
        public Task<string[]> GetEnabledChapterRuleAsync();
        public Task SaveChapterRuleAsync(IChapterRule item);

        public Task DeleteChapterRuleAsync(params int[] items);

        public Task ToggleChapterRuleAsync(bool enabled, params int[] items);
        public Task SortChapterRuleAsync<T>(IEnumerable<T> items) where T: IChapterRule;

        public Task<List<T>> GetSourceRuleAsync<T>() where T : ISourceRule;
        public Task<T?> GetSourceRuleAsync<T>(int id) where T : ISourceRule;
        public Task SaveSourceRuleAsync(ISourceRule item);

        public Task DeleteSourceRuleAsync(params int[] items);

        public Task ToggleSourceRuleAsync(bool enabled, params int[] items);

        public Task SortSourceRuleAsync<T>(IEnumerable<T> items) where T : ISourceRule;

        public Task<List<T>> GetTTSSourceAsync<T>() where T : ITextToSpeech;
        public Task<T?> GetTTSSourceAsync<T>(int id) where T : ITextToSpeech;
        public Task SaveTTSSourceAsync(ITextToSpeech item);
        public Task DeleteTTSSourceAsync(params int[] items);
        public Task ToggleTTSSourceAsync(bool enabled, params int[] items);

        public Task SortTTSSourceAsync<T>(IEnumerable<T> items) where T : ITextToSpeech;

        public Task<List<T>> GetSearchRecordAsync<T>() where T : ISearchHistory;
        public Task SaveSearchRecordAsync(string word);
        public Task ClearSearchRecordAsync();

        public Task<List<T>> GetReadRecordAsync<T>() where T : IReadRecord;
        public Task SaveReadRecordAsync(INovel novel, long time);
        public Task ClearReadRecordAsync();

        public Task<List<T>> GetMarkAsync<T>() where T : INovelMark;
        public Task AddMarkAsync(INovel novel, INovelMark mark);
        public Task DeleteMarkAsync(params int[] items);

        public Task<IAppOption> LoadSettingAsync();

        public Task SaveSettingAsync(IAppOption data);
    }
}
