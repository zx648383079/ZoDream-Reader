using Microsoft.International.Converters.TraditionalChineseToSimplifiedConverter;
using System;
using System.Collections.Generic;
using System.Linq;
using ZoDream.Shared.Storage;

namespace ZoDream.Shared.Text
{
    public class EncodingBuilder : Dictionary<char, int>
    {
        /// <summary>
        /// 排序后的所有字符
        /// </summary>
        public IEnumerable<char> SortedValues => this.OrderByDescending(i => i.Value).ThenBy(i => i.Key).Select(i => i.Key);
        /// <summary>
        /// 过滤标点后的字符
        /// </summary>
        public IEnumerable<char> FilteredValues => SortedValues.Where(i => !IsExclude(i));

        public IEnumerable<KeyValuePair<char, char>> TraditionalItems => Keys.Select(i => new KeyValuePair<char, char>(i, ToSimplified(i))).Where(i => i.Key != i.Value);
        /// <summary>
        /// 追加字符串
        /// </summary>
        /// <param name="text"></param>
        public void Append(string text)
        {
            foreach (var item in text)
            {
                Append(item);
            }
        }
        /// <summary>
        /// 追加字符，并处理一些字符
        /// </summary>
        /// <param name="code"></param>
        public void Append(char code)
        {
            var formatted = Deserialize(Serialize(code));
            if (formatted is '\t' or ' ' or '\n' or '\r')
            {
                return;
            }
            if (TryAdd(formatted, 1))
            {
                return;
            }
            this[formatted]++;
        }

        /// <summary>
        /// 追加文件
        /// </summary>
        /// <param name="fileName"></param>
        public void AppendFile(string fileName)
        {
            using var reader = LocationStorage.Reader(fileName);
            while (true)
            {
                var line = reader.ReadLine();
                if (line == null)
                {
                    break;
                }
                Append(line);
            }
        }

        /// <summary>
        /// 保存字典为文件
        /// </summary>
        /// <param name="fileName"></param>
        public void SaveAs(string fileName)
        {
            OwnDictionary.WriteFile(fileName, FilteredValues);
        }

        public static bool IsExclude(char value)
        {
            return value <= 0x7f || Serialize(value) <= 0x7F;
        }

        /// <summary>
        /// 转化一些特殊符号
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static char Serialize(char value)
        {
            if (value >= 0xFF01 && value <= 0xFF5E)
            {
                value = (char)(value - 0xFF00 + 0x20);
            }
            if (value >= 'a' && value <= 'z')
            {
                // 字母全大写 0x15 - 0x46
                return (char)(value - 'a' + 'A');
            }
            return value switch
            {
                // 移除字符
                '"' or '〃' => (char)0xB, // '“',
                //全角转半角
                '′' => '\'',
                '﹢' => '+',
                '丨' => '|',
                '﹩' or '￥' => '$',
                '　' or ' ' or ' ' => ' ',
                '？' => '?',
                '！' => '!',
                '，' => ',',
                '）' => ')',
                '（' => '(',
                '；' or '﹔' => ';',
                '＊' or '※' => '*',
                '：' or '∶' or '︰' or '﹕' => ':',
                '【' or '〖' => '[',
                '】' or '〗' => ']',
                '．' => '.',
                '～' => '~',
                '―' or '─' or '－' or '—' or 'ー' => '-',

                // 一下是替换
                '“' => (char)0xB,
                '”' => (char)0xC,
                '‘' => (char)0xD,
                '’' => (char)0xE,
                '、' or '﹑' => (char)0xF,
                '「' or '『' => (char)0x10,
                '」' or '』' => (char)0x11,
                '《' => (char)0x12,
                '》' => (char)0x13,
                '。' => (char)0x14,
                '…' or '┅' or '┈' => (char)0x22,
                '°' => (char)0x60,

                '一' => (char)0x15,
                '二' => (char)0x16,
                '三' => (char)0x17,
                '四' => (char)0x18,
                '五' => (char)0x19,
                '六' => (char)0x1A,
                '七' => (char)0x1B,
                '八' => (char)0x1C,
                '九' => (char)0x1D,
                '十' => (char)0x1E,
                '百' => (char)0x1F,
                //'千' => (char)0x7F,
                //'万' => (char)0x80,
                //'忆' => (char)0x81,

                '·' => (char)0x7F,
                '艹' => '草',
                _ => value,
            };
        }

        public static char Deserialize(char value)
        {
            return value switch
            {
                // 替换
                (char)0xB => '“',
                (char)0xC => '”',
                (char)0xD => '‘',
                (char)0xE => '’',
                (char)0xF => '、',
                (char)0x10 => '「',
                (char)0x11 => '」',
                (char)0x12 => '《',
                (char)0x13 => '》',
                (char)0x14 => '。',
                (char)0x22 => '…',
                (char)0x60 => '°',

                (char)0x15 => '一',
                (char)0x16 => '二',
                (char)0x17 => '三',
                (char)0x18 => '四',
                (char)0x19 => '五',
                (char)0x1A => '六',
                (char)0x1B => '七',
                (char)0x1C => '八',
                (char)0x1D => '九',
                (char)0x1E => '十',
                (char)0x1F => '百',
                // (char)0x7F => '千',
                // (char)0x80 => '万',
                // (char)0x81 => '忆',

                (char)0x7F => '·',
                _ => value
            };
        }

        /// <summary>
        /// 载入字典
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static EncodingBuilder OpenFile(string fileName)
        {
            using var reader = LocationStorage.Reader(fileName);
            var res = new EncodingBuilder();
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
                    res.TryAdd(item, 0);
                }
            }
            return res;
        }

        /// <summary>
        /// 一些特别
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static bool IsExcludeSimplified(char code)
        {
            return code is '著' or '瞭' or '捍' or '徬' or '胄' or '妳';
        }
        /// <summary>
        /// 转换成简体
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static char ToSimplified(char code)
        {
            if (IsExcludeSimplified(code))
            {
                return code;
            }
            var res = ChineseConverter.Convert(code.ToString(), ChineseConversionDirection.TraditionalToSimplified);
            return res.Length == 1 ? res[0] : code;
        }
        /// <summary>
        /// 判断字符是是简体
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static bool IsSimplified(char code)
        {
            return code < 0xFF || ToSimplified(code) == code;
        }

        /// <summary>
        /// 批量替换字符
        /// </summary>
        /// <param name="items">旧字符,新字符</param>
        public void Replace(IEnumerable<KeyValuePair<char, char>> items)
        {
            foreach (var item in items)
            {
                Replace(item.Key, item.Value);
            }
        }
        /// <summary>
        /// 替换字符
        /// </summary>
        /// <param name="search"></param>
        /// <param name="replace"></param>
        public void Replace(char search, char replace)
        {
            if (!TryGetValue(search, out var count))
            {
                return;
            }
            Remove(search);
            if (TryAdd(replace, count))
            {
                return;
            }
            this[replace] += count;
        }

        public static explicit operator OwnDictionary(EncodingBuilder data)
        {
            return new([..data.FilteredValues]);
        }

        public static char[] operator -(EncodingBuilder main, EncodingBuilder diff) 
        {
            return [.. main.FilteredValues.Where(i => !diff.ContainsKey(i))];
        }
        public static char[] operator -(EncodingBuilder main, OwnDictionary diff)
        {
            return [.. main.Keys.Where(i => !diff.Contains(i))];
        }
    }
}
