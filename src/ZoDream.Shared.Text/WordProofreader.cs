using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using ZoDream.Shared.Storage;

namespace ZoDream.Shared.Text
{
    public partial class WordProofreader
    {
        private readonly WordDictionary _correctItems = new();
        private readonly WordDictionary _mistakeItems = new();
        private readonly Dictionary<string, string> _mistakeToCorrect = [];

        public void AppendFile(string fileName)
        {
            using var reader = LocationStorage.Reader(fileName);
            while (true)
            {
                // [正确的字词][ \t][错误的字词][ \t][错误的字词]..
                var line = reader.ReadLine()?.Trim();
                if (line == null)
                {
                    break;
                }
                var args = HexRegex().Replace(line, match => {
                    return ((char)Convert.ToInt32(match.Groups[1].Value, 16)).ToString();
                }).Split([' ', '\t', '\r', '\n']);
                if (args[0].Length > 1)
                {
                    _correctItems.Insert(args[0]);
                }
                for (var i = 1; i < args.Length; i++)
                {
                    var item = args[i];
                    if (string.IsNullOrEmpty(item) || item == args[0])
                    {
                        continue;
                    }
                    _mistakeItems.Insert(item);
                    _mistakeToCorrect.TryAdd(item, args[0]);
                }
            }
        }
        /// <summary>
        /// 校对字符
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public string Proofreading(ReadOnlySpan<char> input)
        {
            var sb = new StringBuilder();
            var i = 0;
            while(i < input.Length)
            {
                var current = input[i..];
                var correct = _correctItems.FindLongestWord(current);
                var mistake = _mistakeItems.FindLongestWord(current);
                if (correct.Length > mistake.Length)
                {
                    sb.Append(correct);
                    i += correct.Length;
                } else if (mistake.Length > 0)
                {
                    i += mistake.Length;
                    sb.Append(_mistakeToCorrect[mistake]);
                } else
                {
                    sb.Append(input[i ++]);
                }
            }
            return sb.ToString();
        }

        [GeneratedRegex(@"\\[uU]([0-9a-fA-F]+)")]
        private static partial Regex HexRegex();
    }
}
