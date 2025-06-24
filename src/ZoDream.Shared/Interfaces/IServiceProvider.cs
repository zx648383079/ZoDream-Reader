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

        public ICanvasAnimate Animator { get; }

        public Task ReadyAsync(ICanvasRender render);

        public Task<INovelSerializer> GetReaderAsync();

        public Task<IPageTokenizer> GetTokenizerAsync(ISectionSource document);
        public Task<IList<INovelPage>> PageParseAsync(ISectionSource document);
        public Task<INovelChapter[]> LoadChaptersAsync();

        public Task<ISectionSource> GetChapterAsync(int chapterId);

        public Task<IReadTheme> GetReadThemeAsync();
    }
}
