using System;
using System.Data;

namespace ZoDream.Reader.Model
{
    public class BookItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }

        public string Description { get; set; }
        public string Author { get; set; }

        public BookSources Source { get; set; } = BookSources.本地;

        public BookKinds Kind { get; set; } = BookKinds.其他;

        public string Url { get; set; }

        public int Index { get; set; } = 0;

        public int Count { get; set; } = 0;

        public DateTime Time { get; set; } = DateTime.Now;

        public BookItem()
        {
            
        }

        public BookItem(string name)
        {
            Name = name;
        }

        public BookItem(string name, string image)
        {
            Image = image;
            Name = name;
        }

        public BookItem(string name, string image, string author, string description)
        {
            Image = image;
            Name = name;
            Author = author;
            Description = description;
        }

        public BookItem(string name, BookSources source, BookKinds kind, string url)
        {
            Name = name;
            Source = source;
            Kind = kind;
            Url = url;
        }

        public BookItem(IDataRecord reader)
        {
            Id = reader.GetInt16(0);
            Name = reader.GetString(1);
            Image = reader[2].ToString();
            Description = reader[3].ToString();
            Author = reader[4].ToString();;
            Source = (BookSources)Enum.Parse(typeof(BookSources), reader.GetString(5));
            Kind = (BookKinds)Enum.Parse(typeof(BookKinds), reader.GetString(6));
            Url = reader.GetString(7);
            Index = reader.GetInt32(8);
            var o = reader[9].ToString();
            Count = string.IsNullOrEmpty(o) ? 0 : reader.GetInt32(9);
            if (!string.IsNullOrEmpty(reader[10].ToString()))
            {
                Time = reader.GetDateTime(10);
            }
        }
    }
}
