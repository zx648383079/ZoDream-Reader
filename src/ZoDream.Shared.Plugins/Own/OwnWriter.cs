using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ZoDream.Shared.Interfaces;

namespace ZoDream.Shared.Plugins.Own
{
    public class OwnWriter(INovelDocument data, Encoding encoding) : INovelWriter
    {
        /**
         0x5 [长度]  图标
         书名 
         评分 0-0xA
         作者 0xA
         简介 
        0x1 章节名
        0x2 正文
        0x5 [长度] 图片
        0x2 正文
        0x1 章节名
        0x2 正文
         */

        public void Write(Stream output)
        {
            if (data.Cover is not null)
            {
                WriteImage(output, 0, 0, data.Cover);
            }
            output.Write(encoding.GetBytes(data.Name));
            output.WriteByte(data.Rating);
            output.Write(encoding.GetBytes(data.Author));
            output.WriteByte(0xA);
            output.Write(encoding.GetBytes(data.Brief));
            foreach (var item in data.Items)
            {
                Write(output, item);
            }
        }

        private void Write(Stream output, INovelVolume volume)
        {
            if (!string.IsNullOrWhiteSpace(volume.Name))
            {
                output.WriteByte(0x1);
                output.Write(encoding.GetBytes(volume.Name));
            }
            foreach (var it in volume)
            {
                Write(output, it);
            }
        }

        private void Write(Stream output, INovelSection section)
        {
            WriteTitle(output, section.Title);
            WriteContent(output, section.Items);
        }

        private void WriteTitle(Stream output,string title)
        {
            output.WriteByte(0x1);
            var buffer = encoding.GetBytes(title.Trim());
            output.Write(buffer, 0, buffer.Length);
        }

        private void WriteContent(Stream output, IEnumerable<INovelBlock> items)
        {
            output.WriteByte(0x2);
            bool? lastIsText = null;
            foreach (var block in items)
            {
                if (block is INovelTextBlock text)
                {
                    if (lastIsText is not null)
                    {
                        output.WriteByte((byte)(lastIsText == true ? 0xA : 0x2));
                    }
                    lastIsText = true;
                    output.Write(encoding.GetBytes(text.Text));
                    continue;
                }
                else if (block is INovelImageBlock image)
                {
                    lastIsText = false;
                    WriteImage(output, 0, 0, image.Source);
                    continue;
                }
            }
            output.WriteByte(0x3);
        }

        private void WriteContent(Stream output, string text)
        {
            output.WriteByte(0x2);
            var buffer = encoding.GetBytes(text.Trim());
            output.Write(buffer, 0, buffer.Length);
            output.WriteByte(0x3);
        }
        private void WriteImage(Stream output, int width, int height, Stream input)
        {
            output.WriteByte(0x5);
            Write(output, (ushort)width);
            Write(output, (ushort)height);
            Write(output, (uint)input.Length);
            input.CopyTo(output);
        }

        private void Write(Stream output, ushort val)
        {
            var buffer = BitConverter.GetBytes(val);
            output.Write(buffer, 0, buffer.Length);
        }
        private void Write(Stream output,  uint val)
        {
            var buffer = BitConverter.GetBytes(val);
            output.Write(buffer, 0, buffer.Length);
        }

        public void Dispose()
        {
        }
    }
}
