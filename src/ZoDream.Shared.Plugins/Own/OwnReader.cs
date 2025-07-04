using System;
using System.Diagnostics;
using System.IO;
using ZoDream.Shared.Interfaces;
using ZoDream.Shared.IO;
using ZoDream.Shared.Text;
using ZoDream.Shared.Tokenizers;

namespace ZoDream.Shared.Plugins.Own
{
    public class OwnReader(Stream input, OwnEncoding encoding) : INovelReader
    {
        private readonly byte[] _buffer = new byte[1024 * 5];
        private int _last = -1;
        public INovelDocument Read()
        {
            input.Seek(0, SeekOrigin.Begin);
            var code = input.ReadByte();
            var res = new RichDocument();
            if (code == 0x5)
            {
                res.Cover = ReadImage();
            } else
            {
                input.Seek(-1, SeekOrigin.Current);
            }
            res.Name = ReadString();
            res.Author = ReadString();
            res.Brief = ReadString();
            while (_last == 0x1)
            {
                var title = ReadString();
                if (_last == 0x1)
                {
                    res.Items.Add(new NovelVolume(title));
                    continue;
                }
                Debug.Assert(_last == 0x2);
                var section = new NovelSection(title);
                res.Add(section);
                while (true)
                {
                    var line = ReadString();
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        section.Items.Add(new NovelTextBlock(line));
                    }
                    if (_last == 0x3)
                    {
                        _last = input.ReadByte();
                        break;
                    }
                    if (_last == 0x1)
                    {
                        break;
                    }
                    if (_last == 0x5)
                    {
                        section.Items.Add(new NovelImageBlock(ReadImage()));
                    }
                }
            }

            return res;
        }

        private string ReadString()
        {
            var i = 0;
            while (true)
            {
                var code = input.ReadByte();
                if (code < 0)
                {
                    _last = code;
                    break;
                }
                if (code <= 0xA)
                {
                    _last = code;
                    break;
                }
                _buffer[i++] = (byte)code;
            }
            return encoding.GetString(_buffer, 0, i);
        }

        private Stream ReadImage()
        {
            input.Seek(4, SeekOrigin.Current);
            input.ReadExactly(_buffer, 0, 4);
            var length = BitConverter.ToUInt32(_buffer, 0);
            var res = new PartialStream(input, length);
            input.Seek(length, SeekOrigin.Current);
            return res;
        }

        public void Dispose()
        {
        }
    }
}
