using System;
using System.Collections.Generic;
using System.Text;

namespace ZoDream.Shared.Interfaces
{
    public interface ICanvasAnimate: IDisposable
    {
        public void Ready(ICanvasRender canvas);
        public void Resize(double width, double height);

        public void OnTouchStart(double x, double y);
        public void OnTouchMove(double x, double y);
        public void OnTouchFinish(double x, double y);

        public void TurnPrevious();
        public void TurnNext();

        /// <summary>
        /// 直接重新跟新内容
        /// </summary>
        public void Invalidate();

        public void OnDraw(ICanvasRender canvas);
    }
}
