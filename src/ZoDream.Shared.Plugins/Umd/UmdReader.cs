using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using ZoDream.Shared.Interfaces;
using ZoDream.Shared.Interfaces.Entities;
using ZoDream.Shared.Repositories.Entities;

namespace ZoDream.Shared.Plugins.Umd
{
    public class UmdReader : INovelReader
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task<string> GetChapter(string fileName, INovelChapter chapter)
        {
            var fs = File.OpenRead(fileName);
            fs.Seek(Convert.ToInt64(chapter.Url), SeekOrigin.Begin);
            var content = ReadContent(fs);
            return Task.FromResult(content.Substring((int)chapter.Begin, (int)(chapter.End - chapter.Begin)));
        }

        public async Task<List<INovelChapter>> GetChaptersAsync(string fileName)
        {
            var fs = File.OpenRead(fileName);
            var buffer = new byte[4];
            await fs.ReadAsync(buffer, 0, buffer.Length);
            if (BitConverter.ToUInt32(buffer, 0) != 0xde9a9b89)
            {
                // TODO 文件错误
            }
            fs.Seek(5, SeekOrigin.Current);
            var fileType = fs.ReadByte(); // 1 text 2 图片
            fs.Seek(2, SeekOrigin.Current);
            var novel = ReadNovel(fs);
            var type = ReadHeaderType(fs);
            if (type != 0x83)
            {
                // ChapterOffset

            }
            fs.Seek(11, SeekOrigin.Current);
            buffer = new byte[4];
            fs.Read(buffer, 0, buffer.Length);
            var chapterCount = (BitConverter.ToInt32(buffer, 0) - 9) / 4;
            var items = new List<INovelChapter>(chapterCount);
            for (int i = 0; i < chapterCount; i++)
            {
                fs.Read(buffer, 0, 4);
                // 正文的偏移
                var offset = BitConverter.ToInt32(buffer, 0);
                items.Add(new ChapterEntity()
                {
                    Begin = offset
                });
            }
            type = ReadHeaderType(fs);
            if (type != 0x84)
            {
                // ChapterTitle

            }
            fs.Seek(11, SeekOrigin.Current);
            fs.Read(buffer, 0, 4);
            var len = BitConverter.ToInt32(buffer, 0) - 9;
            for (int i = 0; i < chapterCount; i++)
            {
                //一个字节，长度
                //长度的字节，标题
                var l = fs.ReadByte();
                buffer = new byte[l];
                fs.Read(buffer, 0, l);
                items[i].Title = Encoding.Unicode.GetString(buffer, 0, l);
                if (i > 0)
                {
                    items[i - 1].End = items[i].Begin;
                }
            }
            var entry = fs.Position.ToString();
            foreach (var item in items)
            {
                item.Url = entry;
            }
            items[items.Count - 1].End = ReadContent(fs).Length;
            novel.Cover = ReadCover(fs);
            return items;
        }

        public string Serialize(INovelChapter chapter)
        {
            throw new NotImplementedException();
        }

        public INovelChapter UnSerialize(string data)
        {
            throw new NotImplementedException();
        }

        private INovel ReadNovel(Stream stream)
        {
            var novel = new BookEntity();
            var properties = new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0b };
            while (true)
            {
                //两个字节，表示类别
                var type = ReadHeaderType(stream);
                if (!properties.Any(i => i == type))
                {
                    stream.Seek(-3, SeekOrigin.Current);
                    break;
                }
                stream.ReadByte();
                var len = stream.ReadByte() - 5;
                var buffer = new byte[len];
                stream.Read(buffer, 0, len);
                switch (type)
                {
                    case 0x01:
                        return novel;
                    case 0x02:
                        novel.Name = Encoding.Unicode.GetString(buffer);
                        break;
                    case 0x03:
                        novel.Author = Encoding.Unicode.GetString(buffer);
                        break;
                    case 0x04:
                    case 0x05:
                    case 0x06:
                    case 0x07:
                    case 0x08:
                    case 0x09:
                    case 0x0b: // contentLength
                        break;
                    default:
                        return novel;
                }
            }
            return novel;
        }

        private int ReadHeaderType(Stream stream)
        {
            var start = stream.ReadByte();
            if (start != '#')
            {
                stream.Seek(-1, SeekOrigin.Current);
                return 0;
            }
            //两个字节，表示类别
            var buffer = new byte[2];
            stream.Read(buffer, 0, buffer.Length);
            return BitConverter.ToInt16(buffer, 0);
        }

        private string ReadCover(Stream stream)
        {
            var flag = stream.ReadByte();
            if (flag != 0x23)
            {
                throw new Exception("封面处读取错误");
            }
            int type = stream.ReadByte();
            if (type != 0x82)
            {
                if (type == 0x87)
                {
                    //页面偏移
                    stream.Seek(-2, SeekOrigin.Current);
                    return string.Empty;
                }
                throw new Exception("封面处读取错误");
            }
            //13个无用字节
            stream.Seek(13, SeekOrigin.Current);
            //4个字节的数据和封面字节数有关，内容是封面字节数+9，然后再写入封面字节数据，不需要压缩
            var buffer = new byte[4];
            stream.Read(buffer, 0, 4);
            var len = BitConverter.ToInt32(buffer, 0) - 9;
            buffer = new byte[len];
            stream.Read(buffer, 0, len);
            return Convert.ToBase64String(buffer);
        }

        private string ReadContent(Stream stream)
        {
            using var outputStream = new MemoryStream();
            var buffer = new byte[4];
            do
            {
                //一个0x24
                int flag = stream.ReadByte();
                if (flag == 0x23)
                {
                    //在写完每个数据块后，可以选择做如下两件事的一件或两件（建议用随机数来决定）：
                    var t2 = stream.ReadByte();
                    if (t2 == 0xf1)
                    {
                        //1．  写入1个字节’#’，2个字节的0xf1,2个字节的0x1500，16个字节的0x0
                        stream.Seek(19, SeekOrigin.Current);
                        continue;
                    }
                    else if (t2 == 0x0a)
                    {
                        //2．  写入1个字节’#’，2个字节的0x0a,2个字节的0x0900，4个字节的随机数
                        stream.Seek(7, SeekOrigin.Current);
                        continue;
                    }
                    else if (t2 == 0x81)
                    {
                        //在所有正文数据块写入完毕后，写入1个字节’#’,2个字节数据类型0x81，表示正文写入完毕，
                        //2个字节0x0901，4个字节随机数，1个字节0x24，4个字节随机数（一致），
                        //接下来是4个字节，内容是数据块的数目*4+9，
                        //然后，还记得每个数据块写入前都生成了4个字节的随机数吗，
                        //从最后一个开始，倒序写如这些随机数，每个4个字节，结束正文的写入。
                        stream.Seek(12, SeekOrigin.Current);
                        stream.Read(buffer, 0, 4);
                        var l = BitConverter.ToInt32(buffer, 0) - 9;
                        stream.Seek(l, SeekOrigin.Current);
                        break;
                    }
                }
                //四个随机数字节
                stream.Seek(4, SeekOrigin.Current);
                //四个字节，表示长度
                stream.Read(buffer, 0, 4);
                var len = BitConverter.ToInt32(buffer, 0) - 9;
                //读取
                stream.CopyTo(outputStream, len);
                //using var ms = new MemoryStream();
                //stream.CopyTo(ms, len);
                //using var decompressor = new DeflateStream(ms, CompressionMode.Decompress);
                //decompressor.CopyTo(stream);
            } while (true);
            using var decompressor = new DeflateStream(outputStream, CompressionMode.Decompress);
            using var reader = new StreamReader(decompressor, Encoding.Unicode);
            return reader.ReadToEnd().Replace("\u2029", "\n");
        }
    }
}
