namespace ZoDream.Shared.Text
{
    public interface IEncodingDictionary
    {
        /// <summary>
        /// 是否包含字符
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Contains(char value);

        /// <summary>
        /// 编码字符
        /// </summary>
        /// <param name="value"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public bool TrySerialize(char value, out char result);
        /// <summary>
        /// 解码字符
        /// </summary>
        /// <param name="value"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public bool TryDeserialize(char value, out char result);
        /// <summary>
        /// 根据上一个字符解码字符，
        /// </summary>
        /// <param name="value"></param>
        /// <param name="lastIsAscii">前一个字符是否是字母</param>
        /// <param name="result"></param>
        /// <returns></returns>
        public bool TryDeserialize(char value, bool lastIsAscii, out char result);
    }
}