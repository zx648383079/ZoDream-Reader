using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Media;

namespace ZoDream.Helper
{
    public class StringHelper
    {
        public static string GetStr(string s, int l, string endStr)
        {
            var temp = s.Substring(0, (s.Length < l) ? s.Length : l);

            if (Regex.Replace(temp, "[\u4e00-\u9fa5]", "zz", RegexOptions.IgnoreCase).Length <= l)
            {
                return temp;
            }
            for (var i = temp.Length; i >= 0; i--)
            {
                temp = temp.Substring(0, i);
                if (Regex.Replace(temp, "[\u4e00-\u9fa5]", "zz", RegexOptions.IgnoreCase).Length <= l - endStr.Length)
                {
                    return temp + endStr;
                }
            }
            return endStr;
        }

        public static string GetStr2(string s, int l, string endStr)
        {
            var temp = s.Substring(0, (s.Length < l + 1) ? s.Length : l + 1);
            var encodedBytes = Encoding.ASCII.GetBytes(temp);

            var outputStr = "";
            var count = 0;

            for (var i = 0; i < temp.Length; i++)
            {
                if (encodedBytes[i] == 63)
                    count += 2;
                else
                    count += 1;

                if (count <= l - endStr.Length)
                    outputStr += temp.Substring(i, 1);
                else if (count > l)
                    break;
            }

            if (count <= l)
            {
                outputStr = temp;
                endStr = "";
            }

            outputStr += endStr;

            return outputStr;
        }


        public static Color ToColor(string color)
        {
            var returnColor = Colors.Black;
            if (string.IsNullOrEmpty(color)) return returnColor;
            if (color[0] == '#')
            {
                // #fff;
                var convertFromString = ColorConverter.ConvertFromString(color);
                if (convertFromString != null)
                {
                    return (Color)convertFromString;
                }
                return returnColor;
            }
            var ms = Regex.Matches(color, @"\d+");
            switch (ms.Count)
            {
                case 3:
                    //255,255,255
                    returnColor = Color.FromRgb(Convert.ToByte(ms[0].Value), Convert.ToByte(ms[1].Value),
                        Convert.ToByte(ms[2].Value));
                    break;
                case 4:
                    //0,0,0,0.1
                    returnColor = Color.FromArgb(Convert.ToByte(ms[0].Value), Convert.ToByte(ms[1].Value),
                        Convert.ToByte(ms[2].Value), Convert.ToByte(ms[3].Value));
                    break;
            }
            return returnColor;
        }

        /// <summary>
        /// 全角转半角
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToDbc(string input)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));
            var c = input.ToCharArray();
            for (var i = 0; i < c.Length; i++)
            {
                if (c[i] == 12288)
                {
                    c[i] = (char)32;
                    continue;
                }
                if (c[i] > 65280 && c[i] < 65375)
                    c[i] = (char)(c[i] - 65248);
            }
            return new string(c);
        }

        /// <summary>
        /// 字节转换成16进制字符串
        /// </summary>
        /// <param name="buffers"></param>
        /// <returns></returns>
        public static string ByteTo16(byte[] buffers)
        {
            var sb = new StringBuilder();
            foreach (var item in buffers)
            {
                sb.Append(item.ToString("X2"));
            }
            return sb.ToString();
            // BitConverter.ToString(buffers).Replace("-", "")
            /*
             string sHash = "", sTemp = "";
            for (int counter = 0; counter < args.Length; counter++)
            {
                long i = args[counter] / 16;
                if (i > 9)
                {
                    sTemp = ((char)(i - 10 + 0x41)).ToString();
                }
                else
                {
                    sTemp = ((char)(i + 0x30)).ToString();
                }
                i = args[counter] % 16;
                if (i > 9)
                {
                    sTemp += ((char)(i - 10 + 0x41)).ToString();
                }
                else
                {
                    sTemp += ((char)(i + 0x30)).ToString();
                }
                sHash += sTemp;
            }
            sHash;
             */
        }

        /// <summary>
        /// 计算32位MD5码
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string MD5_32(byte[] args)
        {
            try
            {
                byte[] buffers;
                using (var md5 = new MD5CryptoServiceProvider())
                {
                    buffers = md5.ComputeHash(args);
                }
                return ByteTo16(buffers);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static string MD5_32(string arg)
        {
            return MD5_32(Encoding.UTF8.GetBytes(arg));
        }

        /// <summary>
        /// 计算16位MD5码
        /// </summary>
        /// <param name="arg">字符串</param>
        /// <returns></returns>
        public static string MD5_16(string arg)
        {
            try
            {
                return MD5_32(arg).Substring(8, 16);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 计算16位MD5码
        /// </summary>
        /// <param name="args">字符串</param>
        /// <returns></returns>
        public static string MD5_16(byte[] args)
        {
            try
            {
                return MD5_32(args).Substring(8, 16);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static string SHA_1(byte[] args)
        {
            try
            {
                byte[] buffers;
                using (var sha1 = new SHA1CryptoServiceProvider())
                {
                    buffers = sha1.ComputeHash(args);
                }
                return ByteTo16(buffers);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static string SHA_1(string arg)
        {
            return SHA_1(Encoding.UTF8.GetBytes(arg));
        }

        public static string SHA_256(byte[] args)
        {
            try
            {
                byte[] buffers;
                using (var sha256 = new SHA256CryptoServiceProvider())
                {
                    buffers = sha256.ComputeHash(args);
                }
                return ByteTo16(buffers);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static string SHA_256(string arg)
        {
            return SHA_256(Encoding.UTF8.GetBytes(arg));
        }

        public static string SHA_384(byte[] args)
        {
            try
            {
                byte[] buffers;
                using (var sha = new SHA384CryptoServiceProvider())
                {
                    buffers = sha.ComputeHash(args);
                }
                return ByteTo16(buffers);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static string SHA_384(string arg)
        {
            return SHA_384(Encoding.UTF8.GetBytes(arg));
        }

        public static string SHA_512(byte[] args)
        {
            try
            {
                byte[] buffers;
                using (var sha = new SHA512CryptoServiceProvider())
                {
                    buffers = sha.ComputeHash(args);
                }
                return ByteTo16(buffers);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static string SHA_512(string arg)
        {
            return SHA_512(Encoding.UTF8.GetBytes(arg));
        }
    }
}
