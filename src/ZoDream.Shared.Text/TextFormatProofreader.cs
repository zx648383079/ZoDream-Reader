using System;
using System.Text;

namespace ZoDream.Shared.Text
{
    public class TextFormatProofreader : ITextProofreader
    {
        private readonly char[] _chatCodes = ['“', '”'];
        private readonly char[] _endCodes = ['“', '”', '!', '?', '.', '！', '？', '。'];
        /// <summary>
        /// 只有超过多少字的才自动拆分行
        /// </summary>
        public int MinSplitLength = 0;
        public string Proofreading(string text)
        {
            var sb = new StringBuilder();
            var lastNotEnd = false;
            foreach (var item in text.Split(['\n', '\r']))
            {
                var line = item.Trim();
                if (string.IsNullOrEmpty(line))
                {
                    continue;
                }
                if (!line.Contains('“') && !line.Contains('”'))
                {
                    line = line.Replace('「', '“').Replace('」', '”');
                }
                if (line.StartsWith('”'))
                {
                    line = line[1..];
                    if (sb.Length > 0)
                    {
                        sb.Append('”');
                    }
                    lastNotEnd = false;
                }
                if (IsJustSeparator(line))
                {
                    continue;
                }
                if (line.Length < MinSplitLength)
                {
                    if (sb.Length != 0)
                    {
                        sb.Append('\n');
                    }
                    sb.Append(TextExtension.Indent);
                    sb.Append(line);
                    lastNotEnd = false;
                    continue;
                }
                var start = 0;
                var isInlineIndex = 0;
                var blockIndex = 0;
                while (start < line.Length)
                {
                    blockIndex++;
                    var beginIsChatStart = line[start] == _chatCodes[0];
                    var end = line.IndexOfAny(beginIsChatStart ? _chatCodes : _endCodes, beginIsChatStart ? start + 1 : start);
                    var endIsChatStart = end >= 0 && line[end] == _chatCodes[0];
                    if (end == 0 && endIsChatStart)
                    {
                        if (lastNotEnd)
                        {
                            lastNotEnd = false;
                            sb.Append('。');
                        }
                    }
                    if (end < 0)
                    {
                        end = line.Length;
                    }
                    else if (!endIsChatStart)
                    {
                        end++;
                    }
                    var block = line[start..end];
                    if (lastNotEnd || (start > 0 && IsSaySeparator(line[start - 1])) || (isInlineIndex > 0 && blockIndex - isInlineIndex < 3))
                    {
                        sb.Append(block);
                    }
                    else
                    {
                        if (sb.Length != 0)
                        {
                            sb.Append('\n');
                        }
                        sb.Append(TextExtension.Indent);
                        sb.Append(block);
                    }
                    lastNotEnd = !HasEndChar(block);
                    if (endIsChatStart)
                    {
                        isInlineIndex = lastNotEnd ? blockIndex : 0;
                    }
                    start = end;
                }
            }
            return sb.ToString();
        }

        private static bool HasEndChar(string line)
        {
            if (line.Length == 0)
            {
                return true;
            }
            if (!IsEndChat(line))
            {
                return IsEndSeparator(line[^1]);
            }
            return IsSeparator(line[^1]);
        }

        private static bool IsEndChat(string line)
        {
            var i = line.LastIndexOf('“');
            return i < 0 || line.IndexOf('”', i) > 0;
        }

        private static bool IsJustSeparator(string line)
        {
            for (int i = 0; i < line.Length; i++)
            {
                if (!IsSeparator(line[i]) && !IsWhitespace(line[i]))
                {
                    return false;
                }
            }
            return true;
        }

        private static bool IsSeparator(char code)
        {
            if (IsEndSeparator(code))
            {
                return true;
            }
            if (IsSaySeparator(code))
            {
                return true;
            }
            return code is '.' or ',' or '，' or '）'
                or '…' or '＊' or '*' or '】' or '~';
        }

        private static bool IsSaySeparator(char code)
        {
            return code is '：' or ':';
        }

        private static bool IsEndSeparator(char code)
        {
            return code is '。'
                or '”' or '？' or '！'
                or '?' or '!';
        }

        private static bool IsWhitespace(char code)
        {
            return code is '　' or '\t' or ' ' or '\r' or '\n';
        }
    }
}
