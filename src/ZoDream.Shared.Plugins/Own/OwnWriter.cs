using System;
using System.IO;
using System.Text;

namespace ZoDream.Shared.Plugins.Own
{
    public class OwnWriter(Stream output, Encoding encoding)
    {
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
