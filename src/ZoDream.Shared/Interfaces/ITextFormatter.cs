using System.Numerics;
using ZoDream.Shared.Interfaces.Tokenizers;

namespace ZoDream.Shared.Interfaces
{
    public interface ITextFormatter
    {
        public Vector2 Compute(ICanvasTheme theme, char code);
        public INovelPageLine[] Compute(ICanvasTheme theme, string text);
    }
}
