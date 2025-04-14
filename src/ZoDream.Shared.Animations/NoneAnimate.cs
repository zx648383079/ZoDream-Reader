using System.Numerics;
using ZoDream.Shared.Interfaces;

namespace ZoDream.Shared.Animations
{
    public class NoneAnimate : BaseCanvasAnimate
    {

        private ICanvasLayer? _layer;

        protected override void OnResize()
        {
            var page = Canvas?.Source?.Current;
            if (page is null)
            {
                return;
            }
            _layer = Canvas?.CreateLayer(_size);
            _layer?.DrawText(page);
        }

        public override void OnDraw(ICanvasRender canvas)
        {
            base.OnDraw(canvas);
            canvas.DrawLayer(_layer!);
        }

        public override void OnTouchFinish(Vector2 point)
        {
            base.OnTouchFinish(point);
            if (!IsMoving)
            {
                if (point.X < _size.X / 3)
                {
                    TurnPrevious();
                }
                else if (point.X < _size.X * .7)
                {
                    TurnNext();
                }
                return;
            }
        }

        public override void OnTouchMove(Vector2 point)
        {
            DirectNext = point.X > _lastPoint.X;
            base.OnTouchMove(point);
        }

        public override void OnTouchStart(Vector2 point)
        {
            base.OnTouchStart(point);
        }

        public override void TurnNext()
        {
            var res = Canvas?.Source?.ReadNextAsync().GetAwaiter().GetResult();
            if (res != true)
            {
                return;
            }
            Invalidate();
        }

        public override void TurnPrevious()
        {
            var res = Canvas?.Source?.ReadPreviousAsync().GetAwaiter().GetResult();
            if (res != true)
            {
                return;
            }
            Invalidate();
        }

        public override void Invalidate()
        {
            if (_layer is null)
            {
                return;
            }
            _layer!.DrawText(Canvas!.Source!.Current!);
            Canvas.Invalidate();
        }

        public override void Dispose()
        {
        }
    }
}
