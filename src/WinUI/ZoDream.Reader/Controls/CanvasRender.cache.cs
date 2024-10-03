using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Text;
using Microsoft.Graphics.Canvas.UI.Xaml;

namespace ZoDream.Reader.Controls
{
    public sealed partial class TextContainer
    {

        public CanvasControl Canvas => PaintCanvas;

        public ICanvasImage? BackgroundImage
        {
            get {
                return null;
            }
        }

    }
}
