using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ZoDream.Shared.Interfaces.Entities;

namespace ZoDream.Shared.Interfaces
{
    public interface INovelImporter
    {
        public Task<List<IAppTheme>> LoadAppThemeAsync(string fileName);
        public Task<List<ISubscribeSource>> LoadRssAsync(string fileName);
        public Task<List<IReadTheme>> LoadReadThemeAsync(string fileName);
        public Task<List<ISourceRule>> LoadSourceAsync(string fileName);

        public Task<List<ITextToSpeech>> LoadTTSAsync(string fileName);

        public Task<List<IDictionaryRule>> LoadDictionaryRuleAsync(string fileName);

        public Task<List<IChapterRule>> LoadChapterRuleAsync(string fileName);
    }
}
