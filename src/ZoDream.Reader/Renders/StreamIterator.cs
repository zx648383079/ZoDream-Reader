using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZoDream.Shared.Interfaces;
using ZoDream.Shared.Local;

namespace ZoDream.Reader.Renders
{
    public class StreamIterator : ICharIterator
    {

        public StreamIterator(string fileName)
        {
            _fileName = fileName;
            reader = Open.Reader(fileName);
        }

        private string _fileName;
        private StreamReader reader;

        public long Position { 
            get => reader.BaseStream.Position; 
            set => reader.BaseStream.Seek(value, SeekOrigin.Begin); 
        }

        public string? ReadLine()
        {
            return reader.ReadLine();
        }

        public void Dispose()
        {
            reader?.Close();
        }
    }
}
