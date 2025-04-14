using System;
using System.Numerics;

namespace ZoDream.Shared.Interfaces
{
    public interface ICanvasAnimate: IDisposable
    {
        public void Ready(ICanvasRender canvas);
        public void Resize(Vector2 size);

        public void OnTouchStart(Vector2 point);
        public void OnTouchMove(Vector2 point);
        public void OnTouchFinish(Vector2 point);

        public void TurnPrevious();
        public void TurnNext();

        /// <summary>
        /// 直接重新跟新内容
        /// </summary>
        public void Invalidate();

        public void OnDraw(ICanvasRender canvas);
    }
}
