using System;
using System.IO;

namespace ZoDream.Shared.IO
{
    /// <summary>
    /// 嵌套使用请手动更新源进度
    /// </summary>
    public class PartialStream: Stream
    {
        /// <summary>
        /// 从当前位置获取剩余部分
        /// </summary>
        /// <param name="stream"></param>
        public PartialStream(Stream stream)
            : this (stream, stream.Position, stream.Length - stream.Position)
        {
            
        }

        public PartialStream(Stream stream, long byteLength)
            : this(stream, stream.Position, byteLength)
        { 
        }

        public PartialStream(Stream stream, long beginPosition, long byteLength)
        {
            _byteLength = byteLength;
            if (stream is not PartialStream ps)
            {
                BaseStream = stream;
                _current = _beginPosition = beginPosition;
                return;
            }
            _syncStream = ps;
            BaseStream = ps.BaseStream;
            _current = _beginPosition = beginPosition + ps._beginPosition;
        }

        private readonly PartialStream? _syncStream;
        public readonly Stream BaseStream;
        private readonly bool _leaveStreamOpen = true;
        private readonly long _beginPosition;
        private readonly long _byteLength;

        private long _current;

        private long EndPosition => _beginPosition + _byteLength;

        public override bool CanRead => BaseStream.CanRead;

        public override bool CanSeek => BaseStream.CanSeek;

        public override bool CanWrite => false;

        public override long Length => _byteLength;

        public override long Position {
            get => _current - _beginPosition;
            set {
                Seek(value, SeekOrigin.Begin);
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            var len = (int)Math.Min(count, EndPosition - _current);
            if (len <= 0)
            {
                return 0;
            }
            BaseStream.Seek(_current, SeekOrigin.Begin);
            var res = BaseStream.Read(buffer, offset, len);
            SyncPosition(_current + res);
            return res;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            var min = _beginPosition;
            var max = _beginPosition + _byteLength;
            var pos = origin switch
            {
                SeekOrigin.Current => BaseStream.Position + offset,
                SeekOrigin.End => _beginPosition + _byteLength + offset,
                _ => _beginPosition + offset,
            };
            SyncPosition(Math.Min(Math.Max(pos, min), max));
            return _current - min;
        }

        private void SyncPosition(long current)
        {
            _current = current;
            _syncStream?.SyncPosition(current);
        }

        public override void Flush()
        {
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException(string.Empty);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException(string.Empty);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (_leaveStreamOpen == false)
            {
                BaseStream.Dispose();
            }
        }

    }
}
