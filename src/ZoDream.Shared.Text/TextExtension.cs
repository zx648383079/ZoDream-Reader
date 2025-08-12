using System.Collections.Generic;
using System.Text;
using ZoDream.Shared.Interfaces;
using ZoDream.Shared.Tokenizers;

namespace ZoDream.Shared.Text
{
    public static class TextExtension
    {
        public const string Indent = "    ";

        public static string Format(this IEnumerable<INovelBlock> items)
        {
            var sb = new StringBuilder();
            Format(sb, items);
            return sb.ToString();
        }

        public static void Format(StringBuilder writer, IEnumerable<INovelBlock> items)
        {
            foreach (var item in items)
            {
                if (item is INovelTextBlock o && !string.IsNullOrWhiteSpace(o.Text))
                {
                    writer.Append(Indent);
                    writer.Append(o.Text);
                    writer.Append('\n');
                }
            }
        }

        public static string Format(this INovelSection data)
        {
            var sb = new StringBuilder();
            sb.Append(data.Title).Append('\n').Append('\n');
            Format(sb, data.Items);
            return sb.ToString();
        }

        public static void Parse(this IList<INovelBlock> items, string text)
        {
            items.Clear();
            foreach (var item in text.Split(['\n', '\r']))
            {
                if (!string.IsNullOrWhiteSpace(item))
                {
                    items.Add(new NovelTextBlock(item.Trim()));
                }
            }
        }
    }
}
