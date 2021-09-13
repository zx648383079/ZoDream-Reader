using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using ZoDream.Shared.Interfaces;
using ZoDream.Shared.Models;

namespace ZoDream.Shared.Tokenizers
{
    public class PageTokenizer: IPageTokenizer
    {
        public double Width { get; set; }

        public double Height { get; set; }

        /// <summary>
        /// 单个字的大小
        /// </summary>
        public int FontSize { get; set; } = 20;

        public int LineSpace { get; set; } = 2;

        public int LetterSpace { get; set; } = 2;

        public int Left { get; set; } = 10;

        public int Right {  get; set; } = 10;

        public int Top {  get; set; } = 10;

        public int Bottom {  get; set; } = 10;

        public int Gap {  get; set; } = 20;

        public int ColumnCount { get; set; } = 1;

        public ICharIterator? Content {  get; set; }

        public double PageInnerWidth
        {
            get {
                return (Width - Left - Right - (ColumnCount - 1) * Gap) / ColumnCount;
            }
        }

        public double PageInnerHeight
        {
            get
            {
                return Height - Top - Bottom;
            }
        }

        public double PageX(int column)
        {
            return Left + column * (PageInnerWidth + Gap);
        }

        public double PageY(int column)
        {
            return Top;
        }
        public double LineY(int index)
        {
            return Top + index * (FontSize + LineSpace);
        }

        public double FontX(int column, int index)
        {
            return FontX(PageX(column), index);
        }

        public double FontX(double pageX, int index)
        {
            return pageX + index * (FontSize + LetterSpace);
        }
        public double FontWidth(double count)
        {
            return count * (FontSize + LetterSpace);
        }
        public double FontWidth(char? code)
        {
            if (code == null)
            {
                return 0;
            }
            if (code == '\t')
            {
                return FontWidth(2);
            }
            if ((code >= 48 && code <= 57) || (code >= 65 && code <= 90))
            {
                return FontWidth(.5);
            }
            return FontWidth(1);
        }

        #region 缓存数据

        public IList<PagePositionItem> CachePages { get; private set; } = new List<PagePositionItem>();
        /// <summary>
        /// 当前页码
        /// </summary>
        public int Page { get; set; } = 0;

        public bool CanNext => Page < CachePages.Count - 1;

        public bool CanPrevious => Page > 0;

        #endregion

        #region 对章节的操作
        public IList<ChapterPositionItem> GetChapters()
        {
            var items = new List<ChapterPositionItem>();
            if (Content == null)
            {
                return items;
            }
            Content.Position = 0;
            var regex = new Regex(@"^(正文)?[\s]{0,6}第?[\s]*[0-9一二三四五六七八九十百千]{1,10}[章回|节|卷|集|幕|计]?[\s\S]{0,20}$");
            while (true)
            {
                var postion = Content.Position;
                var line = Content.ReadLine();
                if (line == null)
                {
                    break;
                }
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }
                if (items.Count < 1 || regex.IsMatch(line))
                {
                    items.Add(new ChapterPositionItem()
                    {
                        Title = line.Trim(),
                        Position = postion,
                    });
                }
            }
            return items;
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



        public IList<PagePositionItem> GetPages()
        {
            var items = new List<PagePositionItem>();
            if (Content == null)
            {
                return items;
            }
            Content.Position = 0;
            var maxH = PageInnerHeight;
            var maxW = PageInnerWidth;
            var fontH = FontSize + LineSpace;
            var x = .0;
            var y = .0;
            var page = new PagePositionItem()
            {
                Begin = new PositionItem()
            };
            while (true)
            {
                var position = Content.Position;
                var line = Content.ReadLine();
                if (line == null)
                {
                    page.End = new PositionItem(position);
                    if (page.Length > 0)
                    {
                        items.Add(page);
                    }
                    break;
                }
                for (int i = 0; i < line.Length; i++)
                {
                    var fontW = FontWidth((char?)line[i]);
                    if (x +  fontW > maxW)
                    {
                        x = 0;
                        y += fontH;
                        if (y >= maxH)
                        {
                            y = 0;
                            page.End = new PositionItem(position, i - 1);
                            items.Add(page);
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
                    items.Add(page);
                    page = new PagePositionItem()
                    {
                        Begin = new PositionItem(Content.Position, 0),
                    };
                }
            }
            return items;
        }

        public PageItem GetPage(PagePositionItem page)
        {
            return GetPage(page.Begin);
        }

        public PageItem GetPage(PositionItem begin)
        {
            var data = new PageItem()
            {
                Begin = begin,
                Data = new List<CharItem>(),
            };
            if (Content == null)
            {
                return data;
            }
            Content.Position = begin.Position >= 0 ? begin.Position : 0;
            var maxH = PageInnerHeight;
            var maxW = PageInnerWidth;
            var fontH = FontSize + LineSpace;
            var x = .0;
            var y = .0;
            var offset = begin.Offset;
            while (true)
            {
                var position = Content.Position;
                var line = Content.ReadLine();
                if (line == null)
                {
                    break;
                }
                int i = data.Data.Count < 1 ? offset : 0;
                var isEnd = false;
                for (; i < line.Length; i++)
                {
                    var code = line[i];
                    var fontW = FontWidth((char?)code);
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

        public PageItem GetPage(long position)
        {
            return GetPage(new PositionItem(position));
        }

        public IList<PageItem> GetPages(PagePositionItem page, int count)
        {
            return GetPages(page.Begin, count);
        }

        public IList<PageItem> GetPages(PositionItem begin, int count)
        {
            var items = new List<PageItem>();
            var position = begin;
            for (int i = 0; i < count; i++)
            {
                var page = GetPage(position);
                ResetPage(PageX(i), PageY(i), ref page);
                items.Add(page);
                position = page.End + 1;
            }
            return items;
        }

        public IList<PageItem> GetPages(long position, int count)
        {
            return GetPages(new PositionItem(position), count);
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

        public void Refresh()
        {
            CachePages = GetPages();
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

        /// <summary>
        /// 获取当前页
        /// </summary>
        /// <returns></returns>
        public IList<PageItem> Get()
        {
            var item = CachePages[Page];
            return GetPages(item, ColumnCount);
        }

        /// <summary>
        /// 获取下一页
        /// </summary>
        /// <returns></returns>
        public IList<PageItem> GetNext()
        {
            if (!CanNext)
            {
                return new List<PageItem>();
            }
            Page++;
            return Get();
        }

        /// <summary>
        /// 获取上一页
        /// </summary>
        /// <returns></returns>
        public IList<PageItem> GetPrevious()
        {
            if (!CanPrevious)
            {
                return new List<PageItem>();
            }
            Page --;
            return Get();
        }

        public void Dispose()
        {
            Content?.Dispose();
            CachePages.Clear();
        }
    }
}
