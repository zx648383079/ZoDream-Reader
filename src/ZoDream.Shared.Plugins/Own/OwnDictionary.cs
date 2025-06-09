namespace ZoDream.Shared.Plugins.Own
{
    public class OwnDictionary
    {
        /// <summary>
        /// 转化一些特殊符号
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public char Serialize(char value)
        {
            return value switch
            {
                '　' => ' ',
                '？' => '?',
                '！' => '!',
                '，' => ',',
                '）' => ')',
                '（' => '(',
                '＊' => '*',
                '：' => ':',
                '“' or '”' => '"',
                '’' or '‘' => '\'',
                '《' => '<',
                '》' => '<',
                '【' => '[',
                '】' => ']',
                '。' => '.',
                //'、' => '|',
                _ => value,
            };
        }

        public char Deserialize(char value)
        {
            return value;
        }
    }
}
