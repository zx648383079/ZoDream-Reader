using System.Buffers;
using System.IO;
using System.Text;

namespace ZoDream.Shared.Storage
{
    /// <summary>   
    /// 用于取得一个文本文件的编码方式(Encoding)。   
    /// </summary>   
    public static class TxtEncoder
    {
        /// <summary>   
        /// 取得一个文本文件的编码方式。如果无法在文件头部找到有效的前导符，Encoding.Default将被返回。   
        /// </summary>   
        /// <param name="fileName">文件名。</param>   
        /// <returns></returns>   
        public static Encoding GetEncoding(string fileName)
        {
            return GetEncoding(fileName, Encoding.Default);
        }
        /// <summary>   
        /// 取得一个文本文件流的编码方式。   
        /// </summary>   
        /// <param name="stream">文本文件流。</param>   
        /// <returns></returns>   
        public static Encoding GetEncoding(Stream stream)
        {
            return GetEncoding(stream, Encoding.Default);
        }
        /// <summary>   
        /// 取得一个文本文件的编码方式。   
        /// </summary>   
        /// <param name="fileName">文件名。</param>   
        /// <param name="defaultEncoding">默认编码方式。当该方法无法从文件的头部取得有效的前导符时，将返回该编码方式。</param>   
        /// <returns></returns>   
        public static Encoding GetEncoding(string fileName, Encoding defaultEncoding)
        {
            using var fs = File.OpenRead(fileName);
            return GetEncoding(fs, defaultEncoding);
        }

        /// <summary>   
        /// 取得一个文本文件流的编码方式。   
        /// </summary>   
        /// <param name="stream">文本文件流。</param>   
        /// <param name="defaultEncoding">默认编码方式。当该方法无法从文件的头部取得有效的前导符时，将返回该编码方式。</param>   
        /// <returns></returns>   
        public static Encoding GetEncoding(Stream input, Encoding defaultEncoding)
        {
            var targetEncoding = defaultEncoding;
            if (input == null || input.Length < 2)
            {
                return targetEncoding;
            }
            var beginPos = input.Position;
            var maxLength = 1024; // 判断范围可以设大一点
            var buffer = ArrayPool<byte>.Shared.Rent(maxLength);
            try
            {
                var readLength = input.Read(buffer, 0, 3);
                //根据文件流的前4个字节判断Encoding   
                //Unicode {0xFF, 0xFE};   
                //BE-Unicode {0xFE, 0xFF};   
                //UTF8 = {0xEF, 0xBB, 0xBF};   
                if (buffer[0] == 0xFE && buffer[1] == 0xFF && readLength >= 2)//UnicodeBe   
                {
                    return Encoding.BigEndianUnicode;
                }
                else if (buffer[0] == 0xFF && buffer[1] == 0xFE && buffer[2] != 0xFF && readLength >= 3)//Unicode   
                {
                    return Encoding.Unicode;
                }
                else if (buffer[0] == 0xEF && buffer[1] == 0xBB && buffer[2] == 0xBF && readLength >= 3) //UTF8   
                {
                    return Encoding.UTF8;
                }
                readLength += input.Read(buffer, readLength, maxLength - readLength);
                var isUtf8 = false;
                for (var i = 0; i < readLength - 3; i++)
                {
                    if (buffer[i] < 0x80)
                    {
                        continue;
                    }
                    #region UTF8
                    if ((IsInRange(buffer[i], 0xC2, 0xDF) && IsInRange(buffer[i + 1], 0x80, 0xBF))
                        )
                    {
                        i++;
                        isUtf8 = true;
                        continue;
                    }
                    if ((buffer[i] == 0xE0 && IsInRange(buffer[i + 1], 0xA0, 0xBF) && IsInRange(buffer[i + 2], 0x80, 0xBF))
                        || (IsInRange(buffer[i], 0xE1, 0xEC) && IsInRange(buffer[i + 1], 0x80, 0xBF) && IsInRange(buffer[i + 2], 0x80, 0xBF))
                        || (buffer[i] == 0xED && IsInRange(buffer[i + 1], 0x80, 0x9F) && IsInRange(buffer[i + 2], 0x80, 0xBF))
                        || (IsInRange(buffer[i], 0xEE, 0xEF) && IsInRange(buffer[i + 1], 0x80, 0xBF) && IsInRange(buffer[i + 2], 0x80, 0xBF))
                        )
                    {
                        i += 2;
                        isUtf8 = true;
                        continue;
                    }
                    if ((buffer[i] == 0xF0 && IsInRange(buffer[i + 1], 0x90, 0xBF) && IsInRange(buffer[i + 2], 0x80, 0xBF) && IsInRange(buffer[i + 3], 0x80, 0xBF))
                        || (IsInRange(buffer[i], 0xF1, 0xF3) && IsInRange(buffer[i + 1], 0x80, 0xBF) && IsInRange(buffer[i + 2], 0x80, 0xBF) && IsInRange(buffer[i + 3], 0x80, 0xBF))
                        || (buffer[i] == 0xF4 && IsInRange(buffer[i + 1], 0x80, 0x9F) && IsInRange(buffer[i + 2], 0x80, 0xBF) && IsInRange(buffer[i + 3], 0x80, 0xBF))
                        )
                    {
                        i += 3;
                        isUtf8 = true;
                        continue;
                    }
                    #endregion

                    #region GBK
                    if ((IsInRange(buffer[i], 0x81, 0xA0) && IsInRange(buffer[i + 1], 0x40, 0xFE, 0x7F))
                        || (IsInRange(buffer[i], 0xA8, 0xA9) && IsInRange(buffer[i + 1], 0x40, 0xA0, 0x7F))
                        || (IsInRange(buffer[i], 0xAA, 0xFE) && IsInRange(buffer[i + 1], 0x40, 0xA0, 0x7F))
                        )
                    {
                        i++;
                        return Encoding.GetEncoding("gbk");
                    }
                    #endregion
                    #region GB2312
                    if ((IsInRange(buffer[i], 0xA1, 0xA9) && IsInRange(buffer[i + 1], 0xA1, 0xFE))
                        || (IsInRange(buffer[i], 0xB0, 0xF7) && IsInRange(buffer[i + 1], 0xA1, 0xFE)))
                    {
                        i++;
                        // gb18030 > gbk > gb2312
                        return Encoding.GetEncoding("gbk"); // Encoding.GetEncoding("gb2312");
                    }
                    #endregion
                    return defaultEncoding;
                }
                if (isUtf8)
                {
                    return Encoding.UTF8;
                }
            }
            finally
            {
                input.Seek(beginPos, SeekOrigin.Begin);
                ArrayPool<byte>.Shared.Return(buffer);
            }
            return targetEncoding;
        }

        private static bool IsInRange(byte val, byte min, byte max)
        {
            return val >= min && val <= max;
        }
        private static bool IsInRange(byte val, byte min, byte max, byte exclude)
        {
            if (val == exclude)
            {
                return false;
            }
            return val >= min && val <= max;
        }
    }
}
