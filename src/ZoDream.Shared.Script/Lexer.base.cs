using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ZoDream.Shared.Script
{
    public partial class Lexer
    {

        protected readonly TextReader Reader;
        /// <summary>
        /// 读取多了需要，排个队列
        /// </summary>
        protected readonly Queue<Token> NextTokenQueue = new();
        /// <summary>
        /// 上一次获取到的Token
        /// </summary>
        public Token? CurrentToken { get; private set; }
        private int _lineIndex = 0;
        private int _columnIndex = 0;
        private int _charIndex = -1;
        // 上一个字符
        private int _lastChar = -1;
        // 当前的字符
        private int _currentChar = -1;
        // 指示下一次只获取当前的
        private bool _moveNextStop = false;

        public Token NextToken()
        {
            Token token;
            if (NextTokenQueue.Count > 0)
            {
                token = NextTokenQueue.Dequeue();
            }
            else
            {
                token = ReadToken();
            }
            CurrentToken = token;
            return token;
        }


        public void MoveBackChar()
        {
            _moveNextStop = true;
        }

        protected int ReadChar()
        {
            if (_moveNextStop)
            {
                _moveNextStop = false;
                return _currentChar;
            }
            _lastChar = _currentChar;
            _currentChar = Reader.Read();
            if (_currentChar == -1)
            {
                return _currentChar;
            }
            _charIndex++;
            if (_currentChar == '\n' && _lastChar == '\r')
            {
                return ReadChar();
            }
            if (IsNewLine(_currentChar))
            {
                _lineIndex++;
                _columnIndex = 0;
            }
            else
            {
                _columnIndex++;
            }
            return _currentChar;
        }

        protected bool IsNewLine(int code)
        {
            return code == '\r' || code == '\n';
        }


        protected bool IsNumeric(int code)
        {
            return code >= '0' && code <= '9';
        }

        protected bool IsHexNumeric(int code)
        {
            return IsNumeric(code) || (code >= 'a' && code <= 'f') ||
                    (code >= 'A' && code <= 'F');
        }

        /// <summary>
        /// 是否是大写字母
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        protected bool IsUpperAlphabet(int code)
        {
            return code >= 'A' && code <= 'Z';
        }

        /// <summary>
        /// 是否是小写字母
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        protected bool IsLowerAlphabet(int code)
        {
            return code >= 'a' && code <= 'z';
        }

        protected bool IsBracketOpen(int code)
        {
            return code switch
            {
                '(' or '{' or '[' => true,
                _ => false
            };
        }

        protected bool IsBracketClose(int code)
        {
            return code switch
            {
                ')' or '}' or ']' => true,
                _ => false
            };
        }

        /// <summary>
        /// 是否是字母
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        protected bool IsAlphabet(int code)
        {
            return IsUpperAlphabet(code) || IsLowerAlphabet(code);
        }

        /// <summary>
        /// 也包括换行
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        protected bool IsWhiteSpace(char code)
        {
            return char.IsWhiteSpace(code);
        }

        protected bool IsWhiteSpace(int code)
        {
            return IsWhiteSpace((char)code);
        }

        protected bool IsSeparator(int code)
        {
            return code is ',' or ';';
        }
    }
}
