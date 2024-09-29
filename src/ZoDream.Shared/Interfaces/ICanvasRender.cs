using ZoDream.Shared.Events;

namespace ZoDream.Shared.Interfaces
{
    public interface ICanvasRender
    {
        public ICanvasSource? Source {  get; set; }

        public double ActualWidth { get; }
        public double ActualHeight { get; }

        /// <summary>
        /// 翻页完成事件
        /// </summary>
        public event PageChangedEventHandler? PageChanged;

        /// <summary>
        /// 画布准备就绪事件
        /// </summary>
        public event CanvasReadyEventHandler? OnReady;

        public void SetAnimate(ICanvasAnimate animate);

        public ICanvasLayer CreateLayer(double width, double height);

        public void Invalidate();

        public void DrawLayer(ICanvasLayer layer);

        //public void SwapTo(IList<PageItem> pages, int page);
        ///// <summary>
        ///// 使用过渡动画切换到新的页面，下一页
        ///// </summary>
        ///// <param name="page"></param>
        ///// <returns></returns>
        //public Task SwapToAsync(int page);

        //public void SwapFrom(IList<PageItem> pages, int page);
        ///// <summary>
        ///// 使用过渡动画切换回新的页面，上一页
        ///// </summary>
        ///// <param name="page"></param>
        //public Task SwapFromAsync(int page);

        //public Task SwapNextAsync();

        //public Task SwapPreviousAsync();

        ///// <summary>
        ///// 清空页面内容
        ///// </summary>
        //public void Flush();
    }
}
