using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ZoDream.Shared.Interfaces;
using ZoDream.Shared.Models;
using ZoDream.Shared.Storage;
using ZoDream.Shared.Tokenizers;

namespace ZoDream.Shared.Plugins.Txt
{
    public partial class TxtReader(Stream input, 
        Encoding encoding,
        string fileName, 
        Regex splitRule) : INovelReader
    {
        const int MaxLengthWithRule = 102400;
        const int MaxLengthWithoutRule = 10 * 1024;
        public TxtReader(Stream input, string fileName)
            : this(input, fileName, NovelRuleRegex())
        {
            
        }

        public TxtReader(Stream input, string fileName, string splitRule)
            : this(input, fileName, new Regex(splitRule))
        {
            
        }

        public TxtReader(Stream input, string fileName, Regex splitRule)
            : this (input, GetEncoding(input), fileName, splitRule)
        {
            
        }

        /// <summary>
        /// 是否添加限制最大字数
        /// </summary>
        public bool IsMaxLimit { get; set; } = false;
        public Encoding Encoding => encoding;

        public INovelBasic ReadBasic()
        {
            return new NovelBasic(Parse(Path.GetFileNameWithoutExtension(fileName), out var author, out _))
            {
                Author = author
            };
        }

        public INovelDocument Read()
        {
            var res = new RichDocument(Parse(Path.GetFileNameWithoutExtension(fileName), out var author, out _))
            {
                Author = author
            };
            input.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(input, encoding);
            var isMatchRule = true;
            var bodyLength = 0L;
            INovelSection last = new NovelSection(string.Empty);
            while (true)
            {
                var line = reader.ReadLine();
                if (line == null)
                {
                    break;
                }
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }
                if (TryMatch(line, out var volume, out var title))
                {
                    
                    if (!string.IsNullOrWhiteSpace(last.Title) || last.Items.Count > 0)
                    {
                        res.Add(last);
                    }
                    if (!string.IsNullOrEmpty(volume)
                        && res.Items.LastOrDefault()?.Name != volume)
                    {
                        res.Add(new NovelVolume(volume));
                    }
                    last = new NovelSection(title);
                    bodyLength = 0;
                    isMatchRule = true;
                    continue;
                }
                bodyLength += line.Length;
                if (IsMaxLimit && bodyLength > (isMatchRule ? MaxLengthWithRule : MaxLengthWithoutRule)
                    && line.Length < 30)
                {
                    if (!string.IsNullOrWhiteSpace(last.Title) || last.Items.Count > 0)
                    {
                        res.Add(last);
                    }
                    last = new NovelSection(line.Trim());
                    bodyLength = 0;
                    isMatchRule = false;
                    continue;
                }
                last.Items.Add(new NovelTextBlock(line.Trim()));
            }
            if (!string.IsNullOrWhiteSpace(last.Title) || last.Items.Count > 0)
            {
                res.Add(last);
            }
            return res;
        }

        /// <summary>
        /// 读取章节
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task ReadAsync(IAsyncObservableCollection<string> data)
        {
            data.Start();
            input.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(input, encoding);
            var volumeItems = new HashSet<string>();
            while (!data.IsPaused)
            {
                var line = await reader.ReadLineAsync();
                if (line == null)
                {
                    break;
                }
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }
                if (!TryMatch(line, out var volume, out var title))
                {
                    continue;
                }
                if (!string.IsNullOrEmpty(volume) && !volumeItems.Contains(volume))
                {
                    data.Add(volume);
                    volumeItems.Add(volume);
                }
                if (!string.IsNullOrEmpty(title))
                {
                    data.Add(title);
                }
            }
            data.Stop();
        }

        private bool TryMatch(string line, out string volume, out string section)
        {
            volume = string.Empty;
            section = string.Empty;
            var match = splitRule.Match(line);
            if (!match.Success)
            {
                return false;
            }
            if (match.Groups.TryGetValue("volume", out var group))
            {
                volume = group.Value.Trim();
            }
            if (match.Groups.TryGetValue("section", out var next))
            {
                section = next.Value.Trim();
            }
            if (group is not null && string.IsNullOrEmpty(section))
            {
                section = line[(group.Index + group.Length)..].Trim();
            }
            if (group is null && string.IsNullOrEmpty(section))
            {
                section = line.Trim();
            }
            return true;
        }

        public void Dispose()
        {
            input.Dispose();
        }

        

        #region 编解码书籍文件名信息
        public static string Format(string name, string author, string category)
        {
            var res = name;
            if (!string.IsNullOrWhiteSpace(author))
            {
                res = $"{res} 作者：{author}";
            }
            if (!string.IsNullOrWhiteSpace(category))
            {
                res = $"【{category}】{res}";
            }
            return res;
        }

        public static string Parse(string text, out string author, out string category)
        {
            author = string.Empty;
            category = string.Empty;
            if (string.IsNullOrWhiteSpace(text))
            {
                return string.Empty;
            }
            // [科幻]书名 作者：z
            // 【科幻】书名 作者：z
            // 《书名》z
            // 书名 作者：z
            var data = text.AsSpan();
            var offset = 0;
            static int IndexOf(ReadOnlySpan<char> data, int begin, Func<char, bool> cb)
            {
                for (int i = begin; i < data.Length; i++)
                {
                    if (cb.Invoke(data[i]))
                    {
                        return i;
                    }
                }
                return -1;
            }
            static void SkipEmpty(ReadOnlySpan<char> data, ref int offset)
            {
                for (int i = offset; i < data.Length; i++)
                {
                    if (data[i] is ' ' or '\t' or '　')
                    {
                        continue;
                    }
                    offset = i;
                    break;
                }
            }
            SkipEmpty(data, ref offset);
            if (data[offset] is '【' or '[')
            {
                var i = IndexOf(data, offset + 1, code => code is '】' or ']');
                if (i > 0)
                {
                    category = data[(offset + 1)..i].ToString();
                    offset = i + 1;
                    SkipEmpty(data, ref offset);
                }
            }
            var name = string.Empty;
            if (data[offset] is '《')
            {
                var i = IndexOf(data, offset + 1, code => code is '》');
                if (i > 0)
                {
                    name = data[(offset + 1)..i].ToString();
                    offset = i + 1;
                    SkipEmpty(data, ref offset);
                }
            }
            for (int i = offset; i < data.Length - 2; i++)
            {
                if (data[i] == '作' && data[i + 1] == '者' && data[i + 2] is ':' or '：')
                {
                    author = data[(i + 3)..].Trim().ToString();
                    return string.IsNullOrEmpty(name) ? data[offset..i].Trim().ToString() : name;
                }
            }
            if (!string.IsNullOrEmpty(name))
            {
                author = data[offset..].Trim().ToString();
                return name;
            }
            return data[offset..].Trim().ToString();
        }
        #endregion

        private static Encoding GetEncoding(Stream input)
        {
            return TxtEncoder.GetEncoding(input, Encoding.GetEncoding("gb2312"));
        }


        [GeneratedRegex(@"^(正文)?\s{0,6}第?\s*[0-9零一二三四五六七八九十百千]{1,10}[章回节卷集幕计]?.{0,20}$")]
        internal static partial Regex NovelRuleRegex();
    }
}
