using System;
using System.Collections.Generic;
using System.Text;

namespace ZoDream.Shared.Interfaces.Tokenizers
{
    public interface INovelPage: IList<INovelPageLine>
    {

        public int X { get; }
        public int Y { get; }


    }
}
