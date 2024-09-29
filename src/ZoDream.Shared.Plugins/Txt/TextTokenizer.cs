using System.Collections.Generic;
using System.Linq;
using ZoDream.Shared.Interfaces;
using ZoDream.Shared.Interfaces.Entities;
using ZoDream.Shared.Interfaces.Tokenizers;
using ZoDream.Shared.Tokenizers;

namespace ZoDream.Shared.Plugins.Txt
{
    public class TextTokenizer: IPageTokenizer
    {
        public IList<INovelPage> Parse(INovelDocument content, 
            IReadTheme setting, ICanvasControl control)
        {
            if (content is not TextDocument target)
            {
                return [];
            }
            var theme = new CanvasTheme(setting, control, true);
            var maxH = theme.PageInnerHeight;
            var lines = ParseBlock(target.Title, theme);
            if (!string.IsNullOrWhiteSpace(target.Content))
            {
                foreach (var item in ParseBlock(target.Content, new CanvasTheme(setting, control)))
                {
                    lines.Add(item);
                }
            }
            var items = new List<INovelPage>();
            var page = new NovelPage();
            var y = .0;
            foreach (var item in lines)
            {
                var h = item.ActualHeight;
                if (y + h <= maxH)
                {
                    item.Y = y;
                    page.Add(item);
                    y += h;
                    continue;
                }
                items.Add(page);
                page = [];
                y = .0;
            }
            items.Add(page);
            return items;
        }

        private IList<INovelPageLine> ParseBlock(string content, ICanvasTheme theme)
        {
            var items = new List<INovelPageLine>();
            var i = 0;
            while (i < content.Length)
            {
                var line = ParseLine(content, ref i, theme);
                if (line == null)
                {
                    break;
                }
                items.Add(line);
            }
            return items;
        }

        private INovelPageLine? ParseLine(string content, ref int index, ICanvasTheme theme)
        {
            if (index >= content.Length)
            {
                return null;
            }
            var line = new NovelPageLine(theme);
            var maxW = theme.PageInnerWidth;
            var x = .0;
            while (index < content.Length)
            {
                var code = content[index];
                if (code == 0x0 || code == '\r')
                {
                    index++;
                    continue;
                }
                if (code == '\n')
                {
                    index++;
                    break;
                }
                var (fontW, fontH) = theme.FontBound(code);
                if (x + fontW > maxW)
                {
                    break;
                }
                line.Add(new NovelPageChar(code.ToString())
                {
                    X = x,
                    Width = fontW,
                    Height = fontH,
                });
                x += fontW;
                index++;
            }
            line.X = theme.TextAlign switch
            {
                1 => (theme.Width - x) / 2,
                2 => theme.Width - x - theme.PaddingRight,
                _ => theme.PaddingLeft,
            };
            return line;
        }
    }
}
