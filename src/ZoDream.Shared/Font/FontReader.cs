using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ZoDream.Shared.Font
{
    public class FontReader: IDisposable
    {

        public FontReader(Stream fs)
        {
            BaseStream = fs;
        }

        public Stream BaseStream { get; private set; }

        private ushort ToUInt16BE(byte[] bytes)
        {
            if (!BitConverter.IsLittleEndian)
            {
                return BitConverter.ToUInt16(bytes, 0);
            }
            return (ushort)((bytes[0] << 8) | (bytes[1]));
        }
        private short ToInt16BE(byte[] bytes)
        {
            if (!BitConverter.IsLittleEndian)
            {
                return BitConverter.ToInt16(bytes, 0);
            }
            return (short)((bytes[0] << 8) | (bytes[1]));
        }
        private uint ToUInt32BE(byte[] bytes)
        {
            if (!BitConverter.IsLittleEndian)
            {
                return BitConverter.ToUInt32(bytes, 0);
            }
            return (uint)((bytes[0] << 24) | (bytes[1] << 16) | (bytes[2] << 8) | (bytes[3]));
        }

        public void Close()
        {
            BaseStream.Close();
        }

        public async Task SeekAsync(long position)
        {
            BaseStream.Seek(position, SeekOrigin.Begin);
            await Task.CompletedTask;
        }

        public async Task SkipAsync(int count)
        {
            BaseStream.Seek(count, SeekOrigin.Current);
            await Task.CompletedTask;
        }

        public async Task<byte[]> ReadBytesAsync(int count)
        {
            var data = new byte[count];
            await BaseStream.ReadAsync(data, 0, count).ConfigureAwait(false);
            return data;
        }

        public async Task<ushort> ReadUInt16BEAsync()
        {
            var data = new byte[2];
            await BaseStream.ReadAsync(data, 0, data.Length).ConfigureAwait(false);
            return ToUInt16BE(data);
        }

        public async Task<short> ReadInt16BEAsync()
        {
            var data = new byte[2];
            await BaseStream.ReadAsync(data, 0, data.Length).ConfigureAwait(false);
            return ToInt16BE(data);
        }
        public async Task<uint> ReadUInt32BEAsync()
        {
            var data = new byte[4];
            await BaseStream.ReadAsync(data, 0, data.Length).ConfigureAwait(false);
            return ToUInt32BE(data);
        }

        public async Task<double> ReadInt32FixedBEAsync()
        {
            var hi = await ReadInt16BEAsync().ConfigureAwait(false);
            var lo = await ReadUInt16BEAsync().ConfigureAwait(false);

            var result = Math.Round(((hi) + ((double)lo / ushort.MaxValue)) * 10000) / 10000;
            return result;
        }

        public void Dispose()
        {
            BaseStream.Dispose();
        }
    }
}
