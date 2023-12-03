using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ZoDream.Shared.Interfaces;
using ZoDream.Shared.Interfaces.Entities;
using ZoDream.Shared.Repositories.Entities;
using ZoDream.Shared.Storage;

namespace ZoDream.Shared.Plugins.Txt
{
    public class TxtReader : INovelReader
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

        public async Task<List<INovelChapter>> GetChaptersAsync(string fileName)
        {
            var items = new List<INovelChapter>();
            var fs = new FileStream(fileName, FileMode.Open);
            var encoding = TxtEncoder.GetEncoding(fs);
            var buffer = new byte[BufferSize];
            await fs.ReadAsync(buffer, 0, buffer.Length);
            var pattern = GetRule(encoding.GetString(buffer));
            fs.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(fs, encoding);
            var isMatchRule = true;
            var bodyLength = 0L;
            ChapterEntity? last = null;
            while (true)
            {
                var position = reader.BaseStream.Position;
                var line = reader.ReadLine();
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
                        Begin = reader.BaseStream.Position
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
                        Begin = reader.BaseStream.Position
                    });
                    bodyLength = 0;
                    isMatchRule = false;
                    continue;
                }
            }
            return items;
        }

        public async Task<string> GetChapter(string fileName,INovelChapter chapter)
        {
            var fs = new FileStream(fileName, FileMode.Open);
            var encoding = TxtEncoder.GetEncoding(fs);
            fs.Seek(chapter.Begin, SeekOrigin.Begin);
            var buffer = new byte[chapter.End - chapter.Begin];
            await fs.ReadAsync(buffer, 0, buffer.Length);
            return encoding.GetString(buffer);
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


    }
}
