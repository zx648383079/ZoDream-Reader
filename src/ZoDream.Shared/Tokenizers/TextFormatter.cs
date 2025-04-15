using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using ZoDream.Shared.Interfaces;
using ZoDream.Shared.Interfaces.Tokenizers;

namespace ZoDream.Shared.Tokenizers
{
    public class TextFormatter(ICanvasTheme theme) : ITextFormatter
    {
        public ICanvasTheme Theme => theme;
        public float FontWidth(float count)
        {
            return count * (theme.FontSize + theme.Spacing.X);
        }
        public float FontHeight()
        {
            return theme.FontSize + theme.Spacing.Y;
        }

        public int PerPageLineCount => (int)Math.Floor(theme.BodySize.Y / FontHeight());

        public Vector2 Compute(char code)
        {
            return theme.FontBound(code);
        }

        public INovelPageLine[] Compute(string text)
        {
            var max = theme.BodySize;
            var x = 0f;
            var y = 0f;
            var lineH = 0f;
            var line = new NovelPageLine(theme);
            var items = new List<INovelPageLine>
            {
                line
            };
            var index = 0;
            while (index < text.Length)
            {
                var code = text[index++];
                if (code == '\r')
                {
                    if (text.Length > index && text[index] == '\n')
                    {
                        index++;
                    }
                    line = new NovelPageLine(theme);
                    items.Add(line);
                    y += lineH;
                    lineH = 0;
                    x = 0;
                    continue;
                }
                if (code == '\n')
                {
                    line = new NovelPageLine(theme);
                    items.Add(line);
                    y += lineH;
                    lineH = 0;
                    x = 0;
                    continue;
                }
                var font = theme.FontBound(code);
                if (x + font.X > max.X)
                {
                    line = new NovelPageLine(theme)
                    {
                        new NovelPageChar(code.ToString())
                        {
                            Position = new(x, 0),
                            Size = font
                        }
                    };
                    items.Add(line);
                    y += lineH;
                    lineH = font.Y;
                    x = font.X;
                    continue;
                }
                line.Add(new NovelPageChar(code.ToString())
                {
                    Position = new(x, y),
                    Size = font
                });
                lineH = Math.Max(lineH, font.Y);
                x += font.X;
            }
            return [.. items];
        }

        public int ComputeLine(string text)
        {
            var maxW = theme.BodySize.X;
            var x = .0f;
            var lineNo = 1;
            var index = 0;
            while (index < text.Length)
            {
                var code = text[index ++];
                if (code == '\r')
                {
                    if (text.Length > index && text[index] == '\n')
                    {
                        index++;
                    }
                    lineNo ++;
                    x = 0;
                    continue;
                }
                if (code == '\n')
                {
                    lineNo ++;
                    x = 0;
                    continue;
                }
                var font = theme.FontBound(code);
                if (x + font.X > maxW)
                {
                    lineNo++;
                    x = font.X;
                    continue;
                }
                x += font.X;
            }
            return lineNo;
        }

        public int ComputeLine(ICharIterator input)
        {
            var pos = input.Position;
            var count = 0;
            while (true)
            {
                var line = input.ReadLine();
                if (line == null)
                {
                    break;
                }
                count += ComputeLine(line);
            }
            input.Seek(pos);
            return count;
        }

        public int ComputeLine(TextReader input)
        {
            var pos = input is StreamReader r ? r.BaseStream.Position : 0;
            var count = 0;
            while (true)
            {
                var line = input.ReadLine();
                if (line == null)
                {
                    break;
                }
                count += ComputeLine(line);
            }
            if (input is StreamReader rr)
            {
                rr.BaseStream.Position = pos;
            }
            return count;
        }

        public int ComputePage(string text)
        {
            return (int)Math.Ceiling((float)ComputeLine(text) / PerPageLineCount);
        }

        public int ComputePage(ICharIterator input)
        {
            return (int)Math.Ceiling((float)ComputeLine(input) / PerPageLineCount);
        }

        public int ComputePage(TextReader input)
        {
            return (int)Math.Ceiling((float)ComputeLine(input) / PerPageLineCount);
        }

        public static string? LineText(string text, out int next)
        {
            return LineText(text, 0, out next);
        }
        /// <summary>
        /// 获取一行
        /// </summary>
        /// <param name="text"></param>
        /// <param name="index"></param>
        /// <param name="next">下一行的开始位置，最后一行时，next = 0</param>
        /// <returns></returns>
        public static string? LineText(string text, int index, out int next)
        {
            if (index >= text.Length)
            {
                next = 0;
                return null;
            }
            var i = index;
            var end = text.Length;
            while (i < text.Length)
            {
                var code = text[i++];
                if (code == '\r')
                {
                    end = i - 1;
                    if (text.Length > i && text[i] == '\n')
                    {
                        i++;
                    }
                    next = i;
                    return text.Substring(index, end - index);
                }
                if (code == '\n')
                {
                    end = i - 1;
                    next = i;
                    return text.Substring(index, end - index);
                }
            }
            next = 0;
            return text.Substring(index, end - index);
        }
    }
}
