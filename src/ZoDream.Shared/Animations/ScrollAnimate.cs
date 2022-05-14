using System;
using System.Collections.Generic;
using System.Text;
using ZoDream.Shared.Interfaces;

namespace ZoDream.Shared.Animations
{
    public class ScrollAnimate : ICanvasAnimate
    {
        public bool HasAnimate => true;

        public double Move(ICanvasLayer layer, double x, double y)
        {
            throw new NotImplementedException();
        }

        public void MoveStart(ICanvasLayer layer, double x, double y)
        {
            throw new NotImplementedException();
        }

        public bool Animate(IList<ICanvasLayer> layers, double offset, bool isTouch)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
