using System;
using System.Collections.Generic;
using System.Text;
using ZoDream.Shared.Models;

namespace ZoDream.Shared.Interfaces
{
    public interface ICanvasRender
    {
        /// <summary>
        /// 画字
        /// </summary>
        /// <param name="item"></param>
        void Draw(CharItem item);
        /// <summary>
        /// 画一页
        /// </summary>
        /// <param name="page"></param>
        void Draw(PageItem page);
        /// <summary>
        /// 画多页，分栏
        /// </summary>
        /// <param name="pages"></param>
        void Draw(IEnumerable<PageItem> pages);

        /// <summary>
        /// 使用过渡动画切换
        /// </summary>
        /// <param name="pages"></param>
        void Swap(IEnumerable<PageItem> pages);

        /// <summary>
        /// 清空页面内容
        /// </summary>
        void Flush();
    }
}
