using Microsoft.Graphics.Canvas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using ZoDream.Shared.Interfaces;

namespace ZoDream.Reader.Drawing.Animations
{
    public interface ILayerAnimate: ICanvasAnimate
    {
        void TouchMoveBegin(Point p);

        void TouchMove(Point p);

        void TouchMoveEnd(Point p);

        void Draw(CanvasDrawingSession ds);
    }
}
