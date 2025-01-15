namespace ZoDream.Shared.Script.Interfaces
{
    public interface IBaseObject
    {

        public IBaseObject As(string name);

        public IBaseObject Clone();
    }
}
