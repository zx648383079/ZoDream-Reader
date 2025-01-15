namespace ZoDream.Shared.Script
{
    public enum TokenType
    {
        /// <summary>Not defined token</summary>
        None,
        /// <summary>End of file</summary>
        Eof,
        InvalidChar,
        NewLine,

        String,
        Number,
        Identifier,

        Regex,

        /// <summary>
        /// 分隔符 ,;
        /// </summary>
        Separator,
        Comma,
        Dot,
        DotDot,
        /// <summary>
        /// :
        /// </summary>
        Colon,
        /// <summary>
        /// 成对出现的括号 (){}[]
        /// </summary>
        Bracket,

        
    }
}
