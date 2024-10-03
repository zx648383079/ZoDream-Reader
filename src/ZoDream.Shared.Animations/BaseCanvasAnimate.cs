using ZoDream.Shared.Interfaces;

namespace ZoDream.Shared.Animations
{
    public abstract class BaseCanvasAnimate : ICanvasAnimate
    {
        protected ICanvasRender? Canvas;
        protected double Width;
        protected double Height;

        protected double LastX;
        protected double LastY;
        protected double BeginY;
        protected double BeginX;
        protected bool? DirectNext;
        protected bool IsMoving;

        public virtual void Ready(ICanvasRender canvas)
        {
            Width = 0;
            Height = 0;
            Canvas = canvas;
            Resize(canvas.ActualWidth, canvas.ActualHeight);
        }

        public virtual void Resize(double width, double height)
        {
            if (Width == width &&  Height == height)
            {
                return;
            }
            Width = width;
            Height = height;
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
        public virtual void OnTouchFinish(double x, double y)
        {
            LastX = x;
            LastY = y;
            IsMoving = false;
        }

        public virtual void OnTouchMove(double x, double y)
        {
            LastX = x;
            LastY = y;
            IsMoving = true;
        }

        public virtual void OnTouchStart(double x, double y)
        {
            LastX = BeginX = x;
            LastY = BeginY = y;
            DirectNext = null;
            IsMoving = false;
        }

        public abstract void TurnNext();
        public abstract void TurnPrevious();

        public abstract void Invalidate();
        public abstract void Dispose();
    }
}
