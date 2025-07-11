using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using ZoDream.Shared.Interfaces;
using ZoDream.Shared.IO;
using ZoDream.Shared.Models;
using ZoDream.Shared.Tokenizers;

namespace ZoDream.Shared.Plugins.Umd
{
    public class UmdReader(Stream input) : INovelReader
    {
        internal const uint Magic = 0xde9a9b89;
        private readonly Encoding _encoding = Encoding.Unicode;

        public INovelBasic ReadBasic()
        {
            var data = new NovelBasic();
            input.Seek(0, SeekOrigin.Begin);
            var reader = new BinaryReader(input);
            if (reader.ReadUInt32() != 0xde9a9b89)
            {
                // TODO 文件错误
                return data;
            }
            reader.BaseStream.Seek(5, SeekOrigin.Current);
            var fileType = input.ReadByte(); // 1 text 2 图片
            reader.BaseStream.Seek(2, SeekOrigin.Current);
            ReadNovel(reader, data);
            return data;
        }

        public INovelDocument Read()
        {
            input.Seek(0, SeekOrigin.Begin);
            var reader = new BinaryReader(input);
            if (reader.ReadUInt32() != 0xde9a9b89)
            {
                // TODO 文件错误
                return null;
            }
            reader.BaseStream.Seek(5, SeekOrigin.Current);
            var fileType = input.ReadByte(); // 1 text 2 图片
            reader.BaseStream.Seek(2, SeekOrigin.Current);
            var novel = new RichDocument();
            ReadNovel(reader, novel);
            var type = ReadHeaderType(reader);
            if (type != 0x83)
            {
                // ChapterOffset

            }
            reader.BaseStream.Seek(11, SeekOrigin.Current);
            var chapterCount = (reader.ReadInt32() - 9) / 4;
            var offsetItems = new int[chapterCount];
            for (int i = 0; i < chapterCount; i++)
            {
                // 正文的偏移
                offsetItems[i] = reader.ReadInt32();
            }
            type = ReadHeaderType(reader);
            if (type != 0x84)
            {
                // ChapterTitle

            }
            reader.BaseStream.Seek(11, SeekOrigin.Current);
            var len = reader.ReadInt32() - 9;
            var titleItems = new string[chapterCount];
            for (int i = 0; i < chapterCount; i++)
            {
                //一个字节，长度
                //长度的字节，标题
                var l = reader.ReadByte();
                titleItems[i] = _encoding.GetString(reader.ReadBytes(l));
            }
            var content = ReadContent(reader);
            var volume = new NovelVolume(string.Empty);
            for (int i = 0; i < chapterCount; i++)
            {
                string text;
                if (i < chapterCount - 1)
                {
                    text = content[offsetItems[i]..offsetItems[i + 1]];
                } else
                {
                    text = content[offsetItems[i]..];
                }
                volume.Add(new NovelSection(titleItems[i], text.Split('\n').Where(i => !string.IsNullOrWhiteSpace(i))
                    .Select(i => new NovelTextBlock(i))));
            }
            novel.Cover = ReadCover(reader);
            return novel;
        }


        private void ReadNovel(BinaryReader reader, NovelBasic data)
        {
            while (true)
            {
                //两个字节，表示类别
                var type = ReadHeaderType(reader);
                if (type is 0x0 or 0xA or >= 0xC)
                {
                    reader.BaseStream.Seek(-3, SeekOrigin.Current);
                    break;
                }
                reader.ReadByte();
                var len = reader.ReadByte() - 5;
                var buffer = reader.ReadBytes(len);
                switch (type)
                {
                    case 0x01:
                        return;
                    case 0x02:
                        data.Name = _encoding.GetString(buffer);
                        break;
                    case 0x03:
                        data.Author = _encoding.GetString(buffer);
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
                        return;
                }
            }
            return;
        }

        private int ReadHeaderType(BinaryReader reader)
        {
            var start = reader.ReadByte();
            if (start != '#')
            {
                reader.BaseStream.Seek(-1, SeekOrigin.Current);
                return 0;
            }
            //两个字节，表示类别
            return reader.ReadInt16();
        }

        private Stream? ReadCover(BinaryReader reader)
        {
            var flag = reader.ReadByte();
            if (flag != 0x23)
            {
                throw new Exception("封面处读取错误");
            }
            int type = reader.ReadByte();
            if (type != 0x82)
            {
                if (type == 0x87)
                {
                    //页面偏移
                    reader.BaseStream.Seek(-2, SeekOrigin.Current);
                    return null;
                }
                throw new Exception("封面处读取错误");
            }
            //13个无用字节
            reader.BaseStream.Seek(13, SeekOrigin.Current);
            //4个字节的数据和封面字节数有关，内容是封面字节数+9，然后再写入封面字节数据，不需要压缩
            var len = reader.ReadInt32() - 9;
            var res = new PartialStream(reader.BaseStream, len);
            reader.BaseStream.Seek(len, SeekOrigin.Current);
            return res;
        }

        private string ReadContent(BinaryReader reader)
        {
            using var outputStream = new MemoryStream();
            var buffer = new byte[4];
            do
            {
                //一个0x24
                int flag = reader.ReadByte();
                if (flag == 0x23)
                {
                    //在写完每个数据块后，可以选择做如下两件事的一件或两件（建议用随机数来决定）：
                    var t2 = reader.ReadByte();
                    if (t2 == 0xf1)
                    {
                        //1．  写入1个字节’#’，2个字节的0xf1,2个字节的0x1500，16个字节的0x0
                        reader.BaseStream.Seek(19, SeekOrigin.Current);
                        continue;
                    }
                    else if (t2 == 0x0a)
                    {
                        //2．  写入1个字节’#’，2个字节的0x0a,2个字节的0x0900，4个字节的随机数
                        reader.BaseStream.Seek(7, SeekOrigin.Current);
                        continue;
                    }
                    else if (t2 == 0x81)
                    {
                        //在所有正文数据块写入完毕后，写入1个字节’#’,2个字节数据类型0x81，表示正文写入完毕，
                        //2个字节0x0901，4个字节随机数，1个字节0x24，4个字节随机数（一致），
                        //接下来是4个字节，内容是数据块的数目*4+9，
                        //然后，还记得每个数据块写入前都生成了4个字节的随机数吗，
                        //从最后一个开始，倒序写如这些随机数，每个4个字节，结束正文的写入。
                        reader.BaseStream.Seek(12, SeekOrigin.Current);
                        var l = reader.ReadInt32() - 9;
                        reader.BaseStream.Seek(l, SeekOrigin.Current);
                        break;
                    }
                }
                //四个随机数字节
                reader.BaseStream.Seek(4, SeekOrigin.Current);
                //四个字节，表示长度
                var len = reader.ReadInt32() - 9;
                //读取
                reader.BaseStream.CopyTo(outputStream, len);
            } while (true);
            using var decompressor = new DeflateStream(outputStream, CompressionMode.Decompress);
            using var r = new StreamReader(decompressor, _encoding);
            return r.ReadToEnd().Replace("\u2029", "\n");
        }

        public void Dispose()
        {
            input.Dispose();
        }
    }
}
