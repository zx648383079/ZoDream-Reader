using System.Text;

namespace ZoDream.Shared.Text
{
    public class OwnEncoding : Encoding
    {
        /// <summary>
        /// 保留 ascii 前 10 个字符 0x20 - 0x7E
        /// </summary>
        private readonly byte _system = 0xA;

        

        public override int GetByteCount(char[] chars, int index, int count)
        {
            throw new System.NotImplementedException();
        }

        public override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex)
        {
            throw new System.NotImplementedException();
        }

        public override int GetCharCount(byte[] bytes, int index, int count)
        {
            throw new System.NotImplementedException();
        }

        public override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex)
        {
            throw new System.NotImplementedException();
        }

        public override int GetMaxByteCount(int charCount)
        {
            throw new System.NotImplementedException();
        }

        public override int GetMaxCharCount(int byteCount)
        {
            throw new System.NotImplementedException();
        }
    }
}
