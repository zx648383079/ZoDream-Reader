using System;
using System.Collections.Generic;
using System.Text;

namespace ZoDream.Shared.Script
{
    public enum TokenType
    {
        /// <summary>Not defined token</summary>
        None,
        /// <summary>End of file</summary>
        Eof,

        String,
        Number,
        Identifier,
        /// <summary>
        /// 分隔符 ,;
        /// </summary>
        Separator,
        Dot,
        /// <summary>
        /// 成对出现的括号 (){}[]
        /// </summary>
        Bracket,
    }
}
