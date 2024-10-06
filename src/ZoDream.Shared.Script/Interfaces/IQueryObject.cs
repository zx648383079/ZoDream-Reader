using System;

namespace ZoDream.Shared.Script.Interfaces
{
    public interface IQueryableObject : ICloneable
    {
        public IQueryableObject Query(string selector);

        public ITextObject Text();
    }
}
