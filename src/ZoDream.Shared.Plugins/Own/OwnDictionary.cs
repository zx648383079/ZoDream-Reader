using System;
using System.Collections.Generic;
using System.Linq;
using ZoDream.Shared.Storage;

namespace ZoDream.Shared.Plugins.Own
{
    public class OwnDictionary(char[] extendItems)
    {

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
            using var reader = LocationStorage.Reader(fileName);
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
                    if (item <= 0x7F)
                    {
                        continue;
                    }
                    items.Add(item);
                }
            }
            return new OwnDictionary(items.ToArray());
        }
    }
}
