using System.IO;
using System.Numerics;
using ZoDream.Shared.Interfaces.Tokenizers;

namespace ZoDream.Shared.Interfaces
{
    public interface ITextFormatter
    {
        public ICanvasTheme Theme { get; }
        /// <summary>
        /// 计算一个字符的区域，宽高
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public Vector2 Compute(char code);
        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public INovelPageLine[] Compute(string text);
        /// <summary>
        /// 计算有多少行
        /// </summary>
        /// <param name="theme"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public int ComputeLine(string text);
        public int ComputeLine(ICharIterator input);
        public int ComputeLine(TextReader input);
        /// <summary>
        /// 统计有多少页
        /// </summary>
        /// <param name="theme"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public int ComputePage(string text);
        public int ComputePage(ICharIterator input);
        public int ComputePage(TextReader input);
    }
}
