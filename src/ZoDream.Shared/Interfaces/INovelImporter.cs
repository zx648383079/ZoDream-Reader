using System.Collections.Generic;
using System.Threading.Tasks;
using ZoDream.Shared.Interfaces.Entities;

namespace ZoDream.Shared.Interfaces
{
    public interface INovelImporter
    {
        public Task<List<T>> LoadAppThemeAsync<T>(string fileName) where T: IAppTheme, new();
        public Task<List<T>> LoadRssAsync<T>(string fileName) where T : ISubscribeSource, new();
        public Task<List<T>> LoadReadThemeAsync<T>(string fileName) where T : IReadTheme, new();
        public Task<List<T>> LoadSourceAsync<T>(string fileName) where T : ISourceRule, new();

        public Task<List<T>> LoadTTSAsync<T>(string fileName) where T : ITextToSpeech, new();

        public Task<List<T>> LoadDictionaryRuleAsync<T>(string fileName) where T : IDictionaryRule, new();
        public Task<List<T>> LoadReplaceRuleAsync<T>(string fileName) where T : IReplaceRule, new();

        public Task<List<T>> LoadChapterRuleAsync<T>(string fileName) where T : IChapterRule, new();
    }
}
