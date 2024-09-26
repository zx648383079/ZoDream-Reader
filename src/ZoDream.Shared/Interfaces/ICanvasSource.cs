using System.Threading.Tasks;
using ZoDream.Shared.Interfaces.Tokenizers;

namespace ZoDream.Shared.Interfaces
{
    public interface ICanvasSource
    {
        public Task InvalidateAsync();

        public INovelPage? Current { get; }

        public Task<bool> ReadNextAsync();
    }
}
