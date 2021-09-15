using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ZoDream.Shared.Models;

namespace ZoDream.Shared.Interfaces
{
    public interface IPageTokenizer: IDisposable
    {
        public int Page { get; set; }

        public bool CanNext { get; }

        public bool CanPrevious {  get; }

        public Task<IList<ChapterPositionItem>> GetChaptersAsync();

        /// <summary>
        /// 更新一些东西
        /// </summary>
        public Task Refresh();

        public void SetPageScale(double scale, int max = 1);
        public void SetPage(ChapterPositionItem chapter);
        public void SetPage(double position);
        public void SetPage(double position, int offset);

        public void SetPage(PositionItem position);
        public void SetPage(PagePositionItem page);

        public Task<IList<PageItem>> GetPreviousAsync();
        public Task<IList<PageItem>> GetNextAsync();

        public Task<IList<PageItem>> GetAsync();
    }
}
