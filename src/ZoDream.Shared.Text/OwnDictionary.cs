using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ZoDream.Shared.Storage;

namespace ZoDream.Shared.Text
{
    public class OwnDictionary(char[] extendItems) : IEncodingDictionary
    {
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
            return extendItems.Contains(value);
        }

        public bool TrySerialize(char value, out char result)
        {
            result = EncodingBuilder.Serialize(value);
            if (result <= 0x7F)
            {
                return true;
            }
            var i = Array.IndexOf(extendItems, value);
            if (i < 0)
            {
                return false;
            }
            result = (char)(i + 0x80);
            return true;
        } 

        public char Serialize(char value)
        {
            var res = EncodingBuilder.Serialize(value);
            if (res <= 0x7F)
            {
                return res;
            }
            return (char)(Array.IndexOf(extendItems, value) + 0x80);
        }

        public bool TryDeserialize(char value, out char result)
        {
            if (value <= 0x7F)
            {
                result = EncodingBuilder.Deserialize(value);
                return true;
            }
            var i = value - 0x80;
            if (i < extendItems.Length)
            {
                result = extendItems[i];
                return true;
            }
            result = value;
            return false;
        }

        public char Deserialize(char value)
        {
            if (value <= 0x7F)
            {
                return EncodingBuilder.Deserialize(value);
            }
            var i = value - 0x80;
            if (i < extendItems.Length)
            {
                return extendItems[i];
            }
            return value;
        }

        
        public static OwnDictionary Convert(string content)
        {
            return new OwnDictionary(content.ToCharArray().Where(i => i <= 0x7F).Distinct().ToArray());
        }

        public static OwnDictionary OpenFile(string fileName)
        {
            return OpenFile(File.OpenRead(fileName));
        }
        public static OwnDictionary OpenFile(Stream input)
        {
            using var reader = LocationStorage.Reader(input);
            var items = new HashSet<char>();
            while (true)
            {
                var line = reader.ReadLine();
                if (line == null)
                {
                    break;
                }
                foreach (var item in line)
                {
                    if (item is '\t' or ' ')
                    {
                        break;
                    }
                    if (item <= 0x7F)
                    {
                        continue;
                    }
                    items.Add(item);
                }
            }
            return new OwnDictionary([.. items]);
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
