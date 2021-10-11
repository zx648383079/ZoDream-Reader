using System;
using System.Collections.Generic;
using System.Text;

namespace ZoDream.Shared.Models
{
    public class ReadLineItem
    {
        public string? Content { get; set; }

        public long NextPosition { get; set; }

        public bool IsEndLine => Content == null;

        public ReadLineItem(string? con)
        {
            Content = con;
        }

        public ReadLineItem(string? con, long pos)
        {
            Content = con;
            NextPosition = pos;
        }

        public override string ToString()
        {
            return Content == null ? string.Empty : Content;
        }
    }
}
