using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZoDream.Shared.Interfaces;
using ZoDream.Shared.Local;

namespace ZoDream.Shared.Renders
{
    public class StreamIterator : ICharIterator
    {

        public StreamIterator(string fileName)
        {
            _fileName = fileName;
            _reader = new FileStream(fileName, FileMode.Open);
            _encoding = TxtEncoder.GetEncoding(_reader);
        }

        private string _fileName;
        private Stream _reader;
        private Encoding _encoding;

        public long Position { 
            get => _reader.Position; 
            set
            {
                _reader.Seek(value, SeekOrigin.Begin);
            }
        }

        public string? ReadLine()
        {
            var bytes = new List<byte>();
            var isEnd = false;
            int bInt;
            while (true)
            {
                try
                {
                    bInt = _reader.ReadByte();
                }
                catch (IndexOutOfRangeException)
                {
                    isEnd = true;
                    break;
                }
                if (bInt == -1)
                {
                    isEnd = true;
                    break;
                }
                if (bInt == 0x0A)
                {
                    break;
                }
                if (bInt == 0x0D)
                {
                    var p = Position;
                    if (_reader.ReadByte() == 0x0A)
                    {
                        break;
                    }
                    Position = p;
                    break;
                }
                bytes.Add(Convert.ToByte(bInt));
            }
            if (bytes.Count == 0)
            {
                return isEnd ? null : string.Empty;
            }
            return _encoding.GetString(bytes.ToArray());
        }

        public void Dispose()
        {
            _reader?.Close();
        }

        public Task<string?> ReadLineAsync()
        {
            return Task.Factory.StartNew(() =>
            {
                return ReadLine();
            });
        }

        public Task<string?> ReadLineAsync(long position)
        {
            Position = position;
            return ReadLineAsync();
        }
    }
}
