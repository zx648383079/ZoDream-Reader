using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZoDream.Reader.Model
{
    public class WebRuleItem
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }

        public string CatalogBegin { get; set; }

        public string CatalogEnd { get; set; }

        public string ChapterBegin { get; set; }

        public string ChapterEnd { get; set; }

        public string Replace { get; set; }

        public string AuthorBegin { get; set; }

        public string AuthorEnd { get; set; }

        public string DescriptionBegin { get; set; }

        public string DescriptionEnd { get; set; }

        public string CoverBegin { get; set; }

        public string CoverEnd { get; set; }

        public WebRuleItem()
        {
            
        }

        public WebRuleItem(IDataRecord reader)
        {
            Id = reader.GetInt32(0);
            Name = reader.GetString(1);
            Url = reader.GetString(2);
            CatalogBegin = reader.GetString(3);
            CatalogEnd = reader.GetString(4);
            ChapterBegin = reader.GetString(5);
            ChapterEnd = reader.GetString(6);
            Replace = reader.GetString(7);
            AuthorBegin = reader.GetString(8);
            AuthorEnd = reader.GetString(9);
            DescriptionBegin = reader.GetString(10);
            DescriptionEnd = reader.GetString(11);
            CoverBegin = reader.GetString(12);
            CoverEnd = reader.GetString(13);
        }
    }
}
