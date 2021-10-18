using System;
using System.Collections.Generic;
using System.Text;

namespace ZoDream.Shared.Interfaces
{
    public interface ICanvasAnimate: IDisposable
    {
        public bool HasAnimate { get; }
        public bool IsFinished { get; }

        public void Animate(bool toNext, Action finished);
    }
}
