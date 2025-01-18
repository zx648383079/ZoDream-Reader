using System.Text.RegularExpressions;

namespace ZoDream.Shared.Script.Interfaces
{
    public interface ITextObject : IBaseObject
    {

        public IQueryableObject Html();

        public IQueryableObject Xml();

        public IUrlObject Url();

        public IQueryableObject Json();


        public IArrayObject Split(string tag);
        public IArrayObject Split(string tag, int count);

        public IArrayObject Match(string pattern);
        public ITextObject Match(string pattern, int group);
        public ITextObject Match(string pattern, string group);

        public IArrayObject Match(Regex pattern);
        public ITextObject Match(Regex pattern, int group);
        public ITextObject Match(Regex pattern, string group);

        public ITextObject Replace(string pattern, string replacement);
        public ITextObject Replace(Regex pattern, string replacement);

        public ITextObject Append(string text);
        public ITextObject Append(IBaseObject text);

        public bool Eq(string text);
    }
}
