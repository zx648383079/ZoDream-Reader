using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using ZoDream.Shared.Interfaces;
using ZoDream.Shared.Models;

namespace ZoDream.Shared.Tokenizers
{
    public class PageTokenizer: IPageTokenizer
    {

        public PageSetting Setting { get; set; } = new();

        public ICharIterator? Content {  get; set; }

        
        #region 缓存数据

        public Regex ChapterRegex = new(@"^(正文)?[\s]{0,6}第?[\s]*[0-9一二三四五六七八九十百千]{1,10}[章回|节|卷|集|幕|计]?[\s\S]{0,20}$");

        public IList<PagePositionItem> CachePages { get; private set; } = new List<PagePositionItem>();
        
        public IList<ChapterPositionItem> CacheChapters {  get; private set; } = new List<ChapterPositionItem>();
        
        private readonly IDictionary<int, PageItem> CachePageData = new Dictionary<int, PageItem>();
        private bool IsLoading = false;
        /// <summary>
        /// 当前页码
        /// </summary>
        public int Page { get; set; } = 0;

        public int PageCount => CachePages.Count;

        public bool CanNext => Page < CachePages.Count - 1;

        public bool CanPrevious => Page > 0;

        #endregion

        #region 对章节的操作
        public async Task<IList<ChapterPositionItem>> GetChaptersAsync()
        {
            if (CacheChapters.Count > 0)
            {
                return CacheChapters;
            }
            var items = new List<ChapterPositionItem>();
            if (Content == null)
            {
                return items;
            }
            var postion = 0L;
            while (true)
            {
                var line = await Content.ReadLineAsync(postion);
                if (line.IsEndLine)
                {
                    break;
                }
                var chapter = FormatChapter(line.ToString(), items.Count);
                if (chapter != null)
                {
                    items.Add(new ChapterPositionItem()
                    {
                        Title = chapter,
                        Position = postion,
                    });
                }
                postion = line.NextPosition;
            }
            return CacheChapters = items;
        }

        private string? FormatChapter(string line, int count = 0)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                return null;
            }
            if (count < 1 || ChapterRegex.IsMatch(line))
            {
                return line.Trim();
            }
            return null;
        }

        public int GetChapter(IEnumerable<ChapterPositionItem> items, PositionItem position)
        {
            return GetChapter(items, position.Position);
        }

        public int GetChapter(IEnumerable<ChapterPositionItem> items, long position)
        {
            ChapterPositionItem? data = null;
            var resIndex = -1;
            var i = -1;
            foreach (var item in items)
            {
                i++;
                if (item.Position > position)
                {
                    break;
                }
                if (item.Position == position)
                {
                    return i;
                }
                if (data == null || data.Position < item.Position)
                {
                    resIndex = i;
                    data = item;
                }
            }
            return resIndex;
        }

        #endregion

        /// <summary>
        /// 获取分页同时获取章节
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public async Task<Tuple<IList<PagePositionItem>, IList<ChapterPositionItem>>> GetPagesWithChaptersAsync()
        {
            var data = new Tuple<IList<PagePositionItem>, IList<ChapterPositionItem>>(new List<PagePositionItem>(), new List<ChapterPositionItem>());
            if (Content == null)
            {
                return data;
            }
            
            var maxH = Setting.PageInnerHeight;
            var maxW = Setting.PageInnerWidth;
            if (maxH <= 0 || maxW <= 0)
            {
                throw new ArgumentOutOfRangeException("view size error");
            }
            var fontH = Setting.FontSize + Setting.LineSpace;
            var x = .0;
            var y = .0;
            var position = 0L;
            var page = new PagePositionItem()
            {
                Begin = new PositionItem()
            };
            while (true)
            {
                var line = await Content.ReadLineAsync(position);
                if (line.IsEndLine)
                {
                    page.End = new PositionItem(position);
                    if (page.Length > 0)
                    {
                        data.Item1.Add(page);
                    }
                    break;
                }
                var lineStr = line.ToString();
                #region 添加章节目录
                var chapter = FormatChapter(lineStr, data.Item2.Count);
                if (chapter != null)
                {
                    data.Item2.Add(new ChapterPositionItem()
                    {
                        Title = chapter,
                        Position = position,
                    });
                }
                #endregion


                for (int i = 0; i < lineStr.Length; i++)
                {
                    var fontW = Setting.FontWidth((char?)lineStr[i]);
                    if (x +  fontW > maxW)
                    {
                        x = 0;
                        y += fontH;
                        if (y >= maxH)
                        {
                            y = 0;
                            page.End = new PositionItem(position, i - 1);
                            data.Item1.Add(page);
                            page = new PagePositionItem()
                            {
                                Begin = new PositionItem(position, i),
                            };
                        }
                    }
                    x += fontW;
                }
                y += fontH;
                x = 0;
                if (y >= maxH)
                {
                    y = 0;
                    page.End = new PositionItem(Content.Position, 0);
                    data.Item1.Add(page);
                    page = new PagePositionItem()
                    {
                        Begin = new PositionItem(Content.Position, 0),
                    };
                }
                position = line.NextPosition;
            }
            return data;
        }
        /// <summary>
        /// 只刷新分页
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public async Task<IList<PagePositionItem>> GetPagesAsync()
        {
            var data = new List<PagePositionItem>();
            if (Content == null)
            {
                return data;
            }

            var maxH = Setting.PageInnerHeight;
            var maxW = Setting.PageInnerWidth;
            if (maxH <= 0 || maxW <= 0)
            {
                throw new ArgumentOutOfRangeException("view size error");
            }
            var fontH = Setting.FontSize + Setting.LineSpace;
            var x = .0;
            var y = .0;
            var position = 0L;
            var page = new PagePositionItem()
            {
                Begin = new PositionItem()
            };
            while (true)
            {
                var line = await Content.ReadLineAsync(position);
                if (line.IsEndLine)
                {
                    page.End = new PositionItem(position);
                    if (page.Length > 0)
                    {
                        data.Add(page);
                    }
                    break;
                }
                var lineStr = line.ToString();
                for (int i = 0; i < lineStr.Length; i++)
                {
                    var fontW = Setting.FontWidth((char?)lineStr[i]);
                    if (x + fontW > maxW)
                    {
                        x = 0;
                        y += fontH;
                        if (y >= maxH)
                        {
                            y = 0;
                            page.End = new PositionItem(position, i - 1);
                            data.Add(page);
                            page = new PagePositionItem()
                            {
                                Begin = new PositionItem(position, i),
                            };
                        }
                    }
                    x += fontW;
                }
                y += fontH;
                x = 0;
                if (y >= maxH)
                {
                    y = 0;
                    page.End = new PositionItem(Content.Position, 0);
                    data.Add(page);
                    page = new PagePositionItem()
                    {
                        Begin = new PositionItem(Content.Position, 0),
                    };
                }
                position = line.NextPosition;
            }
            return data;
        }


        public async Task<PageItem> GetPageAsync(PagePositionItem page)
        {
            return await GetPageAsync(page.Begin);
        }

        public async Task<PageItem> GetPageAsync(PositionItem begin)
        {
            var data = new PageItem()
            {
                Begin = begin.Clone(),
                Data = new List<CharItem>(),
            };
            if (Content == null)
            {
                return data;
            }
            await Content.SeekAsync(data.Begin.Position >= 0 ? data.Begin.Position : 0);
            var maxH = Setting.PageInnerHeight;
            var maxW = Setting.PageInnerWidth;
            var fontH = Setting.FontSize + Setting.LineSpace;
            var x = .0;
            var y = .0;
            var offset = data.Begin.Offset;
            while (true)
            {
                var position = Content.Position;
                var line = await Content.ReadLineAsync();
                if (line == null)
                {
                    data.End = new PositionItem(position, 0);
                    break;
                }
                int i = data.Data.Count < 1 ? offset : 0;
                var isEnd = false;
                for (; i < line.Length; i++)
                {
                    var code = line[i];
                    var fontW = Setting.FontWidth((char?)code);
                    if (x + fontW > maxW)
                    {
                        x = 0;
                        y += fontH;
                        if (y >= maxH)
                        {
                            data.End = new PositionItem(position, i - 1);
                            isEnd = true;
                            break;
                        }
                    }
                    data.Data.Add(new CharItem()
                    {
                        Code = code,
                        X = x,
                        Y = y
                    });
                    x += fontW;
                }
                if (isEnd)
                {
                    break;
                }
                if (data.Data.Count < 1)
                {
                    offset -= line.Length;
                    continue;
                }
                y += fontH;
                x = 0;
                if (y >= maxH)
                {
                    data.End = new PositionItem(Content.Position, 0);
                    break;
                }
            }
            return data;
        }

        public async Task<PageItem> GetPageAsync(long position)
        {
            return await GetPageAsync(new PositionItem(position));
        }

        public async Task<IList<PageItem>> GetPagesAsync(PagePositionItem page, int count)
        {
            return await GetPagesAsync(page.Begin, count);
        }

        public async Task<IList<PageItem>> GetPagesAsync(PositionItem begin, int count)
        {
            var items = new List<PageItem>();
            var position = begin;
            for (int i = 0; i < count; i++)
            {
                var page = await GetPageAsync(position);
                ResetPage(Setting.PageX(i), Setting.PageY(i), ref page);
                items.Add(page);
                position = page.End + 1;
            }
            return items;
        }

        public async Task<IList<PageItem>> GetPagesAsync(long position, int count)
        {
            return await GetPagesAsync(new PositionItem(position), count);
        }

        /// <summary>
        /// 重设页中文字的位置
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="page"></param>
        public void ResetPage(double x, double y, ref PageItem page)
        {
            var diffX = x - page.Left;
            var diffY = y - page.Top;
            foreach (var item in page.Data)
            {
                item.X += diffX;
                item.Y += diffY;
            }
            page.Left = x;
            page.Top = y;
        }

        public async Task Refresh()
        {
            var scale = PageCount > 0 ? Page / PageCount : 0;
            CachePages = await GetPagesAsync();
            SetPageScale(scale);
            CachePageData.Clear();
        }

        /// <summary>
        /// 根据比例跳
        /// </summary>
        /// <param name="scale"></param>
        public void SetPageScale(double scale, int max = 1)
        {
            if (scale < 0 || scale > max)
            {
                return;
            }
            Page = Convert.ToInt32(Math.Floor(CachePages.Count * scale / max));
        }

        /// <summary>
        /// 跳章节
        /// </summary>
        /// <param name="chapter"></param>
        public void SetPage(ChapterPositionItem chapter)
        {
            SetPage(chapter.Position);
        }

        public void SetPage(double position)
        {
            SetPage(position, 0);
        }

        public void SetPage(double position, int offset)
        {
            for (int i = CachePages.Count  - 1; i >= 0; i--)
            {
                var item = CachePages[i];
                if (item.Begin.Position <= position && item.Begin.Offset <= offset)
                {
                    Page = i;
                    return;
                }
            }
            return;
        }

        public void SetPage(PositionItem position)
        {
            SetPage(position.Position, position.Offset);
        }

        public void SetPage(PagePositionItem page)
        {
            SetPage(page.Begin);
        }

        public bool Enable(int page)
        {
            return page >= 0 && page < CachePages.Count;
        }

        /// <summary>
        /// 获取当前页
        /// </summary>
        /// <returns></returns>
        public async Task<IList<PageItem>> GetAsync()
        {
            if (Page < 0)
            {
                Page = 0;
            } else if (Page >= CachePages.Count)
            {
                Page = CachePages.Count  - 1;
            }
            return await GetAsync(Page);
        }

        public async Task<IList<PageItem>> GetAsync(int page)
        {
            while (IsLoading)
            {
                Thread.Sleep(50);
            }
            var items = new List<PageItem>();
            for (int i = 0; i < Setting.ColumnCount; i++)
            {
                var item = await GetPageAsync(page + i);
                if (item == null)
                {
                    break;
                }
                ResetPage(Setting.PageX(i), Setting.PageY(i), ref item);
                items.Add(item);
            }
            UpdateCachePages(page);
            return items;
        }

        private async void UpdateCachePages(int page)
        {
            IsLoading = true;
            var pageCount = 3;
            var min = Math.Max(0, page - pageCount * Setting.ColumnCount);
            var max = Math.Min(PageCount - 1, page + pageCount * Setting.ColumnCount);

            var keys = new int[CachePageData.Keys.Count];
            CachePageData.Keys.CopyTo(keys, 0);
            foreach (var item in keys)
            {
                if (item < min || item > max)
                {
                    CachePageData.Remove(item);
                }
            }
            for (int i = min; i <= max; i++)
            {
                if (HasCachePage(i))
                {
                    continue;
                }
                var item = await GetPageAsync(CachePages[i]);
                if (item == null)
                {
                    break;
                }
                SetCachePage(i, item);
            }
            IsLoading = false;
        }

        public async Task<PageItem?> GetPageAsync(int page)
        {
            if (!Enable(page))
            {
                return null;
            }
            if (HasCachePage(page))
            {
                return GetCachePage(page);
            }
            var data = await GetPageAsync(CachePages[page]);
            SetCachePage(page, data);
            return data;
        }

        private void SetCachePage(int page, PageItem data)
        {
            CachePageData[page] = data;
        }

        private PageItem GetCachePage(int page)
        {
            return CachePageData[page];
        }

        private bool HasCachePage(int page)
        {
            return CachePageData.ContainsKey(page);
        }

        /// <summary>
        /// 获取下一页
        /// </summary>
        /// <returns></returns>
        public async Task<IList<PageItem>> GetNextAsync()
        {
            if (!CanNext)
            {
                return new List<PageItem>();
            }
            Page += Setting.ColumnCount;
            return await GetAsync();
        }

        /// <summary>
        /// 获取上一页
        /// </summary>
        /// <returns></returns>
        public async Task<IList<PageItem>> GetPreviousAsync()
        {
            if (!CanPrevious)
            {
                return new List<PageItem>();
            }
            Page -= Setting.ColumnCount;
            return await GetAsync();
        }

        public void Dispose()
        {
            Content?.Dispose();
            CachePages.Clear();
            CacheChapters.Clear();
            CachePageData.Clear();
        }
    }
}
