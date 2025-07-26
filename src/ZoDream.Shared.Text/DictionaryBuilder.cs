using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using ZoDream.Shared.Storage;

namespace ZoDream.Shared.Text
{
    public partial class DictionaryBuilder : IEncodingDictionary
    {

        private readonly HashSet<char> _source = [];

        private readonly Dictionary<char, int> _counter = [];

        private readonly Dictionary<string, string> _replaceItems = [];

        public IEnumerable<KeyValuePair<string, string>> ReplaceItems => _replaceItems;

        public IEnumerable<char> Items 
        {
            get {
                foreach (var item in _source)
                {
                    yield return item;
                }
                foreach (var item in _counter.OrderByDescending(i => i.Value)
                    .ThenBy(i => i.Key).Select(i => i.Key))
                {
                    if (_source.Contains(item))
                    {
                        continue;
                    }
                    yield return item;
                }
            }
        }

        public bool Contains(char value)
        {
            return _source.Contains(value);
        }

        private int IndexOf(char value)
        {
            var i = 0;
            foreach (var item in _source)
            {
                if (item == value)
                {
                    return i;
                }
                i++;
            }
            return -1;
        }

        public bool TrySerialize(char value, out char result)
        {
            result = EncodingBuilder.Serialize(value);
            if (result <= 0x7F)
            {
                return true;
            }
            var i = IndexOf(value);
            if (i < 0)
            {
                return false;
            }
            result = (char)(i + 0x80);
            return true;
        }

        public bool TryDeserialize(char value, out char result)
        {
            if (value <= 0x7F)
            {
                result = EncodingBuilder.Deserialize(value);
                return true;
            }
            var i = value - 0x80;
            if (i < _source.Count)
            {
                result = _source.ElementAt(i);
                return true;
            }
            result = value;
            return false;
        }

        /// <summary>
        /// 追加字符串
        /// </summary>
        /// <param name="text"></param>
        public void Add(string text)
        {
            foreach (var item in text)
            {
                Add(item);
            }
        }
        /// <summary>
        /// 追加字符，并处理一些字符
        /// </summary>
        /// <param name="code"></param>
        public void Add(char code)
        {
            Add(code, 1);
        }

        public void Add(char code, int count)
        {
            var formatted = EncodingBuilder.Deserialize(EncodingBuilder.Serialize(code));
            if (formatted is '\t' or ' ' or '\n' or '\r')
            {
                return;
            }
            if (_counter.TryAdd(formatted, count))
            {
                return;
            }
            _counter[formatted] += count;
        }
        /// <summary>
        /// 会先清除原始计数
        /// </summary>
        /// <param name="items"></param>
        public void Add(EncodingBuilder items)
        {
            _counter.Clear();
            foreach (var item in items)
            {
                Add(item.Key, item.Value);
            }
        }
        public void Add(IDictionary<char, int> items)
        {
            foreach (var item in items)
            {
                Add(item.Key, item.Value);
            }
        }

        /// <summary>
        /// 追加文件
        /// </summary>
        /// <param name="fileName"></param>
        public void AddFile(string fileName)
        {
            using var reader = LocationStorage.Reader(fileName);
            while (true)
            {
                var line = reader.ReadLine();
                if (line == null)
                {
                    break;
                }
                Add(line);
            }
        }
        
        public void Replace(string search,  string replacement)
        {
            if (search == replacement)
            {
                return;
            }
            if (!_replaceItems.TryAdd(search, replacement))
            {
                _replaceItems[search] = replacement;
            }
        }

        private void AddWord(string words)
        {
            foreach (var item in words)
            {
                _source.Add(item);
            }
        }

        private IEnumerable<string> GetLinked(string replacement)
        {
            foreach (var item in _replaceItems)
            {
                if (item.Value == replacement)
                {
                    yield return item.Key;
                }
            }
        }

        /// <summary>
        /// 保存字典为文件
        /// </summary>
        /// <param name="fileName"></param>
        public void SaveAs(string fileName)
        {
            SaveAs(File.Create(fileName));
        }

        public void SaveAs(Stream output)
        {
            using var writer = new StreamWriter(output, Encoding.UTF8);
            foreach (var item in _source)
            {
                writer.Write(item);
                foreach (var it in GetLinked(item.ToString()))
                {
                    writer.Write(' ');
                    writer.Write(it);
                }
                writer.WriteLine();
            }
            foreach (var item in _counter.OrderByDescending(i => i.Value)
                .ThenBy(i => i.Key).Select(i => i.Key))
            {
                if (_source.Contains(item))
                {
                    continue;
                }
                writer.Write(item);
                foreach (var it in GetLinked(item.ToString()))
                {
                    writer.Write(' ');
                    writer.Write(it);
                }
                writer.WriteLine();
            }
            var exclude = new HashSet<string>();
            foreach (var item in _replaceItems)
            {
                if (item.Value.Length < 2 || exclude.Contains(item.Value))
                {
                    continue;
                }
                exclude.Add(item.Value);
                writer.Write(item.Value);
                foreach (var it in GetLinked(item.Value.ToString()))
                {
                    writer.Write(' ');
                    writer.Write(it);
                }
                writer.WriteLine();
            }
        }

        public static DictionaryBuilder OpenFile(string fileName)
        {
            var res = new DictionaryBuilder();
            foreach (var items in ReadFile(fileName))
            {
                res.AddWord(items[0]);
                for (var i = 1; i < items.Length; i++)
                {
                    res.Replace(items[i], items[0]);
                }
            }
            return res;
        }

        public static IEnumerable<string[]> ReadFile(string fileName)
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
                args = args.Where(i => !string.IsNullOrEmpty(i)).Distinct().ToArray();
                if (args.Length == 0)
                {
                    continue;
                }
                yield return args;
            }
        }

        [GeneratedRegex(@"\\[uU]([0-9a-fA-F]+)")]
        private static partial Regex HexRegex();


    }
}
