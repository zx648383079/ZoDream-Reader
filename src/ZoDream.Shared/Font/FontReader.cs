using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ZoDream.Shared.Font
{
    public class FontReader: IDisposable
    {

        public FontReader(Stream fs)
        {
            BaseStream = fs;
        }

        public Stream BaseStream;

        public void Dispose()
        {
            BaseStream.Dispose();
        }
    }
}
