﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZoDream.Shared.Interfaces;
using ZoDream.Shared.Models;
using ZoDream.Shared.Storage;

namespace ZoDream.Shared.Renders
{
    public class StreamIterator(string fileName) : ICharIterator
    {
        private readonly string _fileName = fileName;
        private readonly FileStream _reader = new(fileName, FileMode.Open);
        private Encoding? _encoding;
        private volatile bool _isLoading = false;

        public long Position => _reader.Position;

        public Task SeekAsync(long position)
        {
            return Task.Factory.StartNew(() =>
            {
                WaitUnlock();
                Seek(position);
            });
        }

        private void WaitUnlock()
        {
            while (_isLoading)
            {
                Thread.Sleep(50);
            }
        }

        public long Seek(long position)
        {
           return _reader.Seek(position, SeekOrigin.Begin);
        }

        public string? ReadLine()
        {
            return ReadLine(out _);
        }

        internal string? ReadLine(out long nextPosition)
        {
            _isLoading = true;
            GetEncoding();
            var bytes = new List<byte>();
            var isEnd = false;
            int bInt;
            while (true)
            {
                bInt = ReadByte();
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
                    var next = ReadByte();
                    if (next == 0x0A)
                    {
                        break;
                    }
                    if (next == -1)
                    {
                        isEnd = true;
                        break;
                    }
                    Seek(p);
                    break;
                }
                bytes.Add((byte)bInt);
            }
            nextPosition = Position;
            _isLoading = false;
            if (bytes.Count == 0)
            {
                return isEnd ? null : string.Empty;
            }
            return _encoding!.GetString([.. bytes]);
        }

        private void GetEncoding()
        {
            if (_encoding == null)
            {
                var oldPosition = Position;
                _encoding = TxtEncoder.GetEncoding(_reader);
                Seek(oldPosition);
            }
        }

        internal int ReadByte()
        {
            try
            {
                return _reader.ReadByte();
            }
            catch (IndexOutOfRangeException)
            {
                return -1;
            }
        }

        public void Dispose()
        {
            _reader?.Close();
        }

        public Task<string?> ReadLineAsync()
        {
            return Task.Factory.StartNew(() =>
            {
                WaitUnlock();
                return ReadLine();
            });
        }


        public Task<ReadLineItem> ReadLineAsync(long position)
        {
            return Task.Factory.StartNew(() =>
            {
                WaitUnlock();
                _isLoading = true;
                Seek(position);
                var str = ReadLine(out var nextPos);
                return new ReadLineItem(str, nextPos);
            });
        }

    }
}
