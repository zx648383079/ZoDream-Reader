namespace ZoDream.Shared.Script.Interfaces
{
    public interface IQueryableObject : IBaseObject
    {
        public IQueryableObject Query(string selector);

        public ITextObject Text();
    }
}
