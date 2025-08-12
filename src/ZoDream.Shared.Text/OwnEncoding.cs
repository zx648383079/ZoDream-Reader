using System.Text;

namespace ZoDream.Shared.Text
{
    public class OwnEncoding(IEncodingDictionary dict) : Encoding
    {
        /// <summary>
        /// 保留 ascii 前 10 个字符 0x20 - 0x7E
        /// </summary>
        const byte SystemMax = 0xA;
        const byte CodeBegin = 0xB;
        const byte CodeCount = 0xFF - CodeBegin;
        const byte DoubleSplitTag = 210;



        public override int GetByteCount(char[] chars, int index, int count)
        {
            var res = 0;
            for (int i = 0; i < count; i++)
            {
                res += dict.TrySerialize(chars[index + i], out var code) && code > DoubleSplitTag ? 2 : 1;
            }
            return res;
        }

        public override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex)
        {
            var j = 0;
            for (int i = 0; i < charCount; i++)
            {
                if (!dict.TrySerialize(chars[charIndex + i], out var code))
                {
                    // bytes[byteIndex + j++] = 0x7F;
                    continue;
                }
                if (code <= DoubleSplitTag)
                {
                    bytes[byteIndex + j++] = (byte)code;
                    continue;
                }
                bytes[byteIndex + j++] = (byte)(code / CodeCount + DoubleSplitTag);
                bytes[byteIndex + j++] = (byte)(code % CodeCount + CodeBegin);
            }
            return j;
        }

        public override int GetCharCount(byte[] bytes, int index, int count)
        {
            var res = 0;
            for (int i = 0; i < count; i++)
            {
                if (bytes[index + i] >= DoubleSplitTag)
                {
                    i++;
                }
                res ++;
            }
            return res;
        }

        public override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex)
        {
            var j = 0;
            int n;
            var lastIsAscii = false;
            for (int i = 0; i < byteCount; i++)
            {
                n = byteIndex + i;
                var code = (char)bytes[n];
                if (bytes[n] >= DoubleSplitTag)
                {
                    i++;
                    n++;
                    var next = bytes.Length > n ? (bytes[n] - CodeBegin) : 0;
                    code = (char)((code - DoubleSplitTag) * CodeCount + next);
                }
                var current = dict.TryDeserialize(code, lastIsAscii, out var res) ? res : code;
                chars[charIndex + j++] = current;
                lastIsAscii = current < 0x7F;
            }
            return j;
        }

        public override int GetMaxByteCount(int charCount)
        {
            return charCount * 2;
        }

        public override int GetMaxCharCount(int byteCount)
        {
            return byteCount;
        }
    }
}
