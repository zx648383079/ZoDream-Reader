using System.Text;
using ZoDream.Shared.Interfaces;
using ZoDream.Shared.Interfaces.Entities;
using ZoDream.Shared.Tokenizers;

namespace ZoDream.Shared.Plugins.Txt
{
    public class TxtSource(string fileName, string charset) : FileSource(fileName), INovelSource
    {

        public TxtSource(INovelSourceEntity source)
            : this(source.FileName, source.Charset)
        {
            
        }


        public Encoding Encoding = TryParse(charset);



        private static Encoding TryParse(string charset)
        {
            try
            {
                return Encoding.GetEncoding(charset);
            }
            catch (System.Exception)
            {
                return Encoding.UTF8;
            }
        }
    }
}
