using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ZoDream.Shared.Interfaces;
using ZoDream.Shared.Interfaces.Entities;
using ZoDream.Shared.Repositories.Entities;
using ZoDream.Shared.Storage;
using ZoDream.Shared.Tokenizers;

namespace ZoDream.Shared.Plugins.Txt
{
    public class TxtReader : INovelSerializer
    {
        public TxtReader()
        {
            
        }

        public TxtReader(IEnumerable<string> chapterRules)
        {
            ChapterRuleItems = chapterRules;
        }

        private readonly int BufferSize = 512000;
        private readonly int MaxLengthWithoutRule = 10 * 1024;
        private readonly int MaxLengthWithRule = 102400;

        public IEnumerable<string>? ChapterRuleItems { get; private set; }

        public INovelSource CreateSource(INovelSourceEntity entry)
        {
            return new TxtSource(entry);
        }

        public Task<INovelChapter[]> GetChaptersAsync(INovelSource source)
        {
            return Task.Factory.StartNew(() => {
                using var fs = File.OpenRead(((FileSource)source).FileName);
                var (_, items) = GetChapters(fs);
                return items;
            });
        }

        public async Task<ISectionSource> GetChapterAsync(INovelSource source, INovelChapter chapter)
        {
            if (source is not TxtSource t)
            {
                throw new ArgumentException();
            }
            using var fs = File.OpenRead(t.FileName);
            fs.Seek(chapter.Begin, SeekOrigin.Begin);
            var buffer = new byte[chapter.End - chapter.Begin];
            fs.Read(buffer, 0, buffer.Length);
            return new TextDocument(chapter.Title, t.Encoding.GetString(buffer));
        }

        public Task<(INovel?, INovelChapter[])> LoadAsync(INovelSource source)
        {
            return Task.Factory.StartNew(() => {
                using var fs = File.OpenRead(((FileSource)source).FileName);
                return GetChapters(fs);
            });
        }

        public (INovel?, INovelChapter[]) GetChapters(Stream input)
        {
            var items = new List<INovelChapter>();
            var encoding = TxtEncoder.GetEncoding(input, Encoding.GetEncoding("gb2312"));
            var buffer = new byte[BufferSize];
            input.Read(buffer, 0, buffer.Length);
            var pattern = GetRule(encoding.GetString(buffer));
            input.Seek(0, SeekOrigin.Begin);
            var isMatchRule = true;
            var bodyLength = 0L;
            ChapterEntity? last = null;
            while (true)
            {
                var position = input.Position;

                var line = ReadLine(input, encoding);
                if (line == null)
                {
                    if (last is not null)
                    {
                        last.End = position;
                    }
                    break;
                }
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }
                if (items.Count == 0 ||
                    pattern.IsMatch(line))
                {
                    if (last is not null)
                    {
                        last.End = position;
                    }
                    items.Add(last = new ChapterEntity()
                    {
                        Title = line.Trim(),
                        Begin = input.Position
                    });
                    bodyLength = 0;
                    isMatchRule = true;
                    continue;
                }
                bodyLength += line.Length;
                if (bodyLength > (isMatchRule ? MaxLengthWithRule : MaxLengthWithoutRule)
                    && line.Length < 30)
                {
                    if (last is not null)
                    {
                        last.End = position;
                    }
                    items.Add(last = new ChapterEntity()
                    {
                        Title = line.Trim(),
                        Begin = input.Position
                    });
                    bodyLength = 0;
                    isMatchRule = false;
                    continue;
                }
            }
            return (new BookEntity()
            {
                Charset = encoding.WebName,
            }, [..items]);
        }


        private string? ReadLine(Stream input, Encoding encoding)
        {
            var bytes = new List<byte>();
            var isEnd = false;
            int bInt;
            while (true)
            {
                bInt = input.ReadByte();
                if (bInt == -1)
                {
                    isEnd = true;
                    break;
                }
                if (bInt == 0x0A)
                {
                    break;
                }
                if (bInt == 0x0D)
                {
                    var p = input.Position;
                    var next = input.ReadByte();
                    if (next == 0x0A)
                    {
                        break;
                    }
                    if (next == -1)
                    {
                        isEnd = true;
                        break;
                    }
                    input.Seek(p, SeekOrigin.Begin);
                    break;
                }
                bytes.Add(Convert.ToByte(bInt));
            }
            if (bytes.Count == 0)
            {
                return isEnd ? null : string.Empty;
            }
            return encoding!.GetString([.. bytes]);
        }

        public string Serialize(INovelChapter chapter)
        {
            throw new NotImplementedException();
        }

        public INovelChapter UnSerialize(string data)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            
        }

        private Regex GetRule(string content)
        {
            var max = 1;
            var maxPattern = new Regex(@"^(正文)?[\s]{0,6}第?[\s]*[0-9一二三四五六七八九十百千]{1,10}[章回|节|卷|集|幕|计]?[\s\S]{0,20}$");
            if (ChapterRuleItems is not null)
            {
                foreach (var item in ChapterRuleItems)
                {
                    var pattern = new Regex(item);
                    var match = pattern.Matches(content);
                    if (match.Count > max)
                    {
                        max = match.Count;
                        maxPattern = pattern;
                    }
                }
            }
            return maxPattern;
        }
        #region 编解码书籍文件名信息

        public string Encode(INovel novel)
        {
            if (string.IsNullOrWhiteSpace(novel.Author))
            {
                return novel.Name;
            }
            return $"{novel.Name} 作者：{novel.Author}";
        }

        public void Decode(string str, INovel novel)
        {
            // [科幻]书名 作者：z
            // 【科幻】书名 作者：z
            // 《书名》z
            // 书名 作者：z
            str = str.Replace('【', '[').Replace('】', ']')
                .Replace('《', '<').Replace('》', '>')
                .Replace("作者：", "作者:").Trim();
            if (string.IsNullOrWhiteSpace(str))
            {
                return;
            }
            var i = str.IndexOf('[');
            var category = string.Empty;
            if (i >= 0)
            {
                var j = str.IndexOf(']', i);
                category = str.Substring(i + 1, j - i - 1);
                str = str.Substring(0, i) + str.Substring(j + 1);
            }
            i = str.IndexOf('<');
            var name = string.Empty;
            var author = string.Empty;
            if (i >= 0)
            {
                var j = str.IndexOf('>', i);
                name = str.Substring(i + 1, (j - i) - 1);
                str = str.Substring(j + 1);
            } else if ((i = str.IndexOf(' ')) > 0)
            {
                name = str.Substring(0, i);
                str = str.Substring(i + 1);
            } else
            {
                name = str;
                str = string.Empty;
            }
            i = str.IndexOf("作者:");
            if (i >= 0)
            {
                author = str.Substring(i + 3).Trim();
            } else
            {
                author = str.Trim();
            }
            novel.Name = name;
            novel.Author = author;
            novel.Description = category;
            return;
        }
        #endregion

    }
}
