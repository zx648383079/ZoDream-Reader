using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text;

namespace ZoDream.Shared.Text
{
    public class OwnDictionary : IEncodingDictionary
    {
        public OwnDictionary(char[] items)
        {
            _charItems = items;
            _charToIndex = items.Select((i, j) => KeyValuePair.Create(i, j))
                .ToImmutableDictionary();
        }

        private readonly char[] _charItems;
        private readonly ImmutableDictionary<char, int> _charToIndex;
        /// <summary>
        /// 是否包含字符
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Contains(char value)
        {
            var result = EncodingBuilder.Serialize(value);
            if (result <= 0x7F)
            {
                return true;
            }
            return _charToIndex.ContainsKey(value);
        }

        public bool TrySerialize(char value, out char result)
        {
            result = EncodingBuilder.Serialize(value);
            if (result <= 0x7F)
            {
                return true;
            }
            if (!_charToIndex.TryGetValue(value, out var i))
            {
                return false;
            }
            result = (char)(i + 0x80);
            return true;
        } 

        public char Serialize(char value)
        {
            if (TrySerialize(value, out var res))
            {
                return res;
            }
            return res;
        }

        public bool TryDeserialize(char value, out char result)
        {
            return TryDeserialize(value, false, out result);
        }

        public bool TryDeserialize(char value, bool lastIsAscii, out char result)
        {
            if (value <= 0x7F)
            {
                result = EncodingBuilder.Deserialize(value, lastIsAscii);
                return true;
            }
            var i = value - 0x80;
            if (i < _charItems.Length)
            {
                result = _charItems[i];
                return true;
            }
            result = value;
            return false;
        }

        public char Deserialize(char value)
        {
            TryDeserialize(value, out var res);
            return res;
        }

        
        public static OwnDictionary Convert(string content)
        {
            return new OwnDictionary(content.ToCharArray().Where(i => i <= 0x7F).Distinct().ToArray());
        }

        public static OwnDictionary OpenFile(string fileName)
        {
            using var fs = File.OpenRead(fileName);
            return OpenFile(fs);
        }
        public static OwnDictionary OpenFile(Stream input)
        {
            var res = new HashSet<char>();
            foreach (var items in DictionaryBuilder.ReadFile(input))
            {
                foreach (var item in items[0])
                {
                    if (EncodingBuilder.IsExclude(item))
                    {
                        continue;
                    }
                    res.Add(item);
                }
            }
            return new OwnDictionary([.. res]);
        }


        public static void WriteFile(string fileName, IEnumerable<char> items)
        {
            WriteFile(File.Create(fileName), items);
        }

        public static void WriteFile(Stream output, IEnumerable<char> items)
        {
            using var writer = new StreamWriter(output, Encoding.UTF8);
            foreach (var item in items)
            {
                if (EncodingBuilder.IsExclude(item))
                {
                    continue;
                }
                writer.WriteLine(item);
            }
        }
    }
}
