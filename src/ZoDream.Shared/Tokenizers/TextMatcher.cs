using System.Text.RegularExpressions;
using ZoDream.Shared.Interfaces;

namespace ZoDream.Shared.Tokenizers
{
    public class TextMatcher(string word, string replace = "") : ITextMatcher
    {
        public bool IsMatch(string text)
        {
            return text.Contains(word);
        }

        public bool TryMatch(string text, out int index, out int length)
        {
            index = text.IndexOf(word);
            length = word.Length;
            return index >= 0;
        }

        public bool TryMatch(string text, int begin, out int index, out int length)
        {
            index = text.IndexOf(word, begin);
            length = word.Length;
            return index >= 0;
        }

        public bool TryReplace(string text, out string result)
        {
            result = text.Replace(word, replace);
            return text != result;
        }
    }

    public class RegexMatcher(Regex pattern, string replace = "") : ITextMatcher
    {
        public RegexMatcher(string pattern, string replace = "")
            : this (new Regex(pattern), replace)
        {
            
        }

        public bool IsMatch(string text)
        {
            return pattern.IsMatch(text);
        }

        public bool TryMatch(string text, out int index, out int length)
        {
            var match = pattern.Match(text);
            if (!match.Success)
            {
                index = -1;
                length = 0;
                return false;
            }
            index = match.Index;
            length = match.Length;
            return true;
        }

        public bool TryMatch(string text, int begin, out int index, out int length)
        {
            var match = pattern.Match(text, begin);
            if (!match.Success)
            {
                index = -1;
                length = 0;
                return false;
            }
            index = match.Index;
            length = match.Length;
            return true;
        }

        public bool TryReplace(string text, out string result)
        {
            try
            {
                result = pattern.Replace(text, replace);
                return result != text;
            }
            catch (System.Exception)
            {
                result = string.Empty;
                return false;
            }
        }
    }
}
