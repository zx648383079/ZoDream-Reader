namespace ZoDream.Shared.Script.Interfaces
{
    public interface IBaseObject
    {

        public string Alias { get; }
        public IBaseObject Parent { get; }

        public IBaseObject As(string name);

        public IBaseObject Clone();

        public bool Empty();
        public IBaseObject Is(bool condition, IBaseObject trueResult);
        public IBaseObject Is(bool condition, IBaseObject trueResult, IBaseObject falseResult);
    }
}
