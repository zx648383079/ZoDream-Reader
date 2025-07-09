using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZoDream.Shared.Plugins.Rtf
{
    public class RtfToken
    {
    }

    public enum RtfTokenType
    {
        None,
        Keyword,
        ExtKeyword,
        Control,
        Text,
        Eof,
        GroupStart,
        GroupEnd
    }
}
