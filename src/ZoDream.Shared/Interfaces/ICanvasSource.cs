using System.Threading.Tasks;
using ZoDream.Shared.Interfaces.Tokenizers;

namespace ZoDream.Shared.Interfaces
{
    public interface ICanvasSource
    {
        //public Task<IList<PageItem>> GetAsync(int page);

        //public bool Enable(int page);

        public INovelPage? Current { get; }

        public Task<bool> ReadNextAsync();
    }
}
