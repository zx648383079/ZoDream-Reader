using Microsoft.Graphics.Canvas;
using System;
using System.Collections.Generic;
using System.Text;
using Windows.Foundation;
using ZoDream.Reader.Controls;
using ZoDream.Shared.Interfaces;

namespace ZoDream.Reader.Drawing.Animations
{
    public class NoneAnimate : ILayerAnimate
    {
        public NoneAnimate(PageCanvas canvas)
        {
            Canvas = canvas;
        }

        private PageCanvas Canvas;
        private Point lastMousePoint = new Point(0, 0);

        public bool HasAnimate => false;

        public bool IsFinished => true;


        public void TouchMoveBegin(Point p)
        {
            lastMousePoint = p;
        }

        public void TouchMove(Point p)
        {
            
        }

        public void TouchMoveEnd(Point p)
        {
            var diffX = p.X - lastMousePoint.X;
            var diffY = p.Y - lastMousePoint.Y;
            var diff = Math.Pow(diffX, 2) + Math.Pow(diffY, 2);
            if (diff < 25)
            {
                // TODO 点击
                if (p.X < Canvas.ActualWidth / 3)
                {
                    Canvas.SwapPrevious();
                }
                else if (p.X > Canvas.ActualWidth * .7)
                {
                    Canvas.SwapNext();
                }
                return;
            }

            if (Math.Abs(diffX) > Math.Abs(diffY))
            {
                if (diffX > 0)
                {
                    Canvas.SwapPrevious();
                }
                else if (diffX < 0)
                {
                    Canvas.SwapNext();
                }
            }
        }

        public void Draw(CanvasDrawingSession ds)
        {
            Canvas.layerItems[1]?.Draw(ds);
        }

        public void Animate(bool toNext, Action finished)
        {
            Canvas.Invalidate();
            finished();
        }

        public void Dispose()
        {
            
        }
    }
}
