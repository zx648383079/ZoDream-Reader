using System;
using System.Buffers;
using System.IO;

namespace ZoDream.Shared.Extensions
{
    public static class StreamExtension
    {

        public static string ToBase64String(this Stream input, bool leaveOpen = false)
        {
            var length = (int)input.Length;
            var buffer = ArrayPool<byte>.Shared.Rent(length);
            try
            {
                input.Seek(0, SeekOrigin.Begin);
                input.ReadExactly(buffer, 0, length); 
                return Convert.ToBase64String(buffer, 0, length);
            }
            finally
            {
                if (!leaveOpen)
                {
                    input.Dispose();
                }
                ArrayPool<byte>.Shared.Return(buffer);
            }
        }
    }
}
