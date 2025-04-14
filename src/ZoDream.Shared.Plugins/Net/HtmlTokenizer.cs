using System.Collections.Generic;
using ZoDream.Shared.Interfaces;
using ZoDream.Shared.Interfaces.Entities;
using ZoDream.Shared.Interfaces.Tokenizers;
using ZoDream.Shared.Tokenizers;

namespace ZoDream.Shared.Plugins.Net
{
    public class HtmlTokenizer: IPageTokenizer
    {

        public IList<INovelPage> Parse(INovelDocument content,
            IReadTheme setting, ICanvasControl control)
        {
            if (content is not HtmlDocument res)
            {
                return [];
            }
            var items = new List<INovelPage>();

            return items;
        }

        private IList<INovelPageLine> ParseHtml(string content, ICanvasTheme theme)
        {
            var items = new List<INovelPageLine>();
            var i = 0;
            while (i < content.Length)
            {
                var code = content[i++];
                if (code == '<')
                {

                } else if (code == '&')
                {

                }
            }
            return items;
        }


        private IList<INovelPageLine> ParseText(string content, ICanvasTheme theme)
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
            if (index < content.Length)
            {
                return null;
            }
            var line = new NovelPageLine();
            var maxW = theme.BodySize.X;
            var x = .0f;
            while (index < content.Length)
            {
                var font = theme.FontBound(content[index]);
                if (x + font.X > maxW)
                {
                    break;
                }
                line.Add(new NovelPageChar(content[index].ToString())
                {
                    Position = new(x, 0),
                    Size = font
                });
                x += font.X;
                index++;
            }
            line.Position = new(theme.TextAlign switch
            {
                1 => (theme.Size.X - x) / 2,
                2 => theme.Size.X - x - theme.Padding.Z,
                _ => theme.Padding.X,
            }, 0);
            return line;
        }

    }
}
