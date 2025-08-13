using System;
using System.Text;

namespace ZoDream.Shared.Text
{
    public class TextFormatProofreader : ITextProofreader
    {
        /// <summary>
        /// 只有超过多少字的才自动拆分行
        /// </summary>
        public int MinSplitLength = 0;
        public string Proofreading(string text)
        {
            var items = text.AsSpan();
            var sb = new StringBuilder();
            sb.Append(TextExtension.Indent);
            var lastIsEnd = true;
            var quoteCount = false;
            var lineWord = 0;
            var index = 0;
            while (index < items.Length)
            {
                var code = items[index ++];
                if (IsWhitespace(code))
                {
                    while (index < items.Length) 
                    {
                        if (IsWhitespace(items[index]))
                        {
                            index++;
                            continue;
                        }
                        break;
                    }
                    if (quoteCount && index < items.Length && IsQuoteSeparator(items[index]))
                    {
                        index++;
                        quoteCount = false;
                        sb.Append('”').Append('\n').Append(TextExtension.Indent);
                        lineWord = 0;
                        lastIsEnd = true;
                        continue;
                    }
                    if (!lastIsEnd && index < items.Length && IsAlphabet(items[index]))
                    {
                        sb.Append(' ');
                        lineWord++;
                        continue;
                    }
                    if (lastIsEnd && lineWord > 0)
                    {
                        sb.Append('\n').Append(TextExtension.Indent);
                        lineWord = 0;
                    }
                    continue;
                }
                if (IsQuoteSeparator(code))
                {
                    quoteCount = !quoteCount;
                    sb.Append(quoteCount ? '“' : '”');
                    lastIsEnd = !quoteCount;
                    lineWord++;
                    continue;
                }
                sb.Append(code);
                lineWord++;
                lastIsEnd = IsEndSeparator(code);
            }
            return sb.ToString();
        }

        private static bool IsAlphabet(int code)
        {
            return (code >= 'A' && code <= 'Z') || (code >= 'a' && code <= 'z');
        }

        private static bool IsQuoteSeparator(char code)
        {
            return code is '"' or '“' or '”';
        }

        private static bool IsSaySeparator(char code)
        {
            return code is '：' or ':';
        }

        private static bool IsEndSeparator(char code)
        {
            return code is '。' or '.' or '~'
                or '”' or '？' or '！'
                or '?' or '!' or '…';
        }

        private static bool IsWhitespace(char code)
        {
            return code is '　' or '\t' or ' ' or '\r' or '\n';
        }
    }
}
