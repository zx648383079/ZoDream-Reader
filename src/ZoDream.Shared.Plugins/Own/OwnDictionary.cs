using System;

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
                '；' => ';',
                '＊' => '*',
                '：' => ':',
                '“' or '”' => '"',
                '’' or '‘' or '「' or '」' or '『' or '』' => '\'',
                '《' => '<',
                '》' => '<',
                '【' => '[',
                '】' => ']',
                '°' => '\'',
                '。' or '．' => '.',
                '～' => '~',
                '一' => '1',
                '二' => '2',
                '三' => '3',
                '四' => '4',
                '五' => '5',
                '六' => '6',
                '七' => '7',
                '八' => '8',
                '九' => '9',
                //'、' => '|',
                _ => value,
            };
        }

        public char Deserialize(char value)
        {
            return value;
        }

        public bool IsExclude(char value)
        {
            return value < 128
                || value is '　' or '？' or '！' or '，' or '）'
                or '（' or '＊' or '：' or '“'
                or '’' or '”' or '‘' or '《'
                or '》' or '【' or '】' or '。' or '．' or '°'
                // 以下是转化
                or '、' or '﹑' or '…' or '┅' or '·' or '―' or '─' or '－' or '¨' or '※';
        }
    }
}
