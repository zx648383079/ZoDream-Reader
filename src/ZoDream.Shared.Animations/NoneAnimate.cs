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
            _layer = Canvas?.CreateLayer(Width, Height);
            _layer?.DrawText(page);
        }

        public override void OnDraw(ICanvasRender canvas)
        {
            base.OnDraw(canvas);
            canvas.DrawLayer(_layer!);
        }

        public override void OnTouchFinish(double x, double y)
        {

        }

        public override void OnTouchMove(double x, double y)
        {

        }

        public override void OnTouchStart(double x, double y)
        {

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
        }

        public override void Dispose()
        {
        }
    }
}
