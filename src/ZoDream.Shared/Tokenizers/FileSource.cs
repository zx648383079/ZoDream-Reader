using ZoDream.Shared.Interfaces;
using ZoDream.Shared.Interfaces.Entities;

namespace ZoDream.Shared.Tokenizers
{
    public class FileSource(string fileName) : INovelSource
    {
        public FileSource(INovelSourceEntity source)
            : this (source.FileName)
        {
            
        }
        public string FileName { get; set; } = fileName;
    }
}
