using System;
using System.IO;
using System.Text;

namespace ZoDream.Shared.Plugins.Own
{
    public class OwnWriter(Stream output, Encoding encoding)
    {
        /**
         0x5 [长度]  图标
         书名 0xA
         作者 0xA
         简介 
        0x1 章节名
        0x2 正文
        0x5 [长度] 图片
        0x2 正文
        0x1 章节名
        0x2 正文
         */

        public void WriteTitle(string title)
        {
            output.WriteByte(01);
            var buffer = encoding.GetBytes(title.Trim());
            output.Write(buffer, 0, buffer.Length);
        }

        public void WriteContent(string text)
        {
            output.WriteByte(02);
            var buffer = encoding.GetBytes(text.Trim());
            output.Write(buffer, 0, buffer.Length);
            output.WriteByte(03);
        }
        public void WriteImage(int width, int height, Stream input)
        {
            output.WriteByte(05);
            Write((ushort)width);
            Write((ushort)height);
            Write((uint)input.Length);
            input.CopyTo(output);
        }

        private void Write(ushort val)
        {
            var buffer = BitConverter.GetBytes(val);
            output.Write(buffer, 0, buffer.Length);
        }
        private void Write(uint val)
        {
            var buffer = BitConverter.GetBytes(val);
            output.Write(buffer, 0, buffer.Length);
        }
    }
}
