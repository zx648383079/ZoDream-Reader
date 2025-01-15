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

        public bool Eq(string text);

        public bool Empty();
    }
}
