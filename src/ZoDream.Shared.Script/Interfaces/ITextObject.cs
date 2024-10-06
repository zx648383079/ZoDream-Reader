using System;

namespace ZoDream.Shared.Script.Interfaces
{
    public interface ITextObject : ICloneable
    {

        public IQueryableObject Html();

        public IQueryableObject Xml();

        public IUrlObject Url();

        public IQueryableObject Json();


        public IObjectCollection<ITextObject> Split(string tag);
        public IObjectCollection<ITextObject> Split(string tag, int count);

        public IObjectCollection<ITextObject> Match(string pattern);
        public ITextObject Match(string pattern, int group);
        public ITextObject Match(string pattern, string group);

        public bool Eq(string text);

        public bool Empty();
    }
}
