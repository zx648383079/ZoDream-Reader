using Microsoft.Graphics.Canvas;
using System;
using System.Collections.Generic;
using System.Text;
using Windows.Foundation;
using ZoDream.Shared.Interfaces;

namespace ZoDream.Reader.Drawing.Animations
{
    public class ScrollAnimate : ILayerAnimate
    {
        public bool HasAnimate => throw new NotImplementedException();

        public bool IsFinished => throw new NotImplementedException();

        public void Animate(bool toNext, Action finished)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void Draw(CanvasDrawingSession ds)
        {
            throw new NotImplementedException();
        }

        public void TouchMove(Point p)
        {
            throw new NotImplementedException();
        }

        public void TouchMoveBegin(Point p)
        {
            throw new NotImplementedException();
        }

        public void TouchMoveEnd(Point p)
        {
            throw new NotImplementedException();
        }
    }
}
