using System.Numerics;

namespace ZoDream.Shared.Interfaces
{
    public interface ICanvasTheme: ICanvasControl
    {
        /// <summary>
        /// 对齐，0 居左 1 居中 3 居右
        /// </summary>
        public byte TextAlign { get; }
        public string FontFamily { get; }
        public byte FontSize { get; }
        /// <summary>
        /// 字体加粗，500 正常
        /// </summary>
        public ushort FontWeight { get; }
        public bool FontItalic { get; }
        public bool Underline { get; }
        /// <summary>
        /// x 左 y 上 z 右 w 下 
        /// </summary>
        public Vector4 Padding { get; }

        /// <summary>
        /// x 字间距 y 行间距
        /// </summary>
        public Vector2 Spacing { get; }
        /// <summary>
        /// 内容有效区域
        /// </summary>
        public Vector2 BodySize { get; }

        public Vector2 FontBound(char? code);
    }
}
