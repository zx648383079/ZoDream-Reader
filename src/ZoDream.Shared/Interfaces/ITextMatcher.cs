namespace ZoDream.Shared.Interfaces
{
    public interface ITextMatcher
    {
        public bool IsMatch(string text);
        public bool TryMatch(string text, out int index, out int length);
        public bool TryMatch(string text, int begin, out int index, out int length);
        public bool TryReplace(string text, out string result);
    }
}
