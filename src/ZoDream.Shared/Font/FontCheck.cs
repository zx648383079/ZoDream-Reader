using System;
using System.Collections.Generic;
using System.Text;

namespace ZoDream.Shared.Font
{
    public static class FontCheck
    {
        public static void ValidateSfntVersion(uint sfntVersion)
        {
            if ((sfntVersion != SFNTVersion.DEFAULT) &&
                (sfntVersion != SFNTVersion.OTTO) &&
                (sfntVersion != SFNTVersion.TRUE) &&
                (sfntVersion != SFNTVersion.TYP1))
            {
                throw new ArgumentException(string.Format("Unknow sfntVersion {0:X0}", sfntVersion));
            }
        }

        public static void ValidateTagByteContent(byte[] tagByte)
        {
            if (tagByte.Length != 4)
            {
                throw new ArgumentException("INVALID_TAG_SIZE");
            }

            for (int i = 0; i < tagByte.Length; i++)
            {
                if (!IsInUnicodeBasicLatinCharRange(tagByte[i]))
                {
                    throw new ArgumentOutOfRangeException("INVALID_CHAR_RANGE");
                }
            }
        }

        public static bool IsInUnicodeBasicLatinCharRange(byte b)
        {
            //range of values of Unicode Basic Latin characters in UTF-8 encoding
            return (b >= 0x20) && (b <= 0x7E);
        }

        public static string ConvertToTagName(byte[] tagByte)
        {
            ValidateTagByteContent(tagByte);
            return Encoding.UTF8.GetString(tagByte);
        }
    }
}
