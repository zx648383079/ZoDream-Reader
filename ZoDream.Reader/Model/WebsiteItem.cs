using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZoDream.Reader.Model
{
    public class WebsiteItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }

        public WebsiteItem()
        {
            
        }

        public WebsiteItem(string name, string url)
        {
            Name = name;
            Url = url;
        }

        public WebsiteItem(IDataRecord reader)
        {
            Id = reader.GetInt32(0);
            Name = reader.GetString(1);
            Url = reader.GetString(2);
        }
    }
}
