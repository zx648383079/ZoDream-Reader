using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ZoDream.Shared.Events;
using ZoDream.Shared.Models;

namespace ZoDream.Shared.Interfaces
{
    public interface ICanvasRender
    {
        public ICanvasSource? Source {  get; set; }

        /// <summary>
        /// 翻页完成事件
        /// </summary>
        public event PageChangedEventHandler? PageChanged;

        /// <summary>
        /// 画布准备就绪事件
        /// </summary>
        public event CanvasReadyEventHandler? OnReady;

        /// <summary>
        /// 画多页，分栏
        /// </summary>
        /// <param name="pages"></param>
        public void Draw(IList<PageItem> pages);

        /// <summary>
        /// 使用过渡动画切换到新的页面，下一页
        /// </summary>
        /// <param name="pages"></param>
        public void SwapTo(IList<PageItem> pages);
        public void SwapTo(IList<PageItem> pages, int page);

        public Task SwapTo(int page);

        /// <summary>
        /// 使用过渡动画切换回新的页面，上一页
        /// </summary>
        /// <param name="pages"></param>
        public void SwapFrom(IList<PageItem> pages);

        public void SwapFrom(IList<PageItem> pages, int page);
        public void SwapFrom(int page);

        public void SwapNext();

        public void SwapPrevious();

        /// <summary>
        /// 清空页面内容
        /// </summary>
        public void Flush();
    }
}
