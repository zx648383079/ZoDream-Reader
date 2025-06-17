using System.Collections.Generic;
using System.Linq;
using ZoDream.Shared.Storage;

namespace ZoDream.Shared.Plugins.Own
{
    public class EncodingBuilder
    {
        private readonly OwnDictionary _dict = new();
        public Dictionary<char, int> Items = [];

        public int Count => Items.Count;
        /// <summary>
        /// 排序后的所有字符
        /// </summary>
        public IEnumerable<char> Values => Items.OrderByDescending(i => i.Value).Select(i => i.Key);
        /// <summary>
        /// 过滤标点后的字符
        /// </summary>
        public IEnumerable<char> FilteredValues => Values.Where(i => !_dict.IsExclude(i));
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
        /// 追加字符
        /// </summary>
        /// <param name="code"></param>
        public void Append(char code)
        {
            var formatted = _dict.Serialize(code);
            if (formatted is '\t' or ' ' or '\n' or '\r')
            {
                return;
            }
            if (Items.TryAdd(formatted, 1))
            {
                return;
            }
            Items[formatted]++;
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
            using var writer = LocationStorage.Writer(fileName);
            foreach (var item in FilteredValues)
            {
                writer.WriteLine(item);
            }
        }
    }
}
