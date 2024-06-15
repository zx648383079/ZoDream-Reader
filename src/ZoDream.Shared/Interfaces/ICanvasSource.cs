using System.Threading.Tasks;
using ZoDream.Shared.Interfaces.Tokenizers;

namespace ZoDream.Shared.Interfaces
{
    public interface ICanvasSource
    {
        public void Resize(double width);
        public void Resize(double width, double height);

        public INovelPage? Current { get; }

        public Task<bool> ReadNextAsync();
    }
}
