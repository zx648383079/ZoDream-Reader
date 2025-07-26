using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ZoDream.Shared.Text
{
    public class WordProofreader
    {
        private readonly WordDictionary _correctItems = new();
        private readonly WordDictionary _mistakeItems = new();
        private readonly Dictionary<string, string> _mistakeToCorrect = [];

        public void AppendFile(string fileName)
        {
            using var fs = File.OpenRead(fileName);
            AppendFile(fs);
        }
        public void AppendFile(Stream input)
        {
            foreach (var items in DictionaryBuilder.ReadFile(input))
            {
                if (items[0].Length > 1)
                {
                    _correctItems.Insert(items[0]);
                }
                for (var i = 1; i < items.Length; i++)
                {
                    var item = items[i];
                    _mistakeItems.Insert(item);
                    _mistakeToCorrect.TryAdd(item, items[0]);
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
                if (!IsWord(input[i]))
                {
                    sb.Append(input[i++]);
                    continue;
                }
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

        private static bool IsWord(char value)
        {
            if (value is >= 'a' and <= 'z')
            {
                return true;
            }
            if (value is >= 'A' and <= 'Z')
            {
                return true;
            }
            return value > 0xFF;
        }
    }
}
