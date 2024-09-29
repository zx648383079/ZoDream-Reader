using System.Collections.Generic;
using System.Threading.Tasks;
using ZoDream.Shared.Interfaces.Entities;
using ZoDream.Shared.Interfaces.Tokenizers;

namespace ZoDream.Shared.Interfaces
{
    public interface INovelEnvironment: ICanvasControl
    {
        public string NovelId { get; }

        public int ChapterIndex { get; set; }

        public double ChapterProgresss { get; set; }

        public Task<INovelReader> GetReaderAsync();

        public Task<IPageTokenizer> GetTokenizerAsync(INovelDocument document);
        public Task<IList<INovelPage>> PageParseAsync(INovelDocument document);
        public Task<INovelChapter[]> LoadChaptersAsync();

        public Task<INovelDocument> GetChapterAsync(int chapterId);

        public Task<IReadTheme> GetReadThemeAsync();
    }
}
