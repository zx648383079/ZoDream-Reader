using System.Numerics;
using ZoDream.Shared.Interfaces;

namespace ZoDream.Shared.Animations
{
    public abstract class BaseCanvasAnimate : ICanvasAnimate
    {
        protected ICanvasRender? Canvas;
        protected Vector2 _size;

        protected Vector2 _lastPoint;
        protected Vector2 _beginPoint;
        protected bool? DirectNext;
        protected bool IsMoving;

        public virtual void Ready(ICanvasRender canvas)
        {
            _size = Vector2.Zero;
            Canvas = canvas;
            Resize(canvas.Size);
        }

        public virtual void Resize(Vector2 size)
        {
            if (_size == size)
            {
                return;
            }
            _size = size;
            OnResize();
        }

        protected virtual void OnResize()
        {

        }

        public virtual void OnDraw(ICanvasRender canvas)
        {
            if (Canvas is null)
            {
                Ready(canvas);
            }
        }
        public virtual void OnTouchFinish(Vector2 point)
        {
            _lastPoint = point;
            IsMoving = false;
        }

        public virtual void OnTouchMove(Vector2 point)
        {
            _lastPoint = point;
            IsMoving = true;
        }

        public virtual void OnTouchStart(Vector2 point)
        {
            _lastPoint = _beginPoint = point;
            DirectNext = null;
            IsMoving = false;
        }

        public abstract void TurnNext();
        public abstract void TurnPrevious();

        public abstract void Invalidate();
        public abstract void Dispose();
    }
}
