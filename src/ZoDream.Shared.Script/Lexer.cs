using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ZoDream.Shared.Script
{
    public partial class Lexer
    {
        public Lexer(TextReader reader)
        {
            Reader = reader;
        }


        protected Token ReadToken()
        {
            int next;
            while (true)
            {
                var codeInt = ReadChar();
                if (codeInt == -1)
                {
                    return new Token(TokenType.Eof, string.Empty);
                }
                if (IsNewLine(codeInt))
                {
                    return new Token(TokenType.NewLine);
                }
                var code = (char)codeInt;
                if (IsWhiteSpace(code))
                {
                    continue;
                }
                if (code is '\'' or '"')
                {
                    return GetStringToken(code);
                }
                if (code is >= '0' and <= '9')
                {
                    return GetNumericToken(code);
                }
                if (IsSeparator(code))
                {
                    return new Token(TokenType.Separator, code.ToString());
                }
                if (IsBracketOpen(codeInt) || IsBracketClose(codeInt))
                {
                    return new Token(TokenType.Bracket, code.ToString());
                }
                if (code == '.')
                {
                    next = ReadChar();
                    if (next == '.')
                    {
                        return new Token(TokenType.DotDot, "..");
                    }
                    MoveBackChar();
                    return new Token(TokenType.Dot, code.ToString());
                }
                if (code == ':')
                {
                    return new Token(TokenType.Colon, code.ToString());
                }
                if (code == ',')
                {
                    return new Token(TokenType.Comma, code.ToString());
                }
                if (IsAlphabet(code) || code == '_')
                {
                    return GetNameToken(code);
                }
            }
        }

        private Token GetNameToken(char code)
        {
            var sb = new StringBuilder();
            sb.Append(code);
            while (true)
            {
                var codeInt = ReadChar();
                if (codeInt < 0)
                {
                    break;
                }
                if (IsNewLine(codeInt))
                {
                    MoveBackChar();
                    break;
                }
                var c = (char)codeInt;
                if (IsWhiteSpace(c))
                {
                    MoveBackChar();
                    break;
                }
                if (!(IsNumeric(codeInt) || IsAlphabet(codeInt)) || codeInt > 127)
                {
                    MoveBackChar();
                    break;
                }
                sb.Append(c);
            }
            return new Token(TokenType.Identifier, sb.ToString());
        }

        private Token GetNumericToken(char code)
        {
            var isHex = 0; // 是否是十六进制, 0 未判断 1 是小数 2 是进制
            var sb = new StringBuilder();
            sb.Append(code);
            while (true)
            {
                var codeInt = ReadChar();
                if (codeInt < 0)
                {
                    break;
                }
                if (codeInt == '.')
                {
                    var next = ReadChar();
                    if (next == '.')
                    {
                        NextTokenQueue.Enqueue(new Token(TokenType.DotDot, ".."));
                        return new Token(TokenType.String, sb.ToString());
                    }
                    isHex = 1;
                    sb.Append((char)codeInt);
                    if (next < 0)
                    {
                        break;
                    }
                    codeInt = next;
                }
                if (isHex == 0)
                {
                    if (codeInt != 'X' && codeInt != 'x' && !IsHexNumeric(codeInt))
                    {
                        MoveBackChar();
                        break;
                    }
                    if (!IsNumeric(codeInt))
                    {
                        isHex = 2;
                    }
                }
                else if (isHex == 1)
                {
                    if (!IsNumeric(codeInt))
                    {
                        MoveBackChar();
                        break;
                    }
                }
                else if (isHex == 2 && !IsHexNumeric(codeInt))
                {
                    MoveBackChar();
                    break;
                }
                sb.Append((char)codeInt);
            }
            return new Token(TokenType.String, sb.ToString());
        }

        private Token GetStringToken(char end)
        {
            var reverseCount = 0;
            var sb = new StringBuilder();
            while (true)
            {
                var codeInt = ReadChar();
                if (codeInt < 0)
                {
                    break;
                }
                if (codeInt == end && reverseCount % 2 == 0)
                {
                    break;
                }
                if (codeInt == '\\')
                {
                    reverseCount++;
                    if (reverseCount == 2)
                    {
                        sb.Append((char)codeInt);
                        reverseCount = 0;
                    }
                    continue;
                }
                reverseCount = 0;
                sb.Append((char)codeInt);
            }
            return new Token(TokenType.String, sb.ToString());
        }

    }
}
