using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZoDream.Reader.Models
{
    public class BookItem
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Cover { get; set; }

        public string FileName { get; set; }

        public long Position { get; set; } = 0;

        public static string RandomCover()
        {
            var rd = new Random();
            return $"Assets/cover{rd.Next(1, 11)}.jpg";
        }
    }
}
