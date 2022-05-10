using System;
using System.Collections.Generic;
using System.Text;
using ZoDream.Shared.Interfaces;

namespace ZoDream.Shared.Animations
{
    public class CoverAnimate : ICanvasAnimate
    {
        public bool HasAnimate => throw new NotImplementedException();

        public bool IsFinished => throw new NotImplementedException();

        public void Animate(bool toNext, Action finished)
        {
            
        }

        public void Dispose()
        {
            
        }
    }
}
