using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Pager
{
    class Program
    {
        static void Main(string[] args)
        {
            var file = Console.ReadLine();
            var pager = new Pager();
            pager.Get(file);
            foreach (var item in pager.Titles)
            {
                Console.WriteLine(item.Title);
            }
            Console.ReadKey();
        }
    }
}
