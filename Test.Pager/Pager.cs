using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ZoDream.Helper.Local;

namespace Test.Pager
{
    public class Pager
    {
        public List<TitleItem> Titles { get; set; } = new List<TitleItem>();

        public void Get(string file)
        {
            if (!File.Exists(file))
            {
                return;
            }
            var fs = new FileStream(file, FileMode.Open);
            var encoder = new TxtEncoder();
            var sr = new StreamReader(fs, encoder.GetEncoding(fs));
            string line;
            while (null != (line = sr.ReadLine()))
            {
                _addTitle(line);
            }
            sr.Close();
        }

        private void _addTitle(string line)
        {
            var regexs = new Regex[]
            {
                new Regex(@"^\s+(第.+章.{0,40})$")
            };
            foreach (var item in regexs)
            {
                if (item.IsMatch(line))
                {
                    Titles.Add(new TitleItem()
                    {
                        Title = Regex.Match(line, @"^\s+(第.+章.{0,40})").Groups[1].Value
                    });
                }
            }
        }
    }

    public class TitleItem
    {
        public string Title { get; set; }

        public int Line { get; set; }

        public int Index { get; set; }
    }
}
