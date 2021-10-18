using System;
using System.Collections.Generic;
using System.Text;
using ZoDream.Shared.Interfaces;

namespace ZoDream.Shared.Animations
{
    public class NoneAnimate : ICanvasAnimate
    {
        public bool HasAnimate => false;

        public bool IsFinished => true;

        public void Animate(bool toNext, Action finished)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
