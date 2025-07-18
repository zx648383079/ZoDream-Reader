﻿using System;
using System.IO;

namespace ZoDream.Shared.Interfaces
{
    public interface INovelReader : IDisposable
    {
        public INovelBasic ReadBasic();
        public INovelDocument Read();
    }

    public interface INovelWriter : IDisposable
    {

        public void Write(Stream output);
    }
}
