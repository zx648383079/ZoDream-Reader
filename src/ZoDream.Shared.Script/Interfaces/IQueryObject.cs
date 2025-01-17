using System;

namespace ZoDream.Shared.Script.Interfaces
{
    public interface IQueryableObject : IBaseObject
    {
        public IArrayObject Map(Func<IBaseObject, IBaseObject> func);

        public IQueryableObject Query(string selector);


        public IBaseObject Attr(string name);

        public ITextObject Href();

        public ITextObject Text();
    }
}
