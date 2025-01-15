using System.Collections.Generic;

namespace ZoDream.Shared.Script.Interfaces
{
    public interface IUrlObject: IBaseObject
    {
        public IUrlObject Method(string method);
        public IUrlObject Query(string key, string value);
        public IUrlObject Query(IDictionary<string, object> queries);

        public IUrlObject Proxy(string proxy);
        public IUrlObject Header(string key, string value);
        public IUrlObject Header(IDictionary<string, object> queries);

        public ITextObject Get();
        public ITextObject Post(IDictionary<string, object> body);

        public ITextObject Execute();
    }
}
