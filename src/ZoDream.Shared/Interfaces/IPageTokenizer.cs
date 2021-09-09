using System;
using System.Collections.Generic;
using System.Text;
using ZoDream.Shared.Models;

namespace ZoDream.Shared.Interfaces
{
    public interface IPageTokenizer: IDisposable
    {
        public int Page { get; set; }

        public bool CanNext { get; }

        public bool CanPrevious {  get; }

        public IList<ChapterPositionItem> GetChapters();

        /// <summary>
        /// 更新一些东西
        /// </summary>
        public void Refresh();

        public void SetPageScale(double scale, int max = 1);
        public void SetPage(ChapterPositionItem chapter);
        public void SetPage(double position);
        public void SetPage(double position, int offset);

        public void SetPage(PositionItem position);
        public void SetPage(PagePositionItem page);

        public IList<PageItem> GetPrevious();
        public IList<PageItem> GetNext();

        public IList<PageItem> Get();
    }
}
