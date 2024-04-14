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
            Canvas = canvas;
            Resize(canvas.ActualWidth, canvas.ActualHeight);
        }
        public virtual void Resize(double width, double height)
        {
            Width = width;
            Height = height;
        }

        public abstract void OnDraw(ICanvasRender canvas);
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

        public abstract void Dispose();
    }
}
